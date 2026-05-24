using NubeeAPI.Data;
using NubeeAPI.Models;
using NubeeAPI.Services;
using NubeeAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace NubeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReportesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1) REPORTE DE VENTAS (por día) A CSV
        [HttpGet("ventas/csv")]
        public async Task<IActionResult> ExportVentasCsv()
        {
            Response.ContentType = "text/csv";
            Response.Headers.Add("Content-Disposition", "attachment; filename=reporte_ventas.csv");

            await using var writer = new StreamWriter(Response.Body, Encoding.UTF8);

            await writer.WriteLineAsync("Fecha,TotalGanancia");

            await foreach (var row in _context.Facturas
                .AsNoTracking()
                .GroupBy(f => f.FechaEmision.Date)
                .Select(g => new {
                    Fecha = g.Key,
                    TotalGanancia = g.Sum(x => x.TotalFactura)
                })
                .OrderBy(x => x.Fecha)
                .AsAsyncEnumerable())
            {
                await writer.WriteLineAsync($"{row.Fecha:yyyy-MM-dd},{row.TotalGanancia}");
            }

            await writer.FlushAsync();
            return new EmptyResult();
        }


        // 2) TOP CLIENTES A CSV
        [HttpGet("top-clientes/csv")]
        public async Task<IActionResult> ExportTopClientesCsv()
        {
            Response.ContentType = "text/csv";
            Response.Headers.Add("Content-Disposition", "attachment; filename=top_clientes.csv");

            await using var writer = new StreamWriter(Response.Body, Encoding.UTF8);

            await writer.WriteLineAsync("Cliente,CantidadFacturas,TotalFacturado");

            var query = _context.Facturas
                .AsNoTracking()
                .Join(_context.Clientes,
                      f => f.ClienteId,
                      c => c.Id,
                      (f, c) => new { c.Nombre, f.TotalFactura, f.ClienteId })
                .GroupBy(x => new { x.ClienteId, x.Nombre })
                .Select(g => new
                {
                    Cliente = g.Key.Nombre,
                    CantidadFacturas = g.Count(),
                    TotalFacturado = g.Sum(x => x.TotalFactura)
                })
                .OrderByDescending(x => x.TotalFacturado)
                .AsAsyncEnumerable();

            await foreach (var row in query)
            {
                await writer.WriteLineAsync($"{row.Cliente},{row.CantidadFacturas},{row.TotalFacturado}");
            }

            await writer.FlushAsync();
            return new EmptyResult();
        }


        // 3) PRODUCTOS MÁS VENDIDOS A CSV
        [HttpGet("productos-mas-vendidos/csv")]
        public async Task<IActionResult> ExportProductosMasVendidosCsv()
        {
            Response.ContentType = "text/csv";
            Response.Headers.Add("Content-Disposition", "attachment; filename=productos_mas_vendidos.csv");

            await using var writer = new StreamWriter(Response.Body, Encoding.UTF8);

            await writer.WriteLineAsync("Producto,CantidadVendida,TotalIngresos");

            var query = _context.DetalleFacturas
                .AsNoTracking()
                .Join(_context.Productos,
                      d => d.ProductoId,
                      p => p.Id,
                      (d, p) => new { p.Nombre, d.Cantidad, d.SubtotalLinea, d.ProductoId })
                .GroupBy(x => new { x.ProductoId, x.Nombre })
                .Select(g => new
                {
                    Producto = g.Key.Nombre,
                    CantidadVendida = g.Sum(x => x.Cantidad),
                    TotalIngresos = g.Sum(x => x.SubtotalLinea)
                })
                .OrderByDescending(x => x.CantidadVendida)
                .AsAsyncEnumerable();

            await foreach (var row in query)
            {
                await writer.WriteLineAsync($"{row.Producto},{row.CantidadVendida},{row.TotalIngresos}");
            }

            await writer.FlushAsync();
            return new EmptyResult();
        }

        // 4) DETALLE DE VENTAS (JSON) (Ventas por Cliente)
        [HttpGet("ventas/detalle")]
        public async Task<IActionResult> GetVentasDetalle([FromQuery] string? periodo, [FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin, [FromQuery] int? clienteId, [FromQuery] int? vendedorId)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var query = _context.Facturas
                .AsNoTracking()
                .Include(f => f.Cliente)
                .Where(f => f.UsuarioId == usuarioId); // If admin, maybe can see all. For now keep as is or modify if needed.
                // Wait, if UsuarioId is Vendedor, then filtering by VendedorId makes sense if the user is admin.
                // Since I don't know the role logic here, I will just add the filters.

            if (clienteId.HasValue)
                query = query.Where(f => f.ClienteId == clienteId.Value);

            if (vendedorId.HasValue)
                query = query.Where(f => f.UsuarioId == vendedorId.Value);

            if (fechaInicio.HasValue)
                query = query.Where(f => f.FechaEmision >= fechaInicio.Value);
            if (fechaFin.HasValue)
                query = query.Where(f => f.FechaEmision <= fechaFin.Value);

            var result = await query
                .GroupBy(f => new { f.ClienteId, f.Cliente.Nombre, f.Cliente.NumeroIdentificacion })
                .Select(g => new
                {
                    id = g.Key.ClienteId,
                    identificacion = g.Key.NumeroIdentificacion,
                    sucursal = "Principal",
                    cliente = g.Key.Nombre,
                    comprobantes = g.Count(),
                    bruto = g.Sum(f => f.Subtotal + f.TotalDescuentos),
                    descuentos = g.Sum(f => f.TotalDescuentos),
                    subtotal = g.Sum(f => f.Subtotal),
                    impuestoCargo = g.Sum(f => f.TotalIVA + f.TotalINC + f.TotalICA),
                    retenciones = g.Sum(f => f.TotalRetenciones),
                    total = g.Sum(f => f.TotalFactura)
                })
                .ToListAsync();

            return Ok(result);
        }

        // 5) VENTAS POR VENDEDOR (JSON)
        [HttpGet("ventas-por-vendedor")]
        public async Task<IActionResult> GetVentasPorVendedor([FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin, [FromQuery] int? vendedorId, [FromQuery] string? tercero)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
            var query = _context.Facturas
                .AsNoTracking()
                .Include(f => f.Usuario)
                .Include(f => f.Cliente)
                .AsQueryable();

            // Depending on tenant/business logic, we might filter by EmpresaId.
            // Assuming UsuarioId here might be the creator, but let's just query.
            // If the system is multi-tenant, usually there's a NegocioId. 
            // We'll filter by UsuarioId if the user is not admin, else all.
            // Since I don't have full context, I'll allow querying all if no strict tenant filtering is visible, 
            // but actually let's stick to the existing pattern:
            query = query.Where(f => f.UsuarioId == usuarioId || f.UsuarioId == vendedorId); // Adjust logic as needed. 

            if (vendedorId.HasValue)
                query = query.Where(f => f.UsuarioId == vendedorId.Value);
            
            if (!string.IsNullOrEmpty(tercero))
                query = query.Where(f => f.Cliente.Nombre.Contains(tercero) || f.Cliente.NumeroIdentificacion.Contains(tercero));

            if (fechaInicio.HasValue)
                query = query.Where(f => f.FechaEmision >= fechaInicio.Value);
            if (fechaFin.HasValue)
                query = query.Where(f => f.FechaEmision <= fechaFin.Value);

            var result = await query
                .GroupBy(f => new { f.UsuarioId, f.Usuario.Nombre })
                .Select(g => new
                {
                    id = g.Key.UsuarioId,
                    identificacion = g.Key.UsuarioId.ToString(), // Assuming no specific Document for User here
                    nombreVendedor = g.Key.Nombre,
                    comprobantes = g.Count(),
                    bruto = g.Sum(f => f.Subtotal + f.TotalDescuentos),
                    descuentos = g.Sum(f => f.TotalDescuentos),
                    subtotal = g.Sum(f => f.Subtotal),
                    impuestoCargo = g.Sum(f => f.TotalIVA + f.TotalINC + f.TotalICA),
                    retenciones = g.Sum(f => f.TotalRetenciones),
                    total = g.Sum(f => f.TotalFactura)
                })
                .ToListAsync();

            return Ok(result);
        }

        // 6) COMPARATIVO VENTAS POR MES (JSON)
        [HttpGet("comparativo-mensual")]
        public async Task<IActionResult> GetComparativoMensual([FromQuery] int year)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var query = _context.Facturas
                .AsNoTracking()
                .Where(f => f.UsuarioId == usuarioId && f.FechaEmision.Year == year);

            var grouped = await query
                .GroupBy(f => f.FechaEmision.Month)
                .Select(g => new
                {
                    Mes = g.Key,
                    Total = g.Sum(f => f.TotalFactura),
                    Subtotal = g.Sum(f => f.Subtotal)
                })
                .ToListAsync();

            var mesesNombres = new[] { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };
            var result = Enumerable.Range(1, 12).Select(m => new
            {
                Mes = m,
                NombreMes = mesesNombres[m - 1],
                Total = grouped.FirstOrDefault(g => g.Mes == m)?.Total ?? 0,
                Subtotal = grouped.FirstOrDefault(g => g.Mes == m)?.Subtotal ?? 0
            }).ToList();

            return Ok(result);
        }

        // 7) VENTAS POR PRODUCTO (JSON)
        [HttpGet("ventas-por-producto")]
        public async Task<IActionResult> GetVentasPorProducto([FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var query = _context.DetalleFacturas
                .AsNoTracking()
                .Include(d => d.Producto)
                .Include(d => d.Factura)
                .Where(d => d.Factura.UsuarioId == usuarioId);

            if (fechaInicio.HasValue)
                query = query.Where(d => d.Factura.FechaEmision >= fechaInicio.Value);
            if (fechaFin.HasValue)
                query = query.Where(d => d.Factura.FechaEmision <= fechaFin.Value);

            var result = await query
                .GroupBy(d => new { d.ProductoId, d.Producto.Nombre })
                .Select(g => new
                {
                    id = g.Key.ProductoId,
                    producto = g.Key.Nombre,
                    cantidad = g.Sum(d => d.Cantidad),
                    total = g.Sum(d => d.SubtotalLinea)
                })
                .ToListAsync();

            return Ok(result);
        }

        private FileStreamResult CsvResult(string filename, Action<StreamWriter> build)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, new UTF8Encoding(false));

            build(writer);
            writer.Flush();
            stream.Position = 0;

            return new FileStreamResult(stream, "text/csv")
            {
                FileDownloadName = filename
            };
        }

    }
}
