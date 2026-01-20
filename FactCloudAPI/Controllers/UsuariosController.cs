using FactCloudAPI.Data;
using FactCloudAPI.DTOs;
using FactCloudAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FactCloudAPI.DTOs.Usuarios;
using Microsoft.EntityFrameworkCore;

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
    //comentario de prueba
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
            Correo = dto.Correo,
            FechaRegistro = DateTime.UtcNow,
            Estado = true
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id });
    }

    // PATCH: api/Usuarios/5
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchUsuario(int id, UpdateUsuarioDto dto)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return NotFound();

        if (dto.Nombre != null) usuario.Nombre = dto.Nombre;
        if (dto.Apellido != null) usuario.Apellido = dto.Apellido;
        if (dto.Telefono != null) usuario.Telefono = dto.Telefono;
        if (dto.NombreNegocio != null) usuario.NombreNegocio = dto.NombreNegocio;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Usuarios/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return NotFound();

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
