using FactCloudAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FactCloudAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AddonsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AddonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/addons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAddons()
        {
            var addons = await _context.Addons
                .Where(a => a.Activo)
                .Select(a => new
                {
                    id = a.Id,
                    nombre = a.Nombre,
                    descripcion = a.Descripcion,
                    precio = a.Precio,
                    unidad = a.Unidad,
                    tipo = a.Tipo,
                    color = a.Color
                })
                .ToListAsync();

            return Ok(addons);
        }

        // POST: api/addons/agregar
        [HttpPost("agregar")]
        public async Task<ActionResult> AgregarAddons([FromBody] AgregarAddonsRequest request)
        {
            var userId = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var usuario = await _context.Usuarios
                .Include(u => u.SuscripcionActual)
                .FirstOrDefaultAsync(u => u.Id == int.Parse(userId));

            if (usuario?.SuscripcionActual == null)
                return NotFound("No se encontró suscripción activa");

            // Aquí implementarías la lógica para agregar addons
            // Por ahora retornamos éxito
            return Ok(new { mensaje = "Complementos agregados exitosamente" });
        }
    }

    public class AgregarAddonsRequest
    {
        public List<int> Addons { get; set; } = new List<int>();
    }
}
