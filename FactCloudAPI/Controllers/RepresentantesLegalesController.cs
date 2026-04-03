using FactCloudAPI.Data;
using FactCloudAPI.Models.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FactCloudAPI.Controllers
{
    [ApiController]
    [Route("api/negocios/{negocioId:int}/representante-legal")]
    [Authorize]
    public class RepresentantesLegalesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RepresentantesLegalesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET api/negocios/5/representante-legal
        [HttpGet]
        public async Task<IActionResult> Get(int negocioId)
        {
            if (!await OwnsNegocio(negocioId)) return Forbid();

            var rep = await _context.RepresentantesLegales
                .FirstOrDefaultAsync(r => r.NegocioId == negocioId);

            if (rep == null)
                return NotFound(new { mensaje = "Representante legal no configurado aún." });

            return Ok(rep);
        }

        // POST api/negocios/5/representante-legal
        [HttpPost]
        public async Task<IActionResult> Crear(int negocioId, [FromBody] RepresentanteLegal dto)
        {
            if (!await OwnsNegocio(negocioId)) return Forbid();

            var existe = await _context.RepresentantesLegales
                .AnyAsync(r => r.NegocioId == negocioId);
            if (existe)
                return Conflict(new { mensaje = "Ya existe un representante. Usa PUT para actualizarlo." });

            dto.NegocioId = negocioId;
            _context.RepresentantesLegales.Add(dto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { negocioId }, dto);
        }

        // PUT api/negocios/5/representante-legal
        [HttpPut]
        public async Task<IActionResult> Actualizar(int negocioId, [FromBody] RepresentanteLegal dto)
        {
            if (!await OwnsNegocio(negocioId)) return Forbid();

            var rep = await _context.RepresentantesLegales
                .FirstOrDefaultAsync(r => r.NegocioId == negocioId);

            if (rep == null)
            {
                dto.NegocioId = negocioId;
                _context.RepresentantesLegales.Add(dto);
                await _context.SaveChangesAsync();
                return Ok(dto);
            }

            rep.Nombre = dto.Nombre;
            rep.Apellidos = dto.Apellidos;
            rep.TipoDocumento = dto.TipoDocumento;
            rep.NumeroIdentificacion = dto.NumeroIdentificacion;
            rep.CiudadExpedicion = dto.CiudadExpedicion;
            rep.CiudadResidencia = dto.CiudadResidencia;

            await _context.SaveChangesAsync();
            return Ok(rep);
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