using FactCloudAPI.Data;
using FactCloudAPI.DTOs.FotoPerfil;
using FactCloudAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FactCloudAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FotoPerfilController : Controller
    {

        private readonly ApplicationDbContext _context;

        public FotoPerfilController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost("subir")]
        [Authorize]
        public async Task<ActionResult<FotoPerfilDto>> SubirFotoPerfil([FromBody] SubirFotoPerfilDto dto)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Validar URL
            if (!Uri.TryCreate(dto.Url, UriKind.Absolute, out var uriResult) ||
                (uriResult.Scheme != Uri.UriSchemeHttps && uriResult.Scheme != Uri.UriSchemeHttp))
                return BadRequest("URL de imagen inválida");

            // Desactivar anterior
            var FotoPerfilsAnteriores = await _context.FotoPerfils
                .Where(a => a.UsuarioId == usuarioId)
                .ToListAsync();
            foreach (var anterior in FotoPerfilsAnteriores)
                anterior.EsPrincipal = false;

            // Crear nuevo
            var FotoPerfil = new FotoPerfil
            {
                UsuarioId = usuarioId,
                Url = dto.Url,
                FechaSubida = DateTime.UtcNow,
                EsPrincipal = true
            };
            _context.FotoPerfils.Add(FotoPerfil);
            await _context.SaveChangesAsync();

            // Update usuario
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            usuario.FotoPerfilId = FotoPerfil.Id;
            await _context.SaveChangesAsync();

            var FotoPerfilDto = new FotoPerfilDto
            {
                Id = FotoPerfil.Id,
                Url = FotoPerfil.Url,
                FechaSubida = FotoPerfil.FechaSubida,
                EsPrincipal = true
            };

            return Ok(FotoPerfilDto);
        }

        [HttpGet("{usuarioId}")]
        public async Task<ActionResult<FotoPerfilDto>> GetFotoPerfil(int usuarioId)
        {
            var FotoPerfil = await _context.FotoPerfils
                .Where(a => a.UsuarioId == usuarioId && a.EsPrincipal)
                .Select(a => new FotoPerfilDto
                {
                    Id = a.Id,
                    Url = a.Url,
                    FechaSubida = a.FechaSubida,
                    EsPrincipal = true
                })
                .FirstOrDefaultAsync();

            return FotoPerfil != null ? Ok(FotoPerfil) : NotFound();
        }

        [HttpGet("{usuarioId}/todos")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<FotoPerfilDto>>> GetFotoPerfilsUsuario(int usuarioId)
        {
            var authId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (authId != usuarioId) return Forbid();

            var FotoPerfils = await _context.FotoPerfils
                .Where(a => a.UsuarioId == usuarioId)
                .OrderByDescending(a => a.FechaSubida)
                .Select(a => new FotoPerfilDto
                {
                    Id = a.Id,
                    Url = a.Url,
                    FechaSubida = a.FechaSubida,
                    EsPrincipal = a.EsPrincipal
                })
                .ToListAsync();

            return Ok(FotoPerfils);
        }

        [HttpPatch("{FotoPerfilId}/principal")]
        [Authorize]
        public async Task<IActionResult> CambiarPrincipal(int FotoPerfilId)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var FotoPerfil = await _context.FotoPerfils
                .FirstOrDefaultAsync(a => a.Id == FotoPerfilId && a.UsuarioId == usuarioId);
            if (FotoPerfil == null) return NotFound();

            // Desactivar otros
            await _context.FotoPerfils
                .Where(a => a.UsuarioId == usuarioId && a.Id != FotoPerfilId)
                .ExecuteUpdateAsync(s => s.SetProperty(a => a.EsPrincipal, false));

            // Activar este
            FotoPerfil.EsPrincipal = true;
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            usuario.FotoPerfilId = FotoPerfilId;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{FotoPerfilId}")]
        [Authorize]
        public async Task<IActionResult> EliminarFotoPerfil(int FotoPerfilId)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var FotoPerfil = await _context.FotoPerfils
                .FirstOrDefaultAsync(a => a.Id == FotoPerfilId && a.UsuarioId == usuarioId);
            if (FotoPerfil == null) return NotFound();

            _context.FotoPerfils.Remove(FotoPerfil);

            // Si era principal, reset usuario
            if (FotoPerfil.EsPrincipal)
            {
                var usuario = await _context.Usuarios.FindAsync(usuarioId);
                usuario.FotoPerfilId = null;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
