using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Busqueda;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FactCloudAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SearchController> _logger;

        public SearchController(ApplicationDbContext context, ILogger<SearchController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("global")]
        public async Task<IActionResult> GlobalSearch([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
                return Ok(new List<BusquedaGlobalDTO>());

            query = query.ToLower().Trim();

            var results = new List<BusquedaGlobalDTO>();

            // ── FACTURAS ──
            var facturas = await _context.Facturas
                .Where(f =>
                    EF.Functions.Like(f.Cufe.ToLower(), $"%{query}%") ||
                    EF.Functions.Like(f.NumeroFactura.ToLower(), $"%{query}%") ||
                    EF.Functions.Like(f.Cliente.Nombre.ToLower(), $"%{query}%") ||
                    EF.Functions.Like(f.Cliente.NumeroIdentificacion.ToLower(), $"%{query}%")
                )
                .Select(f => new BusquedaGlobalDTO
                {
                    Id = f.Id,
                    Title = f.NumeroFactura,
                    Subtitle = $"Factura ${f.TotalFactura:N0} - {f.Cliente.Nombre}",
                    Route = $"/facturas/{f.Id}",
                    Type = "factura",
                    Icon = "FileEarmarkTextFill"
                })
                .Take(5)
                .ToListAsync();

            // ── CLIENTES ──
            var clientes = await _context.Clientes
                .Where(c =>
                    EF.Functions.Like(c.Nombre.ToLower(), $"%{query}%") ||
                    EF.Functions.Like(c.NumeroIdentificacion.ToLower(), $"%{query}%") ||
                    EF.Functions.Like(c.Correo.ToLower(), $"%{query}%")
                )
                .Select(c => new BusquedaGlobalDTO
                {
                    Id = c.Id,
                    Title = c.Nombre,
                    Subtitle = c.NumeroIdentificacion + (c.EsProveedor ? " (Proveedor)" : ""),
                    Route = $"/clientes/{c.Id}",
                    Type = "cliente",
                    Icon = "PersonFill"
                })
                .Take(5)
                .ToListAsync();

            // ── PRODUCTOS ──
            var productos = await _context.Productos
                .Where(p =>
                    EF.Functions.Like(p.Nombre.ToLower(), $"%{query}%") ||
                    EF.Functions.Like(p.CodigoInterno.ToLower(), $"%{query}%")
                )
                .Select(p => new BusquedaGlobalDTO
                {
                    Id = p.Id,
                    Title = p.Nombre,
                    Subtitle = $"${p.PrecioUnitario:N0}",
                    Route = $"/productos/{p.Id}",
                    Type = "producto",
                    Icon = "BoxSeam"
                })
                .Take(5)
                .ToListAsync();

            // ── PÁGINAS Y ACCIONES ──
            var acciones = new List<BusquedaGlobalDTO>
    {
        // 🔹 NAVEGACIÓN
        new BusquedaGlobalDTO {
            Title = "Facturas",
            Subtitle = "Ver todas las facturas",
            Route = "/facturas",
            Type = "pagina",
            Icon = "FileEarmarkTextFill"
        },
        new BusquedaGlobalDTO {
            Title = "Clientes",
            Subtitle = "Gestión de clientes",
            Route = "/clientes",
            Type = "pagina",
            Icon = "PersonFill"
        },
        new BusquedaGlobalDTO {
            Title = "Productos",
            Subtitle = "Inventario",
            Route = "/productos",
            Type = "pagina",
            Icon = "BoxSeam"
        },

        // 🔹 ACCIONES
        new BusquedaGlobalDTO {
            Title = "Crear factura",
            Subtitle = "Registrar nueva factura",
            Route = "/nueva-factura",
            Type = "accion",
            Icon = "PlusCircleFill"
        },
        new BusquedaGlobalDTO {
            Title = "Crear nota crédito",
            Subtitle = "Registrar devolución",
            Route = "/nueva-nota-credito",
            Type = "accion",
            Icon = "ArrowCounterclockwise"
        },
        new BusquedaGlobalDTO {
            Title = "Crear nota débito",
            Subtitle = "Registrar ajuste",
            Route = "/nueva-nota-debito",
            Type = "accion",
            Icon = "ArrowClockwise"
        },
        new BusquedaGlobalDTO {
            Title = "Documento soporte",
            Subtitle = "Registrar compra proveedor",
            Route = "/nuevo-documento-soporte",
            Type = "accion",
            Icon = "FileText"
        },

        // 🔹 CONFIG / PERFIL
        new BusquedaGlobalDTO {
            Title = "Configuración",
            Subtitle = "Abrir configuración",
            Route = "/configuracion",
            Type = "modal",
            Icon = "GearFill"
        },
        new BusquedaGlobalDTO {
            Title = "Perfil",
            Subtitle = "Ver perfil de usuario",
            Route = "/perfil",
            Type = "modal",
            Icon = "PersonCircle"
        }
    };

            // 🔍 FILTRAR ACCIONES
            var accionesFiltradas = acciones
                .Where(a =>
                    a.Title.ToLower().Contains(query) ||
                    a.Subtitle.ToLower().Contains(query)
                )
                .Take(5)
                .ToList();

            // 🔥 UNIFICAR TODO
            results.AddRange(accionesFiltradas);
            results.AddRange(facturas);
            results.AddRange(clientes);
            results.AddRange(productos);

            // 🧠 ORDEN INTELIGENTE
            var ordered = results
                .OrderBy(r => r.Type switch
                {
                    "accion" => 0,
                    "pagina" => 1,
                    "factura" => 2,
                    "cliente" => 3,
                    "producto" => 4,
                    _ => 5
                })
                .ThenBy(r => r.Title)
                .Take(12)
                .ToList();

            return Ok(ordered);
        }
    }

}
