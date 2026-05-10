using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Facturas;
using FactCloudAPI.Models;
using Microsoft.EntityFrameworkCore;
using static FactCloudAPI.Models.Factura;

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
            // Traer primero a memoria para poder usar .ToString() en el enum
            var facturas = await _context.Facturas
                .Include(f => f.Cliente)
                .Where(f => f.UsuarioId == usuarioId)
                .OrderByDescending(f => f.FechaEmision)
                .ToListAsync(); // ← primero a memoria

            // ✅ Luego mapear con .ToString() fuera de la query SQL
            return facturas.Select(f => new FacturaDto
            {
                Id = f.Id,
                NumeroFactura = f.NumeroFactura,
                FechaEmision = f.FechaEmision,
                Cliente = $"{f.Cliente!.Nombre} {f.Cliente.Apellido}",
                TotalFactura = f.TotalFactura,
                Estado = f.Estado.ToString() 
            }).ToList();
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
                Cliente = $"{factura.Cliente!.Nombre} {factura.Cliente.Apellido}",
                ClienteCorreo = factura.Cliente.Correo,
                Subtotal = factura.Subtotal,
                TotalIVA = factura.TotalIVA,
                TotalINC = factura.TotalINC,
                TotalFactura = factura.TotalFactura,
                Estado = factura.Estado.ToString(),  // ✅ con ()
                MedioPago = factura.MedioPago,
                FormaPago = factura.FormaPago,
                EnviadaDIAN = factura.EnviadaDIAN,
                EnviadaCliente = factura.EnviadaCliente,
                Items = factura.DetalleFacturas!.Select(d => new FacturaDetalleItemDto
                {
                    ProductoId = d.ProductoId,
                    Producto = d.Producto!.Nombre,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Total = d.Cantidad * d.PrecioUnitario
                }).ToList()
            };
        }

        public async Task<int> CrearAsync(FacturaCreateDto dto, int usuarioId)
        {
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
                throw new KeyNotFoundException("Factura no encontrada");

            // ✅ Parsear string del DTO al enum de forma segura
            if (!string.IsNullOrEmpty(dto.Estado) &&
                Enum.TryParse<EstadoFactura>(dto.Estado, out var nuevoEstado))
                factura.Estado = nuevoEstado;

            factura.MedioPago = dto.MedioPago ?? factura.MedioPago;
            factura.FormaPago = dto.FormaPago ?? factura.FormaPago;
            factura.MontoPagado = dto.MontoPagado > 0 ? dto.MontoPagado : factura.MontoPagado;
            factura.FechaPago = dto.FechaPago ?? DateTime.Now;

            factura.Cufe = CufeService.GenerarCUFE(factura);

            await _context.SaveChangesAsync();
        }

        public async Task EnviarClienteAsync(int id, int usuarioId) { }
        public async Task EnviarDIANAsync(int id, int usuarioId) { }
    }
}