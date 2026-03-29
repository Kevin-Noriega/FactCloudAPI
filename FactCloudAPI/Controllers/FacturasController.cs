using FactCloudAPI.Data;
using FactCloudAPI.Models;
using FactCloudAPI.Models.DTOs;
using FactCloudAPI.Services;
using FactCloudAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace FactCloudAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FacturasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IHubContext<NotificacionesHub> _hub;
        private readonly ILogger<FacturasController> _logger;
        private readonly ISuscripcionService _suscripcionService; // ← NUEVO

        public FacturasController(
            ApplicationDbContext context,
            IEmailService emailService,
            IHubContext<NotificacionesHub> hub,
            ILogger<FacturasController> logger,
            ISuscripcionService suscripcionService) // ← NUEVO
        {
            _context = context;
            _emailService = emailService;
            _hub = hub;
            _logger = logger;
            _suscripcionService = suscripcionService; // ← NUEVO
        }

        // ==================== HELPERS PRIVADOS ====================

        private int? ObtenerUsuarioId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : null;
        }

        private bool FacturaExists(int id) =>
            _context.Facturas.Any(e => e.Id == id);

        // ==================== CRUD BÁSICO ====================

        // GET: api/Facturas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> ObtenerFacturas()
        {
            var usuarioId = ObtenerUsuarioId();
            if (usuarioId == null)
                return Unauthorized(new { message = "Token inválido o sin claim de usuario" });


            var facturas = await _context.Facturas
                .Include(f => f.Cliente)
                    .Include(f => f.Usuario)
                .Where(f => f.UsuarioId == usuarioId)
                .OrderByDescending(f => f.FechaEmision)
                .Select(f => new
                {
                    f.Id,
                    f.NumeroFacturaCompleto,
                    f.NumeroFactura,
                    f.Prefijo,
                    f.FechaEmision,
                    f.FechaVencimiento,
                    f.Estado,
                    f.TipoAmbiente,
                    f.TipoFactura,
                    f.Subtotal,
                    f.TotalIVA,
                    f.TotalINC,
                    f.TotalICA,
                    f.TotalDescuentos,
                    f.TotalRetenciones,
                    f.TotalFactura,
                    f.FormaPago,
                    f.MedioPago,
                    f.EnviadaDIAN,
                    f.HorasRestantesEnvioDIAN,
                    f.Cufe,
                    Cliente = new
                    {
                        f.Cliente!.Id,
                        f.Cliente.Nombre,
                        f.Cliente.Apellido,
                        f.Cliente.NumeroIdentificacion
                    }
                })
                .ToListAsync();

            return Ok(facturas);
        }

        // GET: api/Facturas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Factura>> ObtenerFactura(int id)
        {
            var usuarioId = ObtenerUsuarioId();
            if (usuarioId == null)
                return Unauthorized();

            var factura = await _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.Usuario)
                .Include(f => f.DetalleFacturas!)
                    .ThenInclude(d => d.Producto)
                .Include(f => f.NotasDebito)
                .FirstOrDefaultAsync(f => f.Id == id && f.UsuarioId == usuarioId); // ✅ Filtro por dueño

            if (factura == null)
                return NotFound(new { mensaje = "Factura no encontrada" });

            return Ok(factura);
        }

        // POST: api/Facturas
        [HttpPost]
        public async Task<ActionResult<Factura>> CrearFactura([FromBody] Factura factura)
        {
            var usuarioId = ObtenerUsuarioId();
            if (usuarioId == null)
                return Unauthorized();

            // ── 1. Cargar usuario con su negocio y resoluciones activas ──────────
            var usuario = await _context.Usuarios
                .Include(u => u.Negocio)
                    .ThenInclude(n => n.Resoluciones)
                .FirstOrDefaultAsync(u => u.Id == usuarioId.Value);

            if (usuario == null)
                return Unauthorized(new { mensaje = "Usuario no encontrado." });

            if (usuario.Negocio == null)
                return BadRequest(new
                {
                    mensaje = "Tu cuenta no tiene un negocio configurado. " +
                              "Completa tu perfil en Ajustes → Mi negocio.",
                    codigo = "SIN_NEGOCIO"
                });

            // ── 2. Obtener resolución DIAN vigente ────────────────────────────────
            var resolucion = usuario.Negocio.ResolucionActiva;

            if (resolucion == null)
                return BadRequest(new
                {
                    mensaje = "No tienes una resolución DIAN activa. " +
                              "Configura tu resolución en Ajustes → Facturación electrónica.",
                    codigo = "SIN_RESOLUCION"
                });

            if (!resolucion.EstaVigente)
                return BadRequest(new
                {
                    mensaje = $"La resolución DIAN venció el {resolucion.FechaFin:dd/MM/yyyy}. " +
                               "Solicita una nueva resolución a la DIAN.",
                    codigo = "RESOLUCION_VENCIDA",
                    fechaVencimiento = resolucion.FechaFin
                });

            // ── 3. Asignar campos del servidor (nunca confiar en el body) ─────────
            factura.UsuarioId = usuarioId.Value;
            factura.FechaRegistro = DateTime.Now;
            factura.TipoAmbiente = resolucion.TipoAmbiente;
            factura.NumeroAutorizacion = resolucion.NumeroAutorizacion;
            factura.FechaInicioAutorizacion = resolucion.FechaInicio;
            factura.FechaFinAutorizacion = resolucion.FechaFin;
            factura.RangoNumeracionDesde = resolucion.RangoDesde;
            factura.RangoNumeracionHasta = resolucion.RangoHasta;
            factura.ClaveTecnica = resolucion.ClaveTecnica;
            factura.Prefijo ??= resolucion.Prefijo;

            // ── 4. Numeración secuencial dentro del rango autorizado ──────────────
            // ⚠️ Se ordena por Id DESC para obtener el último consecutivo emitido.
            // Se excluyen Borradores porque no consumen numeración.
            var ultimoNumero = await _context.Facturas
                .Where(f => f.UsuarioId == usuarioId.Value
                         && f.Prefijo == factura.Prefijo
                         && f.Estado != "Borrador")
                .OrderByDescending(f => f.Id)
                .Select(f => f.NumeroFactura)
                .FirstOrDefaultAsync();

            long siguiente = resolucion.RangoDesde;
            if (!string.IsNullOrEmpty(ultimoNumero) && long.TryParse(ultimoNumero, out long ultimo))
                siguiente = ultimo + 1;

            if (siguiente > resolucion.RangoHasta)
                return BadRequest(new
                {
                    mensaje = $"Rango de numeración agotado (hasta {resolucion.RangoHasta}). " +
                               "Solicita una nueva resolución a la DIAN.",
                    codigo = "RANGO_AGOTADO",
                    rangoHasta = resolucion.RangoHasta
                });

            factura.NumeroFactura = siguiente.ToString();

            // ── 5. Manejar estado: Borrador vs Emitida ────────────────────────────
            // "Borrador"  → guardado sin numeración definitiva ni CUFE ni XML
            // "Pendiente" → emitida, lista para enviar a la DIAN
            bool esBorrador = factura.Estado == "Borrador";
            if (!esBorrador)
                factura.Estado = "Emitida";

            // ── 6. Calcular fechas (hora UTC-0500, FechaVencimiento, plazo DIAN) ──
            factura.CalcularFechas();

            // ── 7. CUFE y QR — solo facturas emitidas ────────────────────────────
            if (!esBorrador)
            {
                try
                {
                    factura.Cufe = CufeService.GenerarCUFE(factura);
                    factura.GenerarQRCode();
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex,
                        "No se pudo generar CUFE para factura {Numero}", factura.NumeroFactura);
                    // No bloqueamos — se puede regenerar con PUT /api/facturas/{id}/regenerar
                }

                // ── 8. XML UBL 2.1 — solo facturas emitidas ──────────────────────
                try
                {
                    var xml = XmlFacturaGenerator.GenerarXml(factura);
                    factura.XmlBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(xml));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Error generando XML en emisión de factura {Numero}", factura.NumeroFactura);
                    factura.XmlBase64 = null;
                    // ⚠️ Factura queda sin XML — registrar para revisión manual
                }
            }

            // ── 9. Persistir ──────────────────────────────────────────────────────
            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();

            // ── 10. Notificación en tiempo real via SignalR ───────────────────────
            if (!esBorrador)
            {
                await _hub.Clients.All.SendAsync("FacturaCreada", new
                {
                    id = factura.Id,
                    numero = factura.NumeroFacturaCompleto,
                    total = factura.TotalFactura,
                    fecha = factura.FechaRegistro,
                    ambiente = factura.TipoAmbiente == 2 ? "Pruebas" : "Producción"
                });
            }

            // ── 11. Respuesta ─────────────────────────────────────────────────────
            return CreatedAtAction(nameof(ObtenerFactura), new { id = factura.Id }, new
            {
                factura.Id,
                factura.NumeroFacturaCompleto,
                factura.Estado,
                factura.Cufe,
                factura.QRCode,
                factura.FechaLimiteEnvioDIAN,
                factura.HorasRestantesEnvioDIAN,
                xmlGenerado = !string.IsNullOrEmpty(factura.XmlBase64),
                diasVigencia = resolucion.DiasRestantes,
                ambiente = resolucion.TipoAmbiente == 2 ? "Pruebas" : "Producción"
            });
        }
    


       
        // PUT: api/Facturas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarFactura(int id, [FromBody] Factura factura)
        {
            if (id != factura.Id)
                return BadRequest(new { mensaje = "El ID de la ruta no coincide con el del cuerpo" });

            var usuarioId = ObtenerUsuarioId();
            if (usuarioId == null)
                return Unauthorized();

            // ✅ Verificar propiedad antes de editar
            var facturaExistente = await _context.Facturas
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id && f.UsuarioId == usuarioId);

            if (facturaExistente == null)
                return NotFound(new { mensaje = "Factura no encontrada" });

            // ✅ No permitir editar facturas ya enviadas a la DIAN
            if (facturaExistente.EnviadaDIAN)
                return BadRequest(new { mensaje = "No se puede modificar una factura ya enviada a la DIAN" });

            factura.UsuarioId = usuarioId.Value;
            factura.CalcularFechas();

            // Regenerar CUFE y XML si los datos cambian
            try
            {
                factura.Cufe = CufeService.GenerarCUFE(factura);
                factura.GenerarQRCode();
                var xml = XmlFacturaGenerator.GenerarXml(factura);
                factura.XmlBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(xml));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error regenerando CUFE/XML al actualizar factura {Id}", id);
            }

            try
            {
                _context.Entry(factura).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacturaExists(id))
                    return NotFound();
                throw;
            }
        }

        // DELETE: api/Facturas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarFactura(int id)
        {
            var usuarioId = ObtenerUsuarioId();
            if (usuarioId == null)
                return Unauthorized();

            var factura = await _context.Facturas
                .FirstOrDefaultAsync(f => f.Id == id && f.UsuarioId == usuarioId);

            if (factura == null)
                return NotFound(new { mensaje = "Factura no encontrada" });

            // ✅ No permitir eliminar facturas ya enviadas o validadas por la DIAN
            if (factura.EnviadaDIAN || factura.Estado == "Validada")
                return BadRequest(new
                {
                    mensaje = "No se puede eliminar una factura enviada o validada por la DIAN. Use una Nota Crédito."
                });

            _context.Facturas.Remove(factura);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ==================== ENVÍO ====================

        // POST: api/Facturas/5/enviar-cliente
        [HttpPost("{id}/enviar-cliente")]
        public async Task<IActionResult> EnviarFacturaAlCliente(int id)
        {
            var usuarioId = ObtenerUsuarioId();
            if (usuarioId == null)
                return Unauthorized();

            var factura = await _context.Facturas
                .Include(f => f.Cliente)
                .FirstOrDefaultAsync(f => f.Id == id && f.UsuarioId == usuarioId);

            if (factura == null)
                return NotFound(new { mensaje = "Factura no encontrada" });

            if (string.IsNullOrEmpty(factura.Cliente?.Correo))
                return BadRequest(new { mensaje = "El cliente no tiene correo registrado" });

            if (factura.EnviadaCliente)
                return BadRequest(new { mensaje = "Esta factura ya fue enviada al cliente" });

            await _emailService.EnviarFacturaCliente(id);

            return Ok(new
            {
                mensaje = "Factura enviada exitosamente al cliente",
                correo = factura.Cliente.Correo,
                fecha = DateTime.Now,
                numeroFactura = factura.NumeroFacturaCompleto
            });
        }

        // POST: api/Facturas/5/enviar-dian
        [HttpPost("{id}/enviar-dian")]
        public async Task<IActionResult> EnviarADIAN(int id)
        {
            var usuarioId = ObtenerUsuarioId();
            if (usuarioId == null)
                return Unauthorized();

            var factura = await _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.Usuario)
                .FirstOrDefaultAsync(f => f.Id == id && f.UsuarioId == usuarioId);

            if (factura == null)
                return NotFound(new { mensaje = "Factura no encontrada" });

            if (factura.EnviadaDIAN)
                return BadRequest(new { mensaje = "Esta factura ya fue enviada a la DIAN" });

            if (!factura.DentroPlazoEnvioDIAN)
                return BadRequest(new
                {
                    mensaje = "Factura fuera del plazo de 48 horas para envío a la DIAN",
                    horasVencida = factura.HorasRestantesEnvioDIAN
                });

            // ✅ Validar que el XML esté generado antes de enviar
            if (string.IsNullOrEmpty(factura.XmlBase64))
                return BadRequest(new
                {
                    mensaje = "El XML de la factura no está generado. Regenere la factura.",
                    accion = $"PUT api/Facturas/{id}"
                });

            // ✅ Validar que el CUFE esté calculado
            if (string.IsNullOrEmpty(factura.Cufe))
                return BadRequest(new { mensaje = "El CUFE no está calculado. Regenere la factura." });

            try
            {
                // TODO: Aquí irá el envío real al Web Service DIAN (SendBillAsync)
                // cuando tengas el certificado digital. Por ahora se registra el intento.
                factura.EnviadaDIAN = true;
                factura.FechaEnvioDIAN = DateTime.Now;
                factura.Estado = "Enviada";
                factura.RespuestaDIAN = factura.TipoAmbiente == 2
                    ? "Ambiente de pruebas — integración WS pendiente"
                    : "Pendiente integración WS DIAN (SendBillAsync)";

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    mensaje = factura.TipoAmbiente == 2
                        ? "Registrado en ambiente de pruebas"
                        : "Factura enviada a la DIAN",
                    cufe = factura.Cufe,
                    fechaEnvio = factura.FechaEnvioDIAN,
                    ambiente = factura.TipoAmbiente == 2 ? "Pruebas" : "Producción"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar factura {Id} a DIAN", id);
                return BadRequest(new { mensaje = "Error al enviar a la DIAN", error = ex.Message });
            }
        }

        // ==================== PAGO ====================

        // PATCH: api/Facturas/5/pago
        [HttpPatch("{id}/pago")]
        public async Task<IActionResult> RegistrarPago(int id, [FromBody] FacturaPagoDto pago)
        {
            if (pago == null)
                return BadRequest(new { mensaje = "Datos de pago inválidos" });

            var usuarioId = ObtenerUsuarioId();
            if (usuarioId == null)
                return Unauthorized();

            var factura = await _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.DetalleFacturas!)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(f => f.Id == id && f.UsuarioId == usuarioId);

            if (factura == null)
                return NotFound(new { mensaje = "Factura no encontrada" });

            if (factura.Estado == "Pagada")
                return BadRequest(new { mensaje = "Esta factura ya está pagada" });

            // ✅ Aplicar cambios de pago
            factura.Estado = pago.Estado ?? factura.Estado;
            // ✅ Códigos DIAN: "10"=Efectivo, "42"=Transferencia, "48"=Tarjeta crédito, "ZZZ"=Otro
            factura.MedioPago = pago.MedioPago ?? factura.MedioPago;
            // ✅ Códigos DIAN: "1"=Contado, "2"=Crédito
            factura.FormaPago = pago.FormaPago ?? factura.FormaPago;
            factura.Observaciones = pago.Observaciones ?? factura.Observaciones;

            if (pago.MontoPagado > 0)
                factura.MontoPagado = pago.MontoPagado;

            factura.FechaPago = pago.FechaPago != default ? pago.FechaPago : DateTime.Now;

            // ✅ Un solo generador de CUFE usando ClaveTecnica del modelo (no hardcodeada)
            try
            {
                factura.Cufe = CufeService.GenerarCUFE(factura);
                factura.GenerarQRCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error regenerando CUFE en pago de factura {Id}", id);
            }

            // ✅ Regenerar XML con datos de pago actualizados
            try
            {
                var xml = XmlFacturaGenerator.GenerarXml(factura);
                factura.XmlBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(xml));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error regenerando XML en pago de factura {Id}", id);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                factura.Id,
                factura.NumeroFacturaCompleto,
                factura.Estado,
                factura.TotalFactura,
                factura.MontoPagado,
                factura.FechaPago,
                // ✅ Descriptores legibles del código DIAN
                medioPagoDescripcion = factura.MedioPago switch
                {
                    "10" => "Efectivo",
                    "42" => "Transferencia bancaria",
                    "47" => "Débito bancario",
                    "48" => "Tarjeta crédito",
                    "ZZZ" => "Otro",
                    _ => factura.MedioPago
                },
                factura.Cufe,
                factura.QRCode
            });
        }

        // ==================== DESCARGA DE ARCHIVOS ====================

        // GET: api/Facturas/5/xml
        [HttpGet("{id}/xml")]
        public async Task<IActionResult> DescargarXml(int id)
        {
            var usuarioId = ObtenerUsuarioId();
            if (usuarioId == null)
                return Unauthorized();

            var factura = await _context.Facturas
                .FirstOrDefaultAsync(f => f.Id == id && f.UsuarioId == usuarioId);

            if (factura == null)
                return NotFound(new { mensaje = "Factura no encontrada" });

            if (string.IsNullOrEmpty(factura.XmlBase64))
                return BadRequest(new
                {
                    mensaje = "XML no generado. Actualice la factura para regenerarlo.",
                    accion = $"PUT api/Facturas/{id}"
                });

            try
            {
                var bytes = Convert.FromBase64String(factura.XmlBase64);
                return File(bytes, "application/xml", $"Factura_{factura.NumeroFacturaCompleto}.xml");
            }
            catch
            {
                return BadRequest(new { mensaje = "El XML almacenado está corrupto. Regenere la factura." });
            }
        }

        // GET: api/Facturas/5/pdf
        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> DescargarPdf(int id)
        {
            var usuarioId = ObtenerUsuarioId();
            if (usuarioId == null)
                return Unauthorized();

            var factura = await _context.Facturas
                .Include(f => f.Cliente)
                .FirstOrDefaultAsync(f => f.Id == id && f.UsuarioId == usuarioId);

            if (factura == null)
                return NotFound(new { mensaje = "Factura no encontrada" });

            var path = $"./PDF/factura_{id}.pdf";
            if (!System.IO.File.Exists(path))
                return NotFound(new { mensaje = "PDF no encontrado. Genérelo primero." });

            var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            return File(stream, "application/pdf", $"Factura_{factura.NumeroFacturaCompleto}.pdf");
        }

        // ==================== REPORTES ====================

        // GET: api/Facturas/Reportes/Ventas
        [HttpGet("Reportes/Ventas")]
        public async Task<ActionResult> ObtenerReporteVentas(
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin)
        {
            var usuarioId = ObtenerUsuarioId();
            if (usuarioId == null)
                return Unauthorized();

            var query = _context.Facturas
                .Include(f => f.Cliente)
                .Where(f => f.UsuarioId == usuarioId)
                .AsQueryable();

            if (fechaInicio.HasValue) query = query.Where(f => f.FechaEmision >= fechaInicio.Value);
            if (fechaFin.HasValue) query = query.Where(f => f.FechaEmision <= fechaFin.Value);

            var facturas = await query.OrderByDescending(f => f.FechaEmision).ToListAsync();

            return Ok(new
            {
                totalFacturas = facturas.Count,
                facturasPagadas = facturas.Count(f => f.Estado == "Pagada"),
                facturasPendientes = facturas.Count(f => f.Estado is "Emitida" or "Enviada"),
                facturasVencidas = facturas.Count(f => f.EstaVencida),
                totalVentas = facturas.Sum(f => f.TotalFactura),
                totalVentasPagadas = facturas.Where(f => f.Estado == "Pagada").Sum(f => f.TotalFactura),
                totalVentasPendientes = facturas.Where(f => f.Estado != "Pagada").Sum(f => f.TotalFactura),
                totalIVA = facturas.Sum(f => f.TotalIVA),
                totalINC = facturas.Sum(f => f.TotalINC),  // ✅ ya no nullable
                totalICA = facturas.Sum(f => f.TotalICA),  // ✅ nuevo campo
                facturas = facturas.Select(f => new
                {
                    f.Id,
                    f.NumeroFacturaCompleto,
                    f.FechaEmision,
                    f.FechaVencimiento,
                    cliente = new { f.Cliente!.Nombre, f.Cliente.Apellido },
                    f.Subtotal,
                    f.TotalIVA,
                    f.TotalINC,  // ✅ ya no necesita ?? 0
                    f.TotalICA,
                    f.TotalFactura,
                    f.Estado,
                    f.MedioPago,
                    f.EnviadaDIAN,
                    f.Cufe
                }).ToList()
            });
        }

        // GET: api/Facturas/Reportes/TopClientes
        [HttpGet("Reportes/TopClientes")]
        public async Task<ActionResult> ObtenerTopClientes([FromQuery] int top = 10)
        {
            var usuarioId = ObtenerUsuarioId();
            if (usuarioId == null)
                return Unauthorized();

            var topClientes = await _context.Facturas
                .Include(f => f.Cliente)
                .Where(f => f.UsuarioId == usuarioId)
                .GroupBy(f => new { f.ClienteId, f.Cliente!.Nombre, f.Cliente.Apellido })
                .Select(g => new
                {
                    clienteId = g.Key.ClienteId,
                    nombreCliente = $"{g.Key.Nombre} {g.Key.Apellido}",
                    totalFacturas = g.Count(),
                    totalCompras = g.Sum(f => f.TotalFactura)
                })
                .OrderByDescending(x => x.totalCompras)
                .Take(top)
                .ToListAsync();

            return Ok(topClientes);
        }

        // GET: api/Facturas/Reportes/ProductosMasVendidos
        [HttpGet("Reportes/ProductosMasVendidos")]
        public async Task<ActionResult> ObtenerProductosMasVendidos([FromQuery] int top = 10)
        {
            var usuarioId = ObtenerUsuarioId();
            if (usuarioId == null)
                return Unauthorized();

            var productos = await _context.DetalleFacturas
                .Include(d => d.Producto)
                .Include(d => d.Factura)
                .Where(d => d.Factura.UsuarioId == usuarioId)
                .GroupBy(d => new { d.ProductoId, d.Producto.Nombre })
                .Select(g => new
                {
                    productoId = g.Key.ProductoId,
                    nombreProducto = g.Key.Nombre,
                    cantidadVendida = g.Sum(d => d.Cantidad),
                    totalVentas = g.Sum(d => d.Cantidad * d.PrecioUnitario)
                })
                .OrderByDescending(x => x.cantidadVendida)
                .Take(top)
                .ToListAsync();

            return Ok(productos);
        }

        // ==================== ALERTAS DIAN ====================

        // GET: api/Facturas/proximas-vencer-dian
        [HttpGet("proximas-vencer-dian")]
        public async Task<ActionResult> ObtenerProximasVencerDIAN()
        {
            var usuarioId = ObtenerUsuarioId();
            if (usuarioId == null)
                return Unauthorized();

            var limite = DateTime.Now.AddHours(24);

            // ✅ Filtro por usuario incluido
            var facturas = await _context.Facturas
                .Where(f => !f.EnviadaDIAN && f.FechaLimiteEnvioDIAN <= DateTime.Now.AddHours(24))
                .Include(f => f.Cliente)
                .Where(f => f.UsuarioId == usuarioId
                         && !f.EnviadaDIAN
                         && f.FechaLimiteEnvioDIAN <= limite
                         && f.FechaLimiteEnvioDIAN >= DateTime.Now)
                .Select(f => new
                {
                    f.Id,
                    f.NumeroFacturaCompleto,
                    f.FechaEmision,
                    f.FechaLimiteEnvioDIAN,
                    f.HorasRestantesEnvioDIAN,
                    f.TotalFactura,
                    cliente = new { f.Cliente!.Nombre, f.Cliente.Apellido },
                    tieneXml = !string.IsNullOrEmpty(f.XmlBase64)
                })
                .ToListAsync();

            return Ok(facturas);
        }

        // GET: api/Facturas/vencidas
        [HttpGet("vencidas")]
        public async Task<ActionResult<IEnumerable<Factura>>> ObtenerFacturasVencidas()
        {
            var facturas = await _context.Facturas.Include(f => f.Cliente).ToListAsync();
            var vencidas = facturas.Where(f => f.EstaVencida).ToList();
            return Ok(vencidas);
        }

    }
}