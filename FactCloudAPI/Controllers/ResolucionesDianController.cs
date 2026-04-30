using FactCloudAPI.Data;
using FactCloudAPI.Models;
using FactCloudAPI.Models.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FactCloudAPI.Controllers
{
    [ApiController]
    [Route("api/negocios/{negocioId:int}/resoluciones-dian")]
    [Authorize]
    public class ResolucionesDianController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ResolucionesDianController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET api/negocios/5/resoluciones-dian
        // Lista todas las resoluciones/rangos de un negocio
        [HttpGet]
        public async Task<IActionResult> GetAll(int negocioId)
        {
            if (!await OwnsNegocio(negocioId)) return Forbid();

            var resoluciones = await _context.ResolucionesDIAN
                .Where(r => r.NegocioId == negocioId)
                .OrderByDescending(r => r.FechaInicio)
                .ToListAsync();

            return Ok(resoluciones);
        }

        // GET api/negocios/5/resoluciones-dian/3
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int negocioId, int id)
        {
            if (!await OwnsNegocio(negocioId)) return Forbid();

            var resolucion = await _context.ResolucionesDIAN
                .FirstOrDefaultAsync(r => r.Id == id && r.NegocioId == negocioId);

            if (resolucion == null)
                return NotFound(new { mensaje = "Resolución no encontrada." });

            return Ok(resolucion);
        }

        // GET api/negocios/5/resoluciones-dian/activa
        // Devuelve la resolución actualmente activa (para generar documentos)
        [HttpGet("activa")]
        public async Task<IActionResult> GetActiva(int negocioId)
        {
            if (!await OwnsNegocio(negocioId)) return Forbid();

            var activa = await _context.ResolucionesDIAN
                .Where(r => r.NegocioId == negocioId && r.Activa)
                .FirstOrDefaultAsync();

            if (activa == null)
                return NotFound(new { mensaje = "No hay una resolución activa configurada." });

            return Ok(activa);
        }

        // POST api/negocios/5/resoluciones-dian
        [HttpPost]
        public async Task<IActionResult> Crear(int negocioId, [FromBody] ResolucionDIAN dto)
        {
            if (!await OwnsNegocio(negocioId)) return Forbid();

            // Si viene como activa, desactivar las anteriores
            if (dto.Activa)
            {
                var anteriores = await _context.ResolucionesDIAN
                    .Where(r => r.NegocioId == negocioId && r.Activa)
                    .ToListAsync();
                anteriores.ForEach(r => r.Activa = false);
            }

            dto.NegocioId = negocioId;
            _context.ResolucionesDIAN.Add(dto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { negocioId, id = dto.Id }, dto);
        }

        // PUT api/negocios/5/resoluciones-dian/3
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int negocioId, int id, [FromBody] ResolucionDIAN dto)
        {
            if (!await OwnsNegocio(negocioId)) return Forbid();

            var resolucion = await _context.ResolucionesDIAN
                .FirstOrDefaultAsync(r => r.Id == id && r.NegocioId == negocioId);

            if (resolucion == null)
                return NotFound(new { mensaje = "Resolución no encontrada." });

            // Si se activa esta, desactivar las demás
            if (dto.Activa && !resolucion.Activa)
            {
                var otras = await _context.ResolucionesDIAN
                    .Where(r => r.NegocioId == negocioId && r.Id != id && r.Activa)
                    .ToListAsync();
                otras.ForEach(r => r.Activa = false);
            }

            resolucion.Prefijo = dto.Prefijo;
            resolucion.NumeroAutorizacion = dto.NumeroAutorizacion;
            resolucion.RangoDesde = dto.RangoDesde;
            resolucion.RangoHasta = dto.RangoHasta;
            resolucion.FechaInicio = dto.FechaInicio;
            resolucion.FechaFin = dto.FechaFin;
            resolucion.ClaveTecnica = dto.ClaveTecnica;
            resolucion.Activa = dto.Activa;

            await _context.SaveChangesAsync();
            return Ok(resolucion);
        }

        // DELETE api/negocios/5/resoluciones-dian/3
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int negocioId, int id)
        {
            if (!await OwnsNegocio(negocioId)) return Forbid();

            var resolucion = await _context.ResolucionesDIAN
                .FirstOrDefaultAsync(r => r.Id == id && r.NegocioId == negocioId);

            if (resolucion == null)
                return NotFound(new { mensaje = "Resolución no encontrada." });

            if (resolucion.Activa)
                return BadRequest(new { mensaje = "No puedes eliminar la resolución activa." });

            _context.ResolucionesDIAN.Remove(resolucion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ─── helpers ─────────────────────────────────────────────────

        private async Task<bool> OwnsNegocio(int negocioId)
        {
            var usuarioId = ObtenerUsuarioId();
            return await _context.Negocios
                .AnyAsync(n => n.Id == negocioId && n.UsuarioId == usuarioId);
        }

        private int ObtenerUsuarioId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? User.FindFirst("sub")?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }
    }
}