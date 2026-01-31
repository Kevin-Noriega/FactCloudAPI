using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Facturas;
using FactCloudAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace FactCloudAPI.Services.Facturas
{
    public class FacturaService : IFacturaService
    {
        private readonly ApplicationDbContext _context;

        public FacturaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FacturaDto>> ObtenerAsync(int usuarioId)
        {
            return await _context.Facturas
                .Include(f => f.Cliente)
                .Where(f => f.UsuarioId == usuarioId)
                .OrderByDescending(f => f.FechaEmision)
                .Select(f => new FacturaDto
                {
                    Id = f.Id,
                    NumeroFactura = f.NumeroFactura,
                    FechaEmision = f.FechaEmision,
                    Cliente = $"{f.Cliente.Nombre} {f.Cliente.Apellido}",
                    TotalFactura = f.TotalFactura,
                    Estado = f.Estado
                })
                .ToListAsync();
        }

        public async Task<FacturaDetalleDTO?> ObtenerPorIdAsync(int id, int usuarioId)
        {
            var factura = await _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.DetalleFacturas)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(f => f.Id == id && f.UsuarioId == usuarioId);

            if (factura == null) return null;

            return new FacturaDetalleDTO
            {
                Id = factura.Id,
                NumeroFactura = factura.NumeroFactura,
                FechaEmision = factura.FechaEmision,
                FechaVencimiento = factura.FechaVencimiento,
                Cliente = $"{factura.Cliente.Nombre} {factura.Cliente.Apellido}",
                ClienteCorreo = factura.Cliente.Correo,
                Subtotal = factura.Subtotal,
                TotalIVA = factura.TotalIVA,
                TotalINC = factura.TotalINC,
                TotalFactura = factura.TotalFactura,
                Estado = factura.Estado,
                MedioPago = factura.MedioPago,
                FormaPago = factura.FormaPago,
                EnviadaDIAN = factura.EnviadaDIAN,
                EnviadaCliente = factura.EnviadaCliente,
                Items = factura.DetalleFacturas.Select(d => new FacturaDetalleItemDto
                {
                    ProductoId = d.ProductoId,
                    Producto = d.Producto.Nombre,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Total = d.Cantidad * d.PrecioUnitario
                }).ToList()
            };
        }

        public async Task<int> CrearAsync(FacturaCreateDto dto, int usuarioId)
        {
            // aquí va lógica de stock, totales, impuestos, etc.
            // (la dejamos lista para siguiente paso)
            var factura = new Factura
            {
                UsuarioId = usuarioId,
                ClienteId = dto.ClienteId,
                FormaPago = dto.FormaPago,
                MedioPago = dto.MedioPago,
                DiasCredito = dto.DiasCredito,
                Observaciones = dto.Observaciones
            };

            factura.CalcularFechas();
            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();

            return factura.Id;
        }

        public async Task RegistrarPagoAsync(int id, FacturaPagoDto dto, int usuarioId)
        {
            var factura = await _context.Facturas
                .FirstOrDefaultAsync(f => f.Id == id && f.UsuarioId == usuarioId);

            if (factura == null)
                throw new Exception("Factura no encontrada");

            factura.Estado = dto.Estado;
            factura.MedioPago = dto.MedioPago;
            factura.FormaPago = dto.FormaPago;
            factura.MontoPagado = dto.MontoPagado;
            factura.FechaPago = dto.FechaPago ?? DateTime.Now;

            factura.Cufe = CufeService.GenerarCUFE(factura);

            await _context.SaveChangesAsync();
        }

        public async Task EnviarClienteAsync(int id, int usuarioId)
        {
            // email logic
        }

        public async Task EnviarDIANAsync(int id, int usuarioId)
        {
            // DIAN logic
        }
    }

}
