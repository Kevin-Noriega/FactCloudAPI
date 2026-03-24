using FactCloudAPI.Data;
using FactCloudAPI.Models;
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