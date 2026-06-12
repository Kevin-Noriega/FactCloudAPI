using Microsoft.EntityFrameworkCore;
using NubeeAPI.Data;
using NubeeAPI.Models;
using NubeeAPI.Models.Pos;
using NubeeAPI.Services;
using NubeeAPI.Utils.Exceptions;
using System.Text;
using static NubeeAPI.Models.Factura;

namespace NubeeAPI.Services.Facturas
{
    /// <inheritdoc cref="IPosFacturacionService"/>
    public class PosFacturacionService : IPosFacturacionService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PosFacturacionService> _logger;

        // NIT estándar DIAN para "Consumidor Final".
        private const string ConsumidorFinalNit = "222222222222";

        public PosFacturacionService(ApplicationDbContext context, ILogger<PosFacturacionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Factura> EmitirDesdeVentaAsync(PosVenta venta, int usuarioId, CancellationToken ct = default)
        {
            if (venta.Detalles == null || venta.Detalles.Count == 0)
                throw new BusinessException("La venta no tiene ítems para facturar.");

            // 1. Sólo se pueden facturar ítems que referencian un producto del catálogo:
            //    DetalleFactura.ProductoId es obligatorio para la DIAN.
            if (venta.Detalles.Any(d => d.ProductoId == null))
                throw new BusinessException(
                    "La venta contiene ítems manuales (sin producto del catálogo) y no puede " +
                    "emitirse como factura electrónica. Registra esos ítems como productos.");

            // 2. Resolución DIAN vigente del negocio del usuario.
            var usuario = await _context.Usuarios
                .Include(u => u.Negocio)
                    .ThenInclude(n => n!.Resoluciones)
                .FirstOrDefaultAsync(u => u.Id == usuarioId, ct);

            if (usuario?.Negocio == null)
                throw new BusinessException(
                    "Tu cuenta no tiene un negocio configurado. Complétalo en Ajustes → Mi negocio.");

            var resolucion = usuario.Negocio.ResolucionActiva
                ?? throw new BusinessException(
                    "No tienes una resolución DIAN activa y vigente para emitir facturas desde el POS.");

            // 3. Cliente "Consumidor Final" (se crea una vez por usuario y se reutiliza).
            var cliente = await ObtenerOCrearConsumidorFinalAsync(usuario, ct);

            // 4. Construir la factura (cabecera + detalles).
            var factura = new Factura
            {
                UsuarioId = usuarioId,
                ClienteId = cliente.Id,
                FechaRegistro = DateTime.Now,
                FechaEmision = venta.Fecha,
                TipoAmbiente = resolucion.TipoAmbiente,
                NumeroAutorizacion = resolucion.NumeroAutorizacion,
                FechaInicioAutorizacion = resolucion.FechaInicio,
                FechaFinAutorizacion = resolucion.FechaFin,
                RangoNumeracionDesde = resolucion.RangoDesde,
                RangoNumeracionHasta = resolucion.RangoHasta,
                ClaveTecnica = resolucion.ClaveTecnica,
                FactusRangoId = resolucion.FactusRangoId ?? 0,
                Prefijo = resolucion.Prefijo,
                Estado = EstadoFactura.Emitida,
                FormaPago = "1", // POS = contado
                MedioPago = DeducirMedioPago(venta),
                Subtotal = venta.Subtotal,
                TotalIVA = venta.Impuestos,
                TotalINC = 0,
                TotalFactura = venta.Total,
                MontoPagado = venta.Total,
                FechaPago = venta.Fecha,
                Observaciones = $"Venta POS #{venta.NumeroVenta}",
                DetalleFacturas = new List<DetalleFactura>(),
            };

            foreach (var d in venta.Detalles)
            {
                var bruto = d.Cantidad * d.PrecioUnitario;
                var valorDescuento = Math.Round(bruto * (d.Descuento / 100m), 2);
                var subtotalLinea = Math.Round(bruto - valorDescuento, 2);

                factura.DetalleFacturas.Add(new DetalleFactura
                {
                    ProductoId = d.ProductoId!.Value,
                    Descripcion = d.Nombre,
                    UnidadMedida = "Unidad",
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    PorcentajeDescuento = d.Descuento,
                    ValorDescuento = valorDescuento,
                    SubtotalLinea = subtotalLinea,
                    TotalLinea = d.TotalLinea,
                });
            }

            factura.CalcularFechas();

            // 5. Numeración secuencial + CUFE/QR/XML dentro de una transacción
            //    SERIALIZABLE (mismo criterio que la facturación normal: evita que
            //    dos emisiones simultáneas tomen el mismo consecutivo).
            const int maxIntentos = 4;
            for (int intento = 1; ; intento++)
            {
                await using var tx = await _context.Database
                    .BeginTransactionAsync(System.Data.IsolationLevel.Serializable, ct);
                try
                {
                    var ultimoNumero = await _context.Facturas
                        .Where(f => f.UsuarioId == usuarioId
                                 && f.Prefijo == factura.Prefijo
                                 && f.Estado != EstadoFactura.Borrador)
                        .OrderByDescending(f => f.Id)
                        .Select(f => f.NumeroFactura)
                        .FirstOrDefaultAsync(ct);

                    long siguiente = resolucion.RangoDesde;
                    if (!string.IsNullOrEmpty(ultimoNumero) && long.TryParse(ultimoNumero, out long ultimo))
                        siguiente = ultimo + 1;

                    if (siguiente > resolucion.RangoHasta)
                        throw new BusinessException(
                            $"Rango de numeración agotado (hasta {resolucion.RangoHasta}). " +
                            "Solicita una nueva resolución a la DIAN.");

                    factura.NumeroFactura = siguiente.ToString();

                    try
                    {
                        factura.Cufe = CufeService.GenerarCUFE(factura);
                        factura.GenerarQRCode();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex,
                            "No se pudo generar CUFE para factura POS {Numero}", factura.NumeroFactura);
                    }

                    try
                    {
                        var xml = XmlFacturaGenerator.GenerarXml(factura);
                        factura.XmlBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(xml));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex,
                            "Error generando XML en factura POS {Numero}", factura.NumeroFactura);
                        factura.XmlBase64 = null;
                    }

                    _context.Facturas.Add(factura);
                    await _context.SaveChangesAsync(ct);

                    // Enlazar la venta con la factura. Tras un retry el ChangeTracker
                    // se limpió y `venta` quedó desacoplada: adjuntamos y marcamos sólo
                    // la columna FacturaId para que el UPDATE persista en ambos casos.
                    venta.FacturaId = factura.Id;
                    if (_context.Entry(venta).State == EntityState.Detached)
                        _context.Attach(venta);
                    _context.Entry(venta).Property(v => v.FacturaId).IsModified = true;
                    await _context.SaveChangesAsync(ct);

                    await tx.CommitAsync(ct);
                    break; // éxito
                }
                catch (BusinessException)
                {
                    await tx.RollbackAsync(ct);
                    throw;
                }
                catch (Exception ex) when (intento < maxIntentos && EsConflictoNumeracion(ex))
                {
                    await tx.RollbackAsync(ct);
                    _context.ChangeTracker.Clear();
                    _logger.LogWarning(ex,
                        "Conflicto de numeración emitiendo factura POS (intento {Intento}/{Max}); reintentando",
                        intento, maxIntentos);
                    await Task.Delay(50 * intento, ct);
                }
            }

            return factura;
        }

        private async Task<Cliente> ObtenerOCrearConsumidorFinalAsync(Usuario usuario, CancellationToken ct)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(
                c => c.UsuarioId == usuario.Id && c.NumeroIdentificacion == ConsumidorFinalNit, ct);

            if (cliente != null)
                return cliente;

            cliente = new Cliente
            {
                UsuarioId = usuario.Id,
                Nombre = "Consumidor Final",
                TipoIdentificacion = "CC",
                NumeroIdentificacion = ConsumidorFinalNit,
                TipoPersona = "Natural",
                Departamento = "Bogotá D.C.",
                Ciudad = "Bogotá",
                Direccion = "No aplica",
                Pais = "CO",
                Correo = string.IsNullOrWhiteSpace(usuario.Correo)
                    ? "consumidorfinal@factcloud.co"
                    : usuario.Correo,
                EsProveedor = false,
                Activo = true,
            };
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync(ct);
            return cliente;
        }

        // POS contado: si hay más de un medio, prioriza el de mayor uso real.
        private static string DeducirMedioPago(PosVenta v)
        {
            if (v.Efectivo > 0) return "10";   // Efectivo
            if (v.Tarjeta > 0) return "48";    // Tarjeta crédito
            if (v.PagosLinea > 0) return "42"; // Transferencia
            return "ZZZ";                        // Otro
        }

        private static bool EsConflictoNumeracion(Exception ex)
        {
            for (var e = ex; e != null; e = e.InnerException)
            {
                if (e is DbUpdateException) return true;
                var numberProp = e.GetType().GetProperty("Number");
                if (numberProp?.GetValue(e) is int n && (n == 1205 || n == 3960))
                    return true;
                var msg = e.Message;
                if (msg.Contains("deadlock", StringComparison.OrdinalIgnoreCase) ||
                    msg.Contains("serializ", StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
