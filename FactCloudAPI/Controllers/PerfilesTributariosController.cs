using FactCloudAPI.Data;
using FactCloudAPI.Models.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FactCloudAPI.Controllers
{
    [ApiController]
    [Route("api/negocios/{negocioId:int}/perfil-tributario")]
    [Authorize]
    public class PerfilesTributariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PerfilesTributariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET api/negocios/5/perfil-tributario
        [HttpGet]
        public async Task<IActionResult> Get(int negocioId)
        {
            if (!await OwnsNegocio(negocioId)) return Forbid();

            var perfil = await _context.PerfilesTributarios
                .FirstOrDefaultAsync(p => p.NegocioId == negocioId);

            if (perfil == null)
                return NotFound(new { mensaje = "Perfil tributario no configurado aún." });

            return Ok(perfil);
        }

        // POST api/negocios/5/perfil-tributario
        [HttpPost]
        public async Task<IActionResult> Crear(int negocioId, [FromBody] PerfilTributario dto)
        {
            if (!await OwnsNegocio(negocioId)) return Forbid();

            var existe = await _context.PerfilesTributarios
                .AnyAsync(p => p.NegocioId == negocioId);
            if (existe)
                return Conflict(new { mensaje = "Ya existe un perfil tributario. Usa PUT para actualizarlo." });

            dto.NegocioId = negocioId;
            _context.PerfilesTributarios.Add(dto);
            await _context.SaveChangesAsync();

            // Verificar completitud del negocio
            await ActualizarCompletitud(negocioId);

            return CreatedAtAction(nameof(Get), new { negocioId }, dto);
        }

        // PUT api/negocios/5/perfil-tributario
        [HttpPut]
        public async Task<IActionResult> Actualizar(int negocioId, [FromBody] PerfilTributario dto)
        {
            if (!await OwnsNegocio(negocioId)) return Forbid();

            var perfil = await _context.PerfilesTributarios
                .FirstOrDefaultAsync(p => p.NegocioId == negocioId);

            if (perfil == null)
            {
                // Si no existe, lo creamos automáticamente
                dto.NegocioId = negocioId;
                _context.PerfilesTributarios.Add(dto);
                await _context.SaveChangesAsync();
                await ActualizarCompletitud(negocioId);
                return Ok(dto);
            }

            perfil.RegimenIvaCodigo = dto.RegimenIvaCodigo;
            perfil.ActividadEconomicaCIIU = dto.ActividadEconomicaCIIU;
            perfil.TributosJson = dto.TributosJson;
            perfil.ResponsabilidadesFiscalesJson = dto.ResponsabilidadesFiscalesJson;

            await _context.SaveChangesAsync();
            await ActualizarCompletitud(negocioId);

            return Ok(perfil);
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

        private async Task ActualizarCompletitud(int negocioId)
        {
            var negocio = await _context.Negocios
                .Include(n => n.PerfilTributario)
                .Include(n => n.RepresentanteLegal)
                .Include(n => n.ConfiguracionDIAN)
                .FirstOrDefaultAsync(n => n.Id == negocioId);

            if (negocio == null) return;

            negocio.DatosFacturacionCompletos =
                negocio.PerfilTributario != null &&
                negocio.RepresentanteLegal != null &&
                negocio.ConfiguracionDIAN != null;

            await _context.SaveChangesAsync();
        }
    }
}
