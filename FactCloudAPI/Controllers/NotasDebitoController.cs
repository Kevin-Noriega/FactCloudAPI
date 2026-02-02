using FactCloudAPI.Data;
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
    public class NotasDebitoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NotasDebitoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/NotasDebito
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotaDebitoResponseDto>>> GetNotasDebito()
        {
            try
            {
                var notasDebito = await _context.NotasDebito
                    .Include(nd => nd.Cliente)
                    .Include(nd => nd.Factura)
                    .Include(nd => nd.DetalleNotaDebito)
                    .Include(nd => nd.FormasPago)
                    .OrderByDescending(nd => nd.FechaRegistro)
                    .ToListAsync();

                var response = notasDebito.Select(nd => new NotaDebitoResponseDto
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
                    DetalleNotaDebito = nd.DetalleNotaDebito.Select(d => new DetalleNotaDebitoResponseDto
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
                    .Include(nd => nd.Cliente)
                    .Include(nd => nd.Factura)
                    .Include(nd => nd.DetalleNotaDebito)
                    .Include(nd => nd.FormasPago)
                    .FirstOrDefaultAsync(nd => nd.Id == id);

                if (notaDebito == null)
                {
                    return NotFound(new { message = "Nota débito no encontrada" });
                }

                var response = new NotaDebitoResponseDto
                {
                    Id = notaDebito.Id,
                    NumeroNota = notaDebito.NumeroNota,
                    FacturaId = notaDebito.FacturaId,
                    NumeroFactura = notaDebito.Factura?.NumeroFactura ?? notaDebito.NumeroFactura,
                    Cliente = notaDebito.Cliente != null ? new ClienteSimpleDto
                    {
                        Id = notaDebito.Cliente.Id,
                        Nombre = notaDebito.Cliente.Nombre,
                        Apellido = notaDebito.Cliente.Apellido,
                        Documento = notaDebito.Cliente.NumeroIdentificacion
                    } : null,
                    Tipo = notaDebito.Tipo,
                    MotivoDIAN = notaDebito.MotivoDIAN,
                    FechaElaboracion = notaDebito.FechaElaboracion,
                    FechaRegistro = notaDebito.FechaRegistro,
                    CUFE = notaDebito.CUFE,
                    XMLBase64 = notaDebito.XMLBase64,
                    TotalBruto = notaDebito.TotalBruto,
                    TotalDescuentos = notaDebito.TotalDescuentos,
                    Subtotal = notaDebito.Subtotal,
                    TotalIVA = notaDebito.TotalIVA,
                    TotalINC = notaDebito.TotalINC,
                    ReteICA = notaDebito.ReteICA,
                    TotalNeto = notaDebito.TotalNeto,
                    Estado = notaDebito.Estado,
                    Observaciones = notaDebito.Observaciones,
                    DetalleNotaDebito = notaDebito.DetalleNotaDebito.Select(d => new DetalleNotaDebitoResponseDto
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
                    FormasPago = notaDebito.FormasPago.Select(fp => new FormaPagoResponseDto
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
                return StatusCode(500, new { message = "Error al obtener nota débito", error = ex.Message });
            }
        }

        // POST: api/NotasDebito
        [HttpPost]
        public async Task<ActionResult<NotaDebitoResponseDto>> CreateNotaDebito([FromBody] NotaDebitoDto dto)
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

                // Crear nota débito
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

                // Agregar detalles
                foreach (var detalleDto in dto.DetalleNotaDebito)
                {
                    var detalle = new DetalleNotaDebito
                    {
                        NotaDebitoId = notaDebito.Id,
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
                    _context.DetalleNotaDebito.Add(detalle);
                }

                // Agregar formas de pago
                foreach (var formaPagoDto in dto.FormasPago)
                {
                    var formaPago = new FormaPagoNotaDebito
                    {
                        NotaDebitoId = notaDebito.Id,
                        Metodo = formaPagoDto.Metodo,
                        Valor = formaPagoDto.Valor
                    };
                    _context.FormasPagoNotaDebito.Add(formaPago);
                }

                await _context.SaveChangesAsync();

                // Retornar nota creada
                var notaCreada = await _context.NotasDebito
                    .Include(nd => nd.Cliente)
                    .Include(nd => nd.Factura)
                    .Include(nd => nd.DetalleNotaDebito)
                    .Include(nd => nd.FormasPago)
                    .FirstOrDefaultAsync(nd => nd.Id == notaDebito.Id);

                var response = new NotaDebitoResponseDto
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
                    DetalleNotaDebito = notaCreada.DetalleNotaDebito.Select(d => new DetalleNotaDebitoResponseDto
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
                    FormasPago = notaCreada.FormasPago.Select(fp => new FormaPagoResponseDto
                    {
                        Id = fp.Id,
                        Metodo = fp.Metodo,
                        Valor = fp.Valor
                    }).ToList()
                };

                return CreatedAtAction(nameof(GetNotaDebito), new { id = notaCreada.Id }, response);
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
                    .Include(nd => nd.DetalleNotaDebito)
                    .Include(nd => nd.FormasPago)
                    .FirstOrDefaultAsync(nd => nd.Id == id);

                if (notaDebito == null)
                {
                    return NotFound(new { message = "Nota débito no encontrada" });
                }

                // Actualizar campos principales
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

                // Eliminar detalles antiguos
                _context.DetalleNotaDebito.RemoveRange(notaDebito.DetalleNotaDebito);

                // Agregar nuevos detalles
                foreach (var detalleDto in dto.DetalleNotaDebito)
                {
                    var detalle = new DetalleNotaDebito
                    {
                        NotaDebitoId = notaDebito.Id,
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
                    _context.DetalleNotaDebito.Add(detalle);
                }

                // Eliminar formas de pago antiguas
                _context.FormasPagoNotaDebito.RemoveRange(notaDebito.FormasPago);

                // Agregar nuevas formas de pago
                foreach (var formaPagoDto in dto.FormasPago)
                {
                    var formaPago = new FormaPagoNotaDebito
                    {
                        NotaDebitoId = notaDebito.Id,
                        Metodo = formaPagoDto.Metodo,
                        Valor = formaPagoDto.Valor
                    };
                    _context.FormasPagoNotaDebito.Add(formaPago);
                }

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
                    .Include(nd => nd.DetalleNotaDebito)
                    .Include(nd => nd.FormasPago)
                    .FirstOrDefaultAsync(nd => nd.Id == id);

                if (notaDebito == null)
                {
                    return NotFound(new { message = "Nota débito no encontrada" });
                }

                _context.DetalleNotaDebito.RemoveRange(notaDebito.DetalleNotaDebito);
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
    }
}
