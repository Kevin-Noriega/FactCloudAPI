using FactCloudAPI.Data;
using FactCloudAPI.DTOs.NotaDebito;
using FactCloudAPI.Models;
using FactCloudAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FactCloudAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotasDebitoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ISuscripcionService _suscripcionService; // ← NUEVO

        public NotasDebitoController(
            ApplicationDbContext context,
            ISuscripcionService suscripcionService) // ← NUEVO
        {
            _context = context;
            _suscripcionService = suscripcionService; // ← NUEVO
        }

        // GET: api/NotasDebito
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotaDebitoResponseDto>>> GetNotasDebito()
        {
            try
            {
                var notasDebito = await _context.NotasDebito
                    .Include(nd => nd.Cliente).Include(nd => nd.Factura)
                    .Include(nd => nd.Detalles).Include(nd => nd.FormasPago)
                    .OrderByDescending(nd => nd.FechaRegistro)
                    .ToListAsync();

                return Ok(notasDebito.Select(nd => MapToDto(nd)).ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener notas débito", error = ex.Message });
            }
        }

        // GET: api/NotasDebito/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NotaDebitoResponseDto>> GetNotaDebito(int id)
        {
            try
            {
                var notaDebito = await _context.NotasDebito
                    .Include(nd => nd.Cliente).Include(nd => nd.Factura)
                    .Include(nd => nd.Detalles).Include(nd => nd.FormasPago)
                    .FirstOrDefaultAsync(nd => nd.Id == id);

                if (notaDebito == null)
                    return NotFound(new { message = "Nota débito no encontrada" });

                return Ok(MapToDto(notaDebito));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener nota débito", error = ex.Message });
            }
        }

        // POST: api/NotasDebito
        [HttpPost]
        public async Task<ActionResult<NotaDebitoResponseDto>> CreateNotaDebito([FromBody] NotaDebitoDto dto)
        {
            try
            {
                var factura = await _context.Facturas
                    .Include(f => f.Cliente)
                    .FirstOrDefaultAsync(f => f.Id == dto.FacturaId);

                if (factura == null)
                    return BadRequest(new { message = "Factura no encontrada" });

                var notaDebito = new NotaDebito
                {
                    UsuarioId = dto.UsuarioId,
                    FacturaId = dto.FacturaId,
                    NumeroNota = dto.NumeroNota,
                    NumeroFactura = factura.NumeroFactura,
                    ClienteId = factura.ClienteId,
                    Tipo = dto.Tipo,
                    MotivoDIAN = dto.MotivoDIAN,
                    FechaElaboracion = dto.FechaElaboracion,
                    FechaRegistro = DateTime.Now,
                    CUFE = dto.CUFE,
                    TotalBruto = dto.TotalBruto,
                    TotalDescuentos = dto.TotalDescuentos,
                    Subtotal = dto.Subtotal,
                    TotalIVA = dto.TotalIVA,
                    TotalINC = dto.TotalINC,
                    ReteICA = dto.ReteICA,
                    TotalNeto = dto.TotalNeto,
                    Estado = dto.Estado,
                    Observaciones = dto.Observaciones
                };

                _context.NotasDebito.Add(notaDebito);
                await _context.SaveChangesAsync();

                foreach (var d in dto.DetalleNotaDebito)
                    _context.DetalleNotaDebito.Add(new DetalleNotaDebito
                    {
                        NotaDebitoId = notaDebito.Id,
                        ProductoId = d.ProductoId,
                        Descripcion = d.Descripcion,
                        Cantidad = d.Cantidad,
                        UnidadMedida = d.UnidadMedida,
                        PrecioUnitario = d.PrecioUnitario,
                        PorcentajeDescuento = d.PorcentajeDescuento,
                        ValorDescuento = d.ValorDescuento,
                        SubtotalLinea = d.SubtotalLinea,
                        TarifaIVA = d.TarifaIVA,
                        ValorIVA = d.ValorIVA,
                        TarifaINC = d.TarifaINC,
                        ValorINC = d.ValorINC,
                        TotalLinea = d.TotalLinea
                    });

                foreach (var fp in dto.FormasPago)
                    _context.FormasPagoNotaDebito.Add(new FormaPagoNotaDebito
                    { NotaDebitoId = notaDebito.Id, Metodo = fp.Metodo, Valor = fp.Valor });

                await _context.SaveChangesAsync();

                // ── Incrementar contador de documentos usados ──────────────
                await _suscripcionService.IncrementarDocumentosUsados(dto.UsuarioId);

                var notaCreada = await _context.NotasDebito
                    .Include(nd => nd.Cliente).Include(nd => nd.Factura)
                    .Include(nd => nd.Detalles).Include(nd => nd.FormasPago)
                    .FirstOrDefaultAsync(nd => nd.Id == notaDebito.Id);

                return CreatedAtAction(nameof(GetNotaDebito), new { id = notaCreada!.Id }, MapToDto(notaCreada));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear nota débito", error = ex.Message });
            }
        }

        // PUT: api/NotasDebito/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotaDebito(int id, [FromBody] NotaDebitoDto dto)
        {
            try
            {
                var notaDebito = await _context.NotasDebito
                    .Include(nd => nd.Detalles).Include(nd => nd.FormasPago)
                    .FirstOrDefaultAsync(nd => nd.Id == id);

                if (notaDebito == null)
                    return NotFound(new { message = "Nota débito no encontrada" });

                notaDebito.Tipo = dto.Tipo;
                notaDebito.MotivoDIAN = dto.MotivoDIAN;
                notaDebito.FechaElaboracion = dto.FechaElaboracion;
                notaDebito.CUFE = dto.CUFE;
                notaDebito.TotalBruto = dto.TotalBruto;
                notaDebito.TotalDescuentos = dto.TotalDescuentos;
                notaDebito.Subtotal = dto.Subtotal;
                notaDebito.TotalIVA = dto.TotalIVA;
                notaDebito.TotalINC = dto.TotalINC;
                notaDebito.ReteICA = dto.ReteICA;
                notaDebito.TotalNeto = dto.TotalNeto;
                notaDebito.Estado = dto.Estado;
                notaDebito.Observaciones = dto.Observaciones;

                _context.DetalleNotaDebito.RemoveRange(notaDebito.Detalles);
                _context.FormasPagoNotaDebito.RemoveRange(notaDebito.FormasPago);

                foreach (var d in dto.DetalleNotaDebito)
                    _context.DetalleNotaDebito.Add(new DetalleNotaDebito
                    {
                        NotaDebitoId = notaDebito.Id,
                        ProductoId = d.ProductoId,
                        Descripcion = d.Descripcion,
                        Cantidad = d.Cantidad,
                        UnidadMedida = d.UnidadMedida,
                        PrecioUnitario = d.PrecioUnitario,
                        PorcentajeDescuento = d.PorcentajeDescuento,
                        ValorDescuento = d.ValorDescuento,
                        SubtotalLinea = d.SubtotalLinea,
                        TarifaIVA = d.TarifaIVA,
                        ValorIVA = d.ValorIVA,
                        TarifaINC = d.TarifaINC,
                        ValorINC = d.ValorINC,
                        TotalLinea = d.TotalLinea
                    });

                foreach (var fp in dto.FormasPago)
                    _context.FormasPagoNotaDebito.Add(new FormaPagoNotaDebito
                    { NotaDebitoId = notaDebito.Id, Metodo = fp.Metodo, Valor = fp.Valor });

                await _context.SaveChangesAsync();
                return Ok(new { message = "Nota débito actualizada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar nota débito", error = ex.Message });
            }
        }

        // DELETE: api/NotasDebito/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotaDebito(int id)
        {
            try
            {
                var notaDebito = await _context.NotasDebito
                    .Include(nd => nd.Detalles).Include(nd => nd.FormasPago)
                    .FirstOrDefaultAsync(nd => nd.Id == id);

                if (notaDebito == null)
                    return NotFound(new { message = "Nota débito no encontrada" });

                _context.DetalleNotaDebito.RemoveRange(notaDebito.Detalles);
                _context.FormasPagoNotaDebito.RemoveRange(notaDebito.FormasPago);
                _context.NotasDebito.Remove(notaDebito);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Nota débito eliminada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar nota débito", error = ex.Message });
            }
        }

        // ── Helper de mapeo ───────────────────────────────────────────────
        private static NotaDebitoResponseDto MapToDto(NotaDebito nd) => new()
        {
            Id = nd.Id,
            NumeroNota = nd.NumeroNota,
            FacturaId = nd.FacturaId,
            NumeroFactura = nd.Factura?.NumeroFactura ?? nd.NumeroFactura,
            Cliente = nd.Cliente != null ? new ClienteSimpleDto
            {
                Id = nd.Cliente.Id,
                Nombre = nd.Cliente.Nombre,
                Apellido = nd.Cliente.Apellido,
                Documento = nd.Cliente.NumeroIdentificacion
            } : null,
            Tipo = nd.Tipo,
            MotivoDIAN = nd.MotivoDIAN,
            FechaElaboracion = nd.FechaElaboracion,
            FechaRegistro = nd.FechaRegistro,
            CUFE = nd.CUFE,
            XMLBase64 = nd.XMLBase64,
            TotalBruto = nd.TotalBruto,
            TotalDescuentos = nd.TotalDescuentos,
            Subtotal = nd.Subtotal,
            TotalIVA = nd.TotalIVA,
            TotalINC = nd.TotalINC,
            ReteICA = nd.ReteICA,
            TotalNeto = nd.TotalNeto,
            Estado = nd.Estado,
            Observaciones = nd.Observaciones,
            DetalleNotaDebito = nd.Detalles.Select(d => new DetalleNotaDebitoResponseDto
            {
                Id = d.Id,
                ProductoId = d.ProductoId,
                Descripcion = d.Descripcion,
                Cantidad = d.Cantidad,
                UnidadMedida = d.UnidadMedida,
                PrecioUnitario = d.PrecioUnitario,
                PorcentajeDescuento = d.PorcentajeDescuento,
                ValorDescuento = d.ValorDescuento,
                SubtotalLinea = d.SubtotalLinea,
                TarifaIVA = d.TarifaIVA,
                ValorIVA = d.ValorIVA,
                TarifaINC = d.TarifaINC,
                ValorINC = d.ValorINC,
                TotalLinea = d.TotalLinea
            }).ToList(),
            FormasPago = nd.FormasPago.Select(fp => new FormaPagoResponseDto
            { Id = fp.Id, Metodo = fp.Metodo, Valor = fp.Valor }).ToList()
        };
    }
}