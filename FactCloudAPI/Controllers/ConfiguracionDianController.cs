using FactCloudAPI.Data;
using FactCloudAPI.Models.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FactCloudAPI.Controllers
{
    [ApiController]
    [Route("api/negocios/{negocioId:int}/configuracion-dian")]
    [Authorize]
    public class ConfiguracionDianController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ConfiguracionDianController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET api/negocios/5/configuracion-dian
        [HttpGet]
        public async Task<IActionResult> Get(int negocioId)
        {
            if (!await OwnsNegocio(negocioId)) return Forbid();

            var config = await _context.ConfiguracionesDian
                .FirstOrDefaultAsync(c => c.NegocioId == negocioId);

            if (config == null)
                return NotFound(new { mensaje = "Configuración DIAN no registrada aún." });

            return Ok(config);
        }

        // POST api/negocios/5/configuracion-dian
        [HttpPost]
        public async Task<IActionResult> Crear(int negocioId, [FromBody] ConfiguracionDian dto)
        {
            if (!await OwnsNegocio(negocioId)) return Forbid();

            var existe = await _context.ConfiguracionesDian
                .AnyAsync(c => c.NegocioId == negocioId);
            if (existe)
                return Conflict(new { mensaje = "Ya existe una configuración. Usa PUT para actualizarla." });

            dto.NegocioId = negocioId;
            _context.ConfiguracionesDian.Add(dto);
            await _context.SaveChangesAsync();

            await ActualizarCompletitud(negocioId);

            return CreatedAtAction(nameof(Get), new { negocioId }, dto);
        }

        // PUT api/negocios/5/configuracion-dian
        [HttpPut]
        public async Task<IActionResult> Actualizar(int negocioId, [FromBody] ConfiguracionDian dto)
        {
            if (!await OwnsNegocio(negocioId)) return Forbid();

            var config = await _context.ConfiguracionesDian
                .FirstOrDefaultAsync(c => c.NegocioId == negocioId);

            if (config == null)
            {
                dto.NegocioId = negocioId;
                _context.ConfiguracionesDian.Add(dto);
                await _context.SaveChangesAsync();
                await ActualizarCompletitud(negocioId);
                return Ok(dto);
            }

            config.SoftwareProveedor = dto.SoftwareProveedor;
            config.SoftwarePIN = dto.SoftwarePIN;
            config.PrefijoAutorizadoDIAN = dto.PrefijoAutorizadoDIAN;
            config.NumeroResolucionDIAN = dto.NumeroResolucionDIAN;
            config.RangoNumeracionDesde = dto.RangoNumeracionDesde;
            config.RangoNumeracionHasta = dto.RangoNumeracionHasta;
            config.AmbienteDIAN = dto.AmbienteDIAN;
            config.FechaVigenciaInicio = dto.FechaVigenciaInicio;
            config.FechaVigenciaFinal = dto.FechaVigenciaFinal;

            await _context.SaveChangesAsync();
            await ActualizarCompletitud(negocioId);

            return Ok(config);
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
