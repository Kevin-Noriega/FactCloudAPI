using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NubeeAPI.Data;
using NubeeAPI.Models.Pos;
using System.Security.Claims;

namespace NubeeAPI.Controllers
{
    /// <summary>
    /// Endpoints del módulo POS (punto de venta). Por ahora expone los
    /// productos del usuario autenticado mapeados a la forma que consume el front.
    /// </summary>
    [ApiController]
    [Route("api/pos")]
    [Authorize]
    public class PosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PosController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int UsuarioId =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        // GET: api/pos/products
        [HttpGet("products")]
        public async Task<ActionResult> GetProducts(
            [FromQuery] string? search,
            [FromQuery] string? category,
            [FromQuery] bool? onlyFavorites,
            [FromQuery] int pageSize = 50)
        {
            var query = _context.Productos
                .Where(p => p.UsuarioId == UsuarioId && p.Activo);

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(p => p.Categoria == category);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(p =>
                    p.Nombre.Contains(s) ||
                    (p.CodigoInterno != null && p.CodigoInterno.Contains(s)) ||
                    (p.CodigoBarras != null && p.CodigoBarras.Contains(s)));
            }

            // onlyFavorites: el modelo aún no tiene favoritos persistidos → se ignora.

            var productos = await query
                .OrderBy(p => p.Nombre)
                .Take(pageSize > 0 ? pageSize : 50)
                .ToListAsync();

            var data = productos.Select(MapToPos).ToList();

