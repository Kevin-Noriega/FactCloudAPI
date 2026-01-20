using FactCloudAPI.Data;
using FactCloudAPI.Models;
using FactCloudAPI.DTOs.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace FactCloudAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioListDto>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Select(u => new UsuarioListDto
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Correo = u.Correo,
                    Estado = u.Estado
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDetalleDto>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios
                .Where(u => u.Id == id)
                .Select(u => new UsuarioDetalleDto
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Correo = u.Correo,
                    Telefono = u.Telefono,
                    NombreNegocio = u.NombreNegocio
                })
                .FirstOrDefaultAsync();

            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        // GET: api/Usuarios/me
        [HttpGet("me")]
        public async Task<ActionResult<UsuarioEmpresaDto>> GetMe()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var usuario = await _context.Usuarios
                .Where(u => u.Id == userId)
                .Select(u => new UsuarioEmpresaDto
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    NombreNegocio = u.NombreNegocio,
                    LogoNegocio = u.LogoNegocio,
                    NitNegocio = u.NitNegocio,
                    DireccionNegocio = u.DireccionNegocio
                })
                .FirstOrDefaultAsync();

            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        // POST: api/Usuarios
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> PostUsuario(CreateUsuarioDto dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Correo == dto.Correo))
                return BadRequest(new { mensaje = "El correo ya está registrado" });

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Correo = dto.Correo,
                Telefono = dto.Telefono,
                FechaRegistro = DateTime.UtcNow,
                Estado = true
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

