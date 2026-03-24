// Controllers/ResolucionesController.cs
using FactCloudAPI.Data;
using FactCloudAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FactCloudAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ResolucionesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ResolucionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int? ObtenerUsuarioId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : null;
        }

        // GET api/resoluciones — obtener resoluciones del negocio
        [HttpGet]
        public async Task<IActionResult> ObtenerResoluciones()
        {
            var usuarioId = ObtenerUsuarioId();
            if (usuarioId == null) return Unauthorized();

            var negocio = await _context.Negocios
                .Include(n => n.Resoluciones)
                .FirstOrDefaultAsync(n => n.UsuarioId == usuarioId.Value);

            if (negocio == null)
                return NotFound(new { mensaje = "Negocio no encontrado." });

            return Ok(negocio.Resoluciones.OrderByDescending(r => r.FechaRegistro));
        }

        // POST api/resoluciones — registrar nueva resolución DIAN
        [HttpPost]
        public async Task<IActionResult> RegistrarResolucion([FromBody] ResolucionDIAN dto)
        {
            var usuarioId = ObtenerUsuarioId();
            if (usuarioId == null) return Unauthorized();

            var negocio = await _context.Negocios
                .Include(n => n.Resoluciones)
                .FirstOrDefaultAsync(n => n.UsuarioId == usuarioId.Value);

            if (negocio == null)
                return NotFound(new { mensaje = "Negocio no encontrado." });

            // Desactivar resoluciones anteriores del mismo prefijo
            var anteriores = negocio.Resoluciones
                .Where(r => r.Prefijo == dto.Prefijo)
                .ToList();
            anteriores.ForEach(r => r.Activa = false);

            dto.NegocioId = negocio.Id;
            dto.Activa = true;
            dto.FechaRegistro = DateTime.UtcNow;

            _context.ResolucionesDIAN.Add(dto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObtenerResoluciones), new { id = dto.Id }, dto);
        }

        // PUT api/resoluciones/5/desactivar
        [HttpPut("{id}/desactivar")]
        public async Task<IActionResult> DesactivarResolucion(int id)
        {
            var usuarioId = ObtenerUsuarioId();
            if (usuarioId == null) return Unauthorized();

            var resolucion = await _context.ResolucionesDIAN
                .Include(r => r.Negocio)
                .FirstOrDefaultAsync(r => r.Id == id
                                       && r.Negocio.UsuarioId == usuarioId.Value);

            if (resolucion == null)
                return NotFound(new { mensaje = "Resolución no encontrada." });

            resolucion.Activa = false;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Resolución desactivada correctamente." });
        }
    }
}