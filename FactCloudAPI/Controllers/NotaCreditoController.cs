using FactCloudAPI.Data;
using FactCloudAPI.DTOs.NotaCredito;
using FactCloudAPI.DTOs.NotaDebito;
using FactCloudAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FactCloudAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotasCreditoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NotasCreditoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/NotasCredito
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotaCreditoResponseDto>>> GetNotasCredito()
        {
            try
            {
                var notasCredito = await _context.NotasCredito
                    .Include(nc => nc.Cliente)
                    .Include(nc => nc.Factura)
                    .Include(nc => nc.DetalleNotaCredito)
                    .Include(nc => nc.FormasPago)
                    .OrderByDescending(nc => nc.FechaRegistro)
                    .ToListAsync();

                var response = notasCredito.Select(nc => new NotaCreditoResponseDto
                {
                    Id = nc.Id,
                    NumeroNota = nc.NumeroNota,
                    FacturaId = nc.FacturaId,
                    NumeroFactura = nc.Factura?.NumeroFactura ?? nc.NumeroFactura,
                    Cliente = nc.Cliente != null ? new ClienteSimpleDto
                    {
                        Id = nc.Cliente.Id,
                        Nombre = nc.Cliente.Nombre,
                        Apellido = nc.Cliente.Apellido,
                        Documento = nc.Cliente.NumeroIdentificacion
                    } : null,
                    Tipo = nc.Tipo,
                    MotivoDIAN = nc.MotivoDIAN,
                    FechaElaboracion = nc.FechaElaboracion,
                    FechaRegistro = nc.FechaRegistro,
                    CUFE = nc.CUFE,
                    XMLBase64 = nc.XMLBase64,
                    TotalBruto = nc.TotalBruto,
                    TotalDescuentos = nc.TotalDescuentos,
                    Subtotal = nc.Subtotal,
                    TotalIVA = nc.TotalIVA,
                    TotalINC = nc.TotalINC,
                    ReteICA = nc.ReteICA,
                    TotalNeto = nc.TotalNeto,
                    Estado = nc.Estado,
                    Observaciones = nc.Observaciones,
                    DetalleNotaCredito = nc.DetalleNotaCredito.Select(d => new DetalleNotaCreditoResponseDto
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
                    FormasPago = nc.FormasPago.Select(fp => new FormaPagoNotaCreditoResponseDto
                    {
                        Id = fp.Id,
                        Metodo = fp.Metodo,
                        Valor = fp.Valor
                    }).ToList()
                }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener notas crédito", error = ex.Message });
            }
        }

        // GET: api/NotasCredito/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NotaCreditoResponseDto>> GetNotaCredito(int id)
        {
            try
            {
                var notaCredito = await _context.NotasCredito
                    .Include(nc => nc.Cliente)
                    .Include(nc => nc.Factura)
                    .Include(nc => nc.DetalleNotaCredito)
                    .Include(nc => nc.FormasPago)
                    .FirstOrDefaultAsync(nc => nc.Id == id);

                if (notaCredito == null)
                {
                    return NotFound(new { message = "Nota crédito no encontrada" });
                }

                var response = new NotaCreditoResponseDto
                {
                    Id = notaCredito.Id,
                    NumeroNota = notaCredito.NumeroNota,
                    FacturaId = notaCredito.FacturaId,
                    NumeroFactura = notaCredito.Factura?.NumeroFactura ?? notaCredito.NumeroFactura,
                    Cliente = notaCredito.Cliente != null ? new ClienteSimpleDto
                    {
                        Id = notaCredito.Cliente.Id,
                        Nombre = notaCredito.Cliente.Nombre,
                        Apellido = notaCredito.Cliente.Apellido,
                        Documento = notaCredito.Cliente.NumeroIdentificacion
                    } : null,
                    Tipo = notaCredito.Tipo,
                    MotivoDIAN = notaCredito.MotivoDIAN,
                    FechaElaboracion = notaCredito.FechaElaboracion,
                    FechaRegistro = notaCredito.FechaRegistro,
                    CUFE = notaCredito.CUFE,
                    XMLBase64 = notaCredito.XMLBase64,
                    TotalBruto = notaCredito.TotalBruto,
                    TotalDescuentos = notaCredito.TotalDescuentos,
                    Subtotal = notaCredito.Subtotal,
                    TotalIVA = notaCredito.TotalIVA,
                    TotalINC = notaCredito.TotalINC,
                    ReteICA = notaCredito.ReteICA,
                    TotalNeto = notaCredito.TotalNeto,
                    Estado = notaCredito.Estado,
                    Observaciones = notaCredito.Observaciones,
                    DetalleNotaCredito = notaCredito.DetalleNotaCredito.Select(d => new DetalleNotaCreditoResponseDto
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
                    FormasPago = notaCredito.FormasPago.Select(fp => new FormaPagoNotaCreditoResponseDto
                    {
                        Id = fp.Id,
                        Metodo = fp.Metodo,
                        Valor = fp.Valor
                    }).ToList()
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener nota crédito", error = ex.Message });
            }
        }

        // POST: api/NotasCredito
        [HttpPost]
        public async Task<ActionResult<NotaCreditoResponseDto>> CreateNotaCredito([FromBody] NotaCreditoDto dto)
        {
            try
            {
                // Validar que la factura existe
                var factura = await _context.Facturas
                    .Include(f => f.Cliente)
                    .FirstOrDefaultAsync(f => f.Id == dto.FacturaId);

                if (factura == null)
                {
                    return BadRequest(new { message = "Factura no encontrada" });
                }

                // Crear nota crédito
                var notaCredito = new NotaCredito
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

                _context.NotasCredito.Add(notaCredito);
                await _context.SaveChangesAsync();

                // Agregar detalles
                foreach (var detalleDto in dto.DetalleNotaCredito)
                {
                    var detalle = new DetalleNotaCredito
                    {
                        NotaCreditoId = notaCredito.Id,
                        ProductoId = detalleDto.ProductoId,
                        Descripcion = detalleDto.Descripcion,
                        Cantidad = detalleDto.Cantidad,
                        UnidadMedida = detalleDto.UnidadMedida,
                        PrecioUnitario = detalleDto.PrecioUnitario,
                        PorcentajeDescuento = detalleDto.PorcentajeDescuento,
                        ValorDescuento = detalleDto.ValorDescuento,
                        SubtotalLinea = detalleDto.SubtotalLinea,
                        TarifaIVA = detalleDto.TarifaIVA,
                        ValorIVA = detalleDto.ValorIVA,
                        TarifaINC = detalleDto.TarifaINC,
                        ValorINC = detalleDto.ValorINC,
                        TotalLinea = detalleDto.TotalLinea
                    };
                    _context.DetalleNotaCredito.Add(detalle);
                }

                // Agregar formas de pago
                foreach (var formaPagoDto in dto.FormasPago)
                {
                    var formaPago = new FormaPagoNotaCredito
                    {
                        NotaCreditoId = notaCredito.Id,
                        Metodo = formaPagoDto.Metodo,
                        Valor = formaPagoDto.Valor
                    };
                    _context.FormasPagoNotaCredito.Add(formaPago);
                }

                await _context.SaveChangesAsync();

                // Retornar nota creada
                var notaCreada = await _context.NotasCredito
                    .Include(nc => nc.Cliente)
                    .Include(nc => nc.Factura)
                    .Include(nc => nc.DetalleNotaCredito)
                    .Include(nc => nc.FormasPago)
                    .FirstOrDefaultAsync(nc => nc.Id == notaCredito.Id);

                var response = new NotaCreditoResponseDto
                {
                    Id = notaCreada!.Id,
                    NumeroNota = notaCreada.NumeroNota,
                    FacturaId = notaCreada.FacturaId,
                    NumeroFactura = notaCreada.NumeroFactura,
                    Cliente = notaCreada.Cliente != null ? new ClienteSimpleDto
                    {
                        Id = notaCreada.Cliente.Id,
                        Nombre = notaCreada.Cliente.Nombre,
                        Apellido = notaCreada.Cliente.Apellido,
                        Documento = notaCreada.Cliente.NumeroIdentificacion
                    } : null,
                    Tipo = notaCreada.Tipo,
                    MotivoDIAN = notaCreada.MotivoDIAN,
                    FechaElaboracion = notaCreada.FechaElaboracion,
                    FechaRegistro = notaCreada.FechaRegistro,
                    CUFE = notaCreada.CUFE,
                    XMLBase64 = notaCreada.XMLBase64,
                    TotalBruto = notaCreada.TotalBruto,
                    TotalDescuentos = notaCreada.TotalDescuentos,
                    Subtotal = notaCreada.Subtotal,
                    TotalIVA = notaCreada.TotalIVA,
                    TotalINC = notaCreada.TotalINC,
                    ReteICA = notaCreada.ReteICA,
                    TotalNeto = notaCreada.TotalNeto,
                    Estado = notaCreada.Estado,
                    Observaciones = notaCreada.Observaciones,
                    DetalleNotaCredito = notaCreada.DetalleNotaCredito.Select(d => new DetalleNotaCreditoResponseDto
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
                    FormasPago = notaCreada.FormasPago.Select(fp => new FormaPagoNotaCreditoResponseDto
                    {
                        Id = fp.Id,
                        Metodo = fp.Metodo,
                        Valor = fp.Valor
                    }).ToList()
                };

                return CreatedAtAction(nameof(GetNotaCredito), new { id = notaCreada.Id }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear nota crédito", error = ex.Message });
            }
        }

        // PUT: api/NotasCredito/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotaCredito(int id, [FromBody] NotaCreditoDto dto)
        {
            try
            {
                var notaCredito = await _context.NotasCredito
                    .Include(nc => nc.DetalleNotaCredito)
                    .Include(nc => nc.FormasPago)
                    .FirstOrDefaultAsync(nc => nc.Id == id);

                if (notaCredito == null)
                {
                    return NotFound(new { message = "Nota crédito no encontrada" });
                }

                // Actualizar campos principales
                notaCredito.Tipo = dto.Tipo;
                notaCredito.MotivoDIAN = dto.MotivoDIAN;
                notaCredito.FechaElaboracion = dto.FechaElaboracion;
                notaCredito.CUFE = dto.CUFE;
                notaCredito.TotalBruto = dto.TotalBruto;
                notaCredito.TotalDescuentos = dto.TotalDescuentos;
                notaCredito.Subtotal = dto.Subtotal;
                notaCredito.TotalIVA = dto.TotalIVA;
                notaCredito.TotalINC = dto.TotalINC;
                notaCredito.ReteICA = dto.ReteICA;
                notaCredito.TotalNeto = dto.TotalNeto;
                notaCredito.Estado = dto.Estado;
                notaCredito.Observaciones = dto.Observaciones;

                // Eliminar detalles antiguos
                _context.DetalleNotaCredito.RemoveRange(notaCredito.DetalleNotaCredito);

                // Agregar nuevos detalles
                foreach (var detalleDto in dto.DetalleNotaCredito)
                {
                    var detalle = new DetalleNotaCredito
                    {
                        NotaCreditoId = notaCredito.Id,
                        ProductoId = detalleDto.ProductoId,
                        Descripcion = detalleDto.Descripcion,
                        Cantidad = detalleDto.Cantidad,
                        UnidadMedida = detalleDto.UnidadMedida,
                        PrecioUnitario = detalleDto.PrecioUnitario,
                        PorcentajeDescuento = detalleDto.PorcentajeDescuento,
                        ValorDescuento = detalleDto.ValorDescuento,
                        SubtotalLinea = detalleDto.SubtotalLinea,
                        TarifaIVA = detalleDto.TarifaIVA,
                        ValorIVA = detalleDto.ValorIVA,
                        TarifaINC = detalleDto.TarifaINC,
                        ValorINC = detalleDto.ValorINC,
                        TotalLinea = detalleDto.TotalLinea
                    };
                    _context.DetalleNotaCredito.Add(detalle);
                }

                // Eliminar formas de pago antiguas
                _context.FormasPagoNotaCredito.RemoveRange(notaCredito.FormasPago);

                // Agregar nuevas formas de pago
                foreach (var formaPagoDto in dto.FormasPago)
                {
                    var formaPago = new FormaPagoNotaCredito
                    {
                        NotaCreditoId = notaCredito.Id,
                        Metodo = formaPagoDto.Metodo,
                        Valor = formaPagoDto.Valor
                    };
                    _context.FormasPagoNotaCredito.Add(formaPago);
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = "Nota crédito actualizada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar nota crédito", error = ex.Message });
            }
        }

        // DELETE: api/NotasCredito/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotaCredito(int id)
        {
            try
            {
                var notaCredito = await _context.NotasCredito
                    .Include(nc => nc.DetalleNotaCredito)
                    .Include(nc => nc.FormasPago)
                    .FirstOrDefaultAsync(nc => nc.Id == id);

                if (notaCredito == null)
                {
                    return NotFound(new { message = "Nota crédito no encontrada" });
                }

                _context.DetalleNotaCredito.RemoveRange(notaCredito.DetalleNotaCredito);
                _context.FormasPagoNotaCredito.RemoveRange(notaCredito.FormasPago);
                _context.NotasCredito.Remove(notaCredito);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Nota crédito eliminada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar nota crédito", error = ex.Message });
            }
        }
    }
}