            return Ok(new { data, total = data.Count });
        }

        // GET: api/pos/products/categories
        [HttpGet("products/categories")]
        public async Task<ActionResult> GetCategories()
        {
            var categorias = await _context.Productos
                .Where(p => p.UsuarioId == UsuarioId && p.Activo && p.Categoria != null && p.Categoria != "")
                .Select(p => p.Categoria!)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            var data = categorias
                .Select(c => new { id = c, name = c })
                .ToList();

            return Ok(data);
        }

        // GET: api/pos/products/barcode/{barcode}
        [HttpGet("products/barcode/{barcode}")]
        public async Task<ActionResult> GetByBarcode(string barcode)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p =>
                    p.UsuarioId == UsuarioId && p.Activo && p.CodigoBarras == barcode);

            if (producto == null)
                return NotFound(new { message = "Producto no encontrado" });

            return Ok(MapToPos(producto));
        }

        // POST: api/pos/products/{id}/favorite
        // El modelo aún no persiste favoritos; el front maneja el estado localmente.
        [HttpPost("products/{id:int}/favorite")]
        public async Task<ActionResult> ToggleFavorite(int id)
        {
            var existe = await _context.Productos
                .AnyAsync(p => p.Id == id && p.UsuarioId == UsuarioId);

            if (!existe)
                return NotFound(new { message = "Producto no encontrado" });

            return Ok(new { ok = true });
        }

        // POST: api/pos/products/{id}/stock → añade (o ajusta) unidades al inventario
        [HttpPost("products/{id:int}/stock")]
        public async Task<ActionResult> AddStock(int id, [FromBody] AjusteStockDto dto)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == id && p.UsuarioId == UsuarioId);

            if (producto == null)
                return NotFound(new { message = "Producto no encontrado" });
            if (producto.EsServicio)
                return BadRequest(new { message = "Un servicio no maneja inventario" });
            if (dto == null || dto.Cantidad == 0)
                return BadRequest(new { message = "Cantidad inválida" });

            producto.CantidadDisponible = (producto.CantidadDisponible ?? 0) + dto.Cantidad;
            await _context.SaveChangesAsync();

            return Ok(MapToPos(producto));
        }

        // ════════════════════════════════════════════════════════════════
        // TURNOS (apertura / cierre de caja)
        // ════════════════════════════════════════════════════════════════

        // GET: api/pos/shifts/current → turno abierto actual (o null)
        [HttpGet("shifts/current")]
        public async Task<ActionResult> GetCurrentShift()
        {
            var turno = await _context.TurnosPos
                .Where(t => t.UsuarioId == UsuarioId && t.Estado == "Abierto")
                .OrderByDescending(t => t.FechaApertura)
                .FirstOrDefaultAsync();

            return Ok(turno == null ? null : MapToShift(turno));
        }

        // POST: api/pos/shifts/open
        [HttpPost("shifts/open")]
        public async Task<ActionResult> OpenShift([FromBody] AbrirTurnoDto? dto)
        {
            var yaAbierto = await _context.TurnosPos
                .AnyAsync(t => t.UsuarioId == UsuarioId && t.Estado == "Abierto");
            if (yaAbierto)
                return Conflict(new { message = "Ya tienes un turno abierto. Ciérralo antes de abrir otro." });

            var usuario = await _context.Usuarios.FindAsync(UsuarioId);
            if (usuario == null) return Unauthorized();

            var ultimoNumero = await _context.TurnosPos
                .Where(t => t.UsuarioId == UsuarioId)
                .Select(t => (int?)t.NumeroTurno)
                .MaxAsync() ?? 0;

            var turno = new TurnoPos
            {
                UsuarioId = UsuarioId,
                NumeroTurno = ultimoNumero + 1,
                VendedorNombre = $"{usuario.Nombre} {usuario.Apellido}".Trim(),
                FechaApertura = DateTime.Now,
                BaseInicial = dto?.BaseInicial ?? 0,
                Estado = "Abierto",
            };

            _context.TurnosPos.Add(turno);
            await _context.SaveChangesAsync();

            return Ok(MapToShift(turno));
        }

        // POST: api/pos/shifts/close
        [HttpPost("shifts/close")]
        public async Task<ActionResult> CloseShift([FromBody] CerrarTurnoDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Datos de cierre requeridos" });

            var turno = await _context.TurnosPos
                .FirstOrDefaultAsync(t => t.Id == dto.TurnoId && t.UsuarioId == UsuarioId);
            if (turno == null)
                return NotFound(new { message = "Turno no encontrado" });
            if (turno.Estado == "Cerrado")
                return BadRequest(new { message = "El turno ya está cerrado" });

            var usuario = await _context.Usuarios.FindAsync(UsuarioId);

            turno.TotalEfectivoReal = dto.TotalEfectivoReal ?? 0;
            turno.TotalTarjeta = dto.TotalTarjeta ?? 0;
            turno.TotalPagosLinea = dto.TotalPagosLinea ?? 0;
            turno.TotalOtros = dto.TotalOtros ?? 0;
            turno.Observaciones = dto.Observaciones;

            // Efectivo esperado = base + ventas en efectivo + ingresos - retiros de caja.
            var calc = await CalcularTurno(turno);
            var efectivoEsperado =
                turno.BaseInicial + calc.vEfectivo + calc.ingresos - calc.retiros;

            turno.TotalEsperado = efectivoEsperado;
            turno.Diferencia = (turno.TotalEfectivoReal ?? 0) - efectivoEsperado;
            turno.FechaCierre = DateTime.Now;
            turno.Estado = "Cerrado";
            turno.CerradoPorNombre = $"{usuario?.Nombre} {usuario?.Apellido}".Trim();

            await _context.SaveChangesAsync();

            return Ok(MapToShift(turno));
        }

        // GET: api/pos/shifts?from&to&page&pageSize → historial paginado
        [HttpGet("shifts")]
        public async Task<ActionResult> GetShifts(
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.TurnosPos.Where(t => t.UsuarioId == UsuarioId);

            if (from.HasValue)
                query = query.Where(t => t.FechaApertura >= from.Value.Date);
            if (to.HasValue)
                query = query.Where(t => t.FechaApertura < to.Value.Date.AddDays(1));

            var total = await query.CountAsync();
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var turnos = await query
                .OrderByDescending(t => t.FechaApertura)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                data = turnos.Select(MapToShift).ToList(),
                total,
                page,
                pageSize,
            });
        }

        // GET: api/pos/shifts/{id}/summary → resumen para el cierre/detalle
        [HttpGet("shifts/{id:int}/summary")]
        public async Task<ActionResult> GetShiftSummary(int id)
        {
            var turno = await _context.TurnosPos
                .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == UsuarioId);
            if (turno == null)
                return NotFound(new { message = "Turno no encontrado" });

            var calc = await CalcularTurno(turno);
            var efectivoEsperado =
                turno.BaseInicial + calc.vEfectivo + calc.ingresos - calc.retiros;

            var totalIngresado =
                (turno.TotalEfectivoReal ?? 0) +
                (turno.TotalTarjeta ?? 0) +
                (turno.TotalPagosLinea ?? 0) +
                (turno.TotalOtros ?? 0);
            var totalEsperado = turno.Estado == "Cerrado"
                ? (turno.TotalEsperado ?? efectivoEsperado)
                : efectivoEsperado;

            return Ok(new
            {
                turnoId = turno.Id,
                baseInicial = turno.BaseInicial,
                totalIngresado,
                totalEsperado,
                diferencia = turno.Diferencia ?? ((turno.TotalEfectivoReal ?? 0) - totalEsperado),
                ventasEfectivo = calc.vEfectivo,
                ventasTarjeta = calc.vTarjeta,
                ventasPagosLinea = calc.vLinea,
                ventasOtros = calc.vOtros,
                ventasCredito = calc.vCredito,
                ingresosEfectivo = calc.ingresos,
                retirosEfectivo = calc.retiros,
                devoluciones = 0m,
                retirosNoEfectivo = 0m,
            });
        }

        // ════════════════════════════════════════════════════════════════
        // VENTAS POS
        // ════════════════════════════════════════════════════════════════

        [HttpPost("sales")]
        public async Task<ActionResult> CreateSale([FromBody] CrearVentaDto dto)
        {
            if (dto?.Items == null || dto.Items.Count == 0)
                return BadRequest(new { message = "La venta no tiene items" });

            var turnoAbierto = await _context.TurnosPos
                .Where(t => t.UsuarioId == UsuarioId && t.Estado == "Abierto")
                .OrderByDescending(t => t.FechaApertura)
                .FirstOrDefaultAsync();

            var productoIds = dto.Items
                .Where(i => i.ProductoId.HasValue)
                .Select(i => i.ProductoId!.Value)
                .Distinct()
                .ToList();
            var productos = await _context.Productos
                .Where(p => p.UsuarioId == UsuarioId && productoIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            decimal subtotal = 0, impuestos = 0;
            var detalles = new List<PosVentaDetalle>();

            foreach (var item in dto.Items)
            {
                var cantidad = item.Cantidad <= 0 ? 1 : item.Cantidad;
                var lineaConDesc = item.PrecioUnitario * cantidad * (1 - item.Descuento / 100m);
                subtotal += lineaConDesc;

                Models.Producto? prod = null;
                if (item.ProductoId.HasValue)
                    productos.TryGetValue(item.ProductoId.Value, out prod);
                if (prod != null && prod.IncluyeIVA)
                    impuestos += lineaConDesc * 0.19m;

                detalles.Add(new PosVentaDetalle
                {
                    ProductoId = item.ProductoId,
                    Nombre = item.Nombre ?? prod?.Nombre ?? "",
                    Cantidad = cantidad,
                    PrecioUnitario = item.PrecioUnitario,
                    Descuento = item.Descuento,
                    TotalLinea = Math.Round(lineaConDesc, 2),
                });

                // Descuenta inventario (solo productos físicos del catálogo)
                if (prod != null && !prod.EsServicio)
                    prod.CantidadDisponible =
                        (prod.CantidadDisponible ?? 0) - (int)Math.Ceiling(cantidad);
            }

            subtotal = Math.Round(subtotal, 2);
            impuestos = Math.Round(impuestos, 2);
            var total = subtotal + impuestos;

            decimal MontoDe(string m) =>
                dto.Pagos?.Where(p => (p.Metodo ?? "") == m).Sum(p => p.Monto) ?? 0;

            var consecutivo =
                await _context.PosVentas.CountAsync(v => v.UsuarioId == UsuarioId) + 1;

            var venta = new PosVenta
            {
                UsuarioId = UsuarioId,
                TurnoId = turnoAbierto?.Id,
                NumeroVenta = consecutivo,
                Fecha = DateTime.Now,
                ClienteNombre = string.IsNullOrWhiteSpace(dto.ClienteNombre)
                    ? "Consumidor Final"
                    : dto.ClienteNombre.Trim(),
                Subtotal = subtotal,
                Impuestos = impuestos,
                Total = total,
                Efectivo = MontoDe("Efectivo"),
                Tarjeta = MontoDe("Tarjeta"),
                PagosLinea = MontoDe("PagosLinea"),
                Otros = MontoDe("Otros"),
                Credito = MontoDe("Credito"),
                Estado = "Registrada",
                Detalles = detalles,
            };

            _context.PosVentas.Add(venta);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                id = venta.Id,
                numeroVenta = venta.NumeroVenta,
                fecha = venta.Fecha,
                subtotal = venta.Subtotal,
                impuestos = venta.Impuestos,
                total = venta.Total,
                turnoId = venta.TurnoId,
                clienteNombre = venta.ClienteNombre,
            });
        }

        // Agrega ventas (por medio de pago) y movimientos de caja del turno.
        private async Task<(decimal vEfectivo, decimal vTarjeta, decimal vLinea,
            decimal vOtros, decimal vCredito, decimal ingresos, decimal retiros)>
            CalcularTurno(TurnoPos t)
        {
            var ventas = _context.PosVentas.Where(v => v.TurnoId == t.Id);
            var vEfectivo = await ventas.SumAsync(v => (decimal?)v.Efectivo) ?? 0;
            var vTarjeta = await ventas.SumAsync(v => (decimal?)v.Tarjeta) ?? 0;
            var vLinea = await ventas.SumAsync(v => (decimal?)v.PagosLinea) ?? 0;
            var vOtros = await ventas.SumAsync(v => (decimal?)v.Otros) ?? 0;
            var vCredito = await ventas.SumAsync(v => (decimal?)v.Credito) ?? 0;

            var fin = t.FechaCierre ?? DateTime.Now;
            var movs = _context.MovimientosCajaPos.Where(m =>
                m.UsuarioId == t.UsuarioId && m.Fecha >= t.FechaApertura && m.Fecha <= fin);
            var ingresos = await movs.Where(m => m.Tipo == "Ingreso").SumAsync(m => (decimal?)m.Monto) ?? 0;
            var retiros = await movs.Where(m => m.Tipo == "Retiro").SumAsync(m => (decimal?)m.Monto) ?? 0;

            return (vEfectivo, vTarjeta, vLinea, vOtros, vCredito, ingresos, retiros);
        }

        // GET: api/pos/reports/daily?date | ?from&to → reporte de ventas
        [HttpGet("reports/daily")]
        public async Task<ActionResult> GetDailyReport(
            [FromQuery] DateTime? date,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            DateTime desde, hasta;
            if (from.HasValue || to.HasValue)
            {
                desde = (from ?? DateTime.Today).Date;
                hasta = (to ?? DateTime.Today).Date.AddDays(1);
            }
            else
            {
                var d = (date ?? DateTime.Today).Date;
                desde = d;
                hasta = d.AddDays(1);
            }

            var ventas = _context.PosVentas
                .Where(v => v.UsuarioId == UsuarioId && v.Fecha >= desde && v.Fecha < hasta);

            var total = await ventas.SumAsync(v => (decimal?)v.Total) ?? 0;
            var transacciones = await ventas.CountAsync();
            var ticket = transacciones > 0 ? total / transacciones : 0;
            var clientes = await ventas.Select(v => v.ClienteNombre).Distinct().CountAsync();

            var porMetodo = new
            {
                efectivo = await ventas.SumAsync(v => (decimal?)v.Efectivo) ?? 0,
                tarjeta = await ventas.SumAsync(v => (decimal?)v.Tarjeta) ?? 0,
                pagosLinea = await ventas.SumAsync(v => (decimal?)v.PagosLinea) ?? 0,
                otros = await ventas.SumAsync(v => (decimal?)v.Otros) ?? 0,
                credito = await ventas.SumAsync(v => (decimal?)v.Credito) ?? 0,
            };

            var topProductos = await _context.PosVentaDetalles
                .Where(d => d.PosVenta.UsuarioId == UsuarioId &&
                            d.PosVenta.Fecha >= desde && d.PosVenta.Fecha < hasta)
                .GroupBy(d => d.Nombre)
                .Select(g => new
                {
                    nombre = g.Key,
                    cantidad = g.Sum(x => x.Cantidad),
                    total = g.Sum(x => x.TotalLinea),
                })
                .OrderByDescending(x => x.total)
                .Take(10)
                .ToListAsync();

            var listaVentas = await ventas
                .OrderByDescending(v => v.Fecha)
                .Take(50)
                .Select(v => new
                {
                    numeroVenta = v.NumeroVenta,
                    fecha = v.Fecha,
                    clienteNombre = v.ClienteNombre,
                    total = v.Total,
                })
                .ToListAsync();

            return Ok(new
            {
                totalVentas = total,
                transacciones,
                ticketPromedio = ticket,
                clientes,
                porMetodo,
                topProductos,
                ventas = listaVentas,
            });
        }

        // ════════════════════════════════════════════════════════════════
        // MOVIMIENTOS DE CAJA (ingresos / retiros de efectivo)
        // ════════════════════════════════════════════════════════════════

        [HttpGet("cash-movements")]
        public async Task<ActionResult> GetCashMovements(
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.MovimientosCajaPos.Where(m => m.UsuarioId == UsuarioId);

            if (from.HasValue)
                query = query.Where(m => m.Fecha >= from.Value.Date);
            if (to.HasValue)
                query = query.Where(m => m.Fecha < to.Value.Date.AddDays(1));
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(m =>
                    m.NumeroComprobante.Contains(s) ||
                    (m.Descripcion != null && m.Descripcion.Contains(s)));
            }

            var totalIngresos = await query.Where(m => m.Tipo == "Ingreso")
                .SumAsync(m => (decimal?)m.Monto) ?? 0;
            var totalRetiros = await query.Where(m => m.Tipo == "Retiro")
                .SumAsync(m => (decimal?)m.Monto) ?? 0;
            var total = await query.CountAsync();

            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var items = await query
                .OrderByDescending(m => m.Fecha)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                data = items.Select(MapToMovimiento).ToList(),
                total,
                page,
                pageSize,
                totalIngresos,
                totalRetiros,
                neto = totalIngresos - totalRetiros,
            });
        }

        [HttpPost("cash-movements")]
        public async Task<ActionResult> CreateCashMovement([FromBody] MovimientoCajaDto dto)
        {
            if (dto == null || dto.Monto <= 0)
                return BadRequest(new { message = "El monto debe ser mayor a 0" });

            var tipo = dto.Tipo == "Retiro" ? "Retiro" : "Ingreso";
            var consecutivo =
                await _context.MovimientosCajaPos.CountAsync(m => m.UsuarioId == UsuarioId) + 1;

            var mov = new MovimientoCajaPos
            {
                UsuarioId = UsuarioId,
                Tipo = tipo,
                Monto = dto.Monto,
                Descripcion = dto.Descripcion?.Trim(),
                NumeroComprobante = $"MC-{consecutivo:D5}",
                Fecha = DateTime.Now,
            };
            _context.MovimientosCajaPos.Add(mov);
            await _context.SaveChangesAsync();

            return Ok(MapToMovimiento(mov));
        }

        // ════════════════════════════════════════════════════════════════
        // ETIQUETAS
        // ════════════════════════════════════════════════════════════════

        [HttpGet("labels")]
        public async Task<ActionResult> GetLabels()
        {
            var labels = await _context.EtiquetasPos
                .Where(e => e.UsuarioId == UsuarioId)
                .OrderBy(e => e.Nombre)
                .ToListAsync();
            return Ok(labels.Select(MapToLabel).ToList());
        }

        [HttpPost("labels")]
        public async Task<ActionResult> CreateLabel([FromBody] EtiquetaDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Nombre))
                return BadRequest(new { message = "El nombre es obligatorio" });

            var etiqueta = new EtiquetaPos
            {
                UsuarioId = UsuarioId,
                Nombre = dto.Nombre.Trim(),
                Activa = dto.Activa ?? true,
                FechaCreacion = DateTime.Now,
            };
            _context.EtiquetasPos.Add(etiqueta);
            await _context.SaveChangesAsync();

            return Ok(MapToLabel(etiqueta));
        }

        [HttpPut("labels/{id:int}")]
        public async Task<ActionResult> UpdateLabel(int id, [FromBody] EtiquetaDto dto)
        {
            var etiqueta = await _context.EtiquetasPos
                .FirstOrDefaultAsync(e => e.Id == id && e.UsuarioId == UsuarioId);
            if (etiqueta == null)
                return NotFound(new { message = "Etiqueta no encontrada" });

            if (!string.IsNullOrWhiteSpace(dto?.Nombre))
                etiqueta.Nombre = dto.Nombre.Trim();
            if (dto?.Activa.HasValue == true)
                etiqueta.Activa = dto.Activa.Value;

            await _context.SaveChangesAsync();
            return Ok(MapToLabel(etiqueta));
        }

        [HttpDelete("labels/{id:int}")]
        public async Task<ActionResult> DeleteLabel(int id)
        {
            var etiqueta = await _context.EtiquetasPos
                .FirstOrDefaultAsync(e => e.Id == id && e.UsuarioId == UsuarioId);
            if (etiqueta == null)
                return NotFound(new { message = "Etiqueta no encontrada" });

            _context.EtiquetasPos.Remove(etiqueta);
            await _context.SaveChangesAsync();
            return Ok(new { ok = true });
        }

        // ════════════════════════════════════════════════════════════════
        // CONFIGURACIÓN DE IMPRESIÓN
        // ════════════════════════════════════════════════════════════════

        [HttpGet("print-config")]
        public async Task<ActionResult> GetPrintConfig()
        {
            var cfg = await _context.ConfiguracionesImpresionPos
                .FirstOrDefaultAsync(c => c.UsuarioId == UsuarioId);
            if (cfg == null)
            {
                cfg = new ConfiguracionImpresionPos { UsuarioId = UsuarioId };
                _context.ConfiguracionesImpresionPos.Add(cfg);
                await _context.SaveChangesAsync();
            }
            return Ok(MapToPrintConfig(cfg));
        }

        [HttpPut("print-config")]
        public async Task<ActionResult> UpdatePrintConfig([FromBody] ConfiguracionImpresionDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Datos requeridos" });

            var cfg = await _context.ConfiguracionesImpresionPos
                .FirstOrDefaultAsync(c => c.UsuarioId == UsuarioId);
            if (cfg == null)
            {
                cfg = new ConfiguracionImpresionPos { UsuarioId = UsuarioId };
                _context.ConfiguracionesImpresionPos.Add(cfg);
            }

            cfg.MetodoImpresion = dto.MetodoImpresion == "directa" ? "directa" : "navegador";
            cfg.ImpresoraDefecto = dto.ImpresoraDefecto;
            cfg.TamanoPapel = dto.TamanoPapel;
            cfg.Copias = dto.Copias ?? 1;
            cfg.ImpresionSimple = dto.ImpresionSimple ?? false;
            cfg.MargenSuperior = dto.MargenSuperior ?? 2;
            cfg.MargenInferior = dto.MargenInferior ?? 2;
            cfg.MargenIzquierdo = dto.MargenIzquierdo ?? 2;
            cfg.MargenDerecho = dto.MargenDerecho ?? 2;

            await _context.SaveChangesAsync();
            return Ok(MapToPrintConfig(cfg));
        }

        // ── Mapeos ───────────────────────────────────────────────────────
        private static object MapToMovimiento(MovimientoCajaPos m) => new
        {
            id = m.Id,
            tipo = m.Tipo,
            monto = m.Monto,
            descripcion = m.Descripcion,
            numeroComprobante = m.NumeroComprobante,
            fecha = m.Fecha,
        };

        private static object MapToLabel(EtiquetaPos e) => new
        {
            id = e.Id,
            nombre = e.Nombre,
            activa = e.Activa,
            fechaCreacion = e.FechaCreacion,
        };

        private static object MapToPrintConfig(ConfiguracionImpresionPos c) => new
        {
            metodoImpresion = c.MetodoImpresion,
            impresoraDefecto = c.ImpresoraDefecto,
            tamanoPapel = c.TamanoPapel,
            copias = c.Copias,
            impresionSimple = c.ImpresionSimple,
            margenSuperior = c.MargenSuperior,
            margenInferior = c.MargenInferior,
            margenIzquierdo = c.MargenIzquierdo,
            margenDerecho = c.MargenDerecho,
        };

        // ── Mapeo TurnoPos → forma que consume el front ──────────────────
        private static object MapToShift(TurnoPos t) => new
        {
            id = t.Id.ToString(),
            numeroTurno = t.NumeroTurno,
            vendedorNombre = t.VendedorNombre,
            estado = t.Estado,
            fechaApertura = t.FechaApertura,
            fechaCierre = t.FechaCierre,
            baseInicial = t.BaseInicial,
            totalEfectivoReal = t.TotalEfectivoReal,
            totalTarjeta = t.TotalTarjeta,
            totalPagosLinea = t.TotalPagosLinea,
            totalOtros = t.TotalOtros,
            totalEsperado = t.TotalEsperado,
            diferencia = t.Diferencia,
            observaciones = t.Observaciones,
            cerradoPorNombre = t.CerradoPorNombre,
        };

        // ── Mapeo Producto → forma POS que consume el front ──────────────
        private static object MapToPos(Models.Producto p) => new
        {
            id = p.Id.ToString(),
            name = p.Nombre,
            sku = string.IsNullOrWhiteSpace(p.CodigoInterno) ? p.Id.ToString() : p.CodigoInterno,
            barcode = p.CodigoBarras,
            price = p.PrecioUnitario,
            taxRate = p.IncluyeIVA ? 0.19m : 0m,
            stock = p.CantidadDisponible ?? 0,
            category = p.Categoria,
            isService = p.EsServicio,
            isFavorite = false,
            imageUrl = (string?)null,
            initials = Iniciales(p.Nombre),
        };

        private static string Iniciales(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return "?";
            var partes = nombre.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (partes.Length == 1)
                return partes[0].Substring(0, Math.Min(2, partes[0].Length)).ToUpperInvariant();
            return (partes[0][0].ToString() + partes[1][0]).ToUpperInvariant();
        }
    }

    // ── DTOs ──────────────────────────────────────────────────────────────
    public class AjusteStockDto
    {
        public int Cantidad { get; set; }
    }

    public class MovimientoCajaDto
    {
        public string? Tipo { get; set; }
        public decimal Monto { get; set; }
        public string? Descripcion { get; set; }
    }

    public class CrearVentaDto
    {
        public string? ClienteNombre { get; set; }
        public List<VentaItemDto> Items { get; set; } = new();
        public List<VentaPagoDto> Pagos { get; set; } = new();
    }

    public class VentaItemDto
    {
        public int? ProductoId { get; set; }
        public string? Nombre { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Descuento { get; set; }
    }

    public class VentaPagoDto
    {
        public string? Metodo { get; set; }
        public decimal Monto { get; set; }
    }

    public class EtiquetaDto
    {
        public string? Nombre { get; set; }
        public bool? Activa { get; set; }
    }

    public class ConfiguracionImpresionDto
    {
        public string? MetodoImpresion { get; set; }
        public string? ImpresoraDefecto { get; set; }
        public string? TamanoPapel { get; set; }
        public int? Copias { get; set; }
        public bool? ImpresionSimple { get; set; }
        public int? MargenSuperior { get; set; }
        public int? MargenInferior { get; set; }
        public int? MargenIzquierdo { get; set; }
        public int? MargenDerecho { get; set; }
    }

    public class AbrirTurnoDto
    {
        public decimal? BaseInicial { get; set; }
    }

    public class CerrarTurnoDto
    {
        public int TurnoId { get; set; }
        public decimal? TotalEfectivoReal { get; set; }
        public decimal? TotalTarjeta { get; set; }
        public decimal? TotalPagosLinea { get; set; }
        public decimal? TotalOtros { get; set; }
        public string? Observaciones { get; set; }
    }
}
