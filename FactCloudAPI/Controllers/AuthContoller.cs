using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Login;
using FactCloudAPI.Models;
using FactCloudAPI.Services.Seguridad;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;
    private readonly SeguridadService _seguridadService;

    public AuthController(ApplicationDbContext context, IConfiguration config, SeguridadService seguridadService)
    {
        _context = context;
        _config = config;
        _seguridadService = seguridadService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO model)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Correo == model.Correo);

        if (usuario == null || usuario.ContrasenaHash != model.Contrasena)
            return Unauthorized("Credenciales incorrectas");

        // 3. VERIFICAR ESTADO (con Entity)
        if (!usuario.Estado)
        {
            var diasRestantes = (int)(usuario.FechaDesactivacion.Value.AddDays(30) - DateTime.Now).TotalDays;
            return StatusCode(423, new { diasRestantes, mensaje = "Reactivar cuenta" });
        }

        var usuarioDto = new UsuarioLoginDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Correo = usuario.Correo,
            Estado = usuario.Estado,
            FechaDesactivacion = usuario.FechaDesactivacion
        };

        var token = GenerarToken(usuario);
        await _seguridadService.RegistrarSesion(usuario.Id,"Login", true);
        return Ok(new { token, usuario = usuarioDto });
    }

    private string GenerarToken(Usuario usuario)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
             new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Sub, usuario.Correo),
             new Claim(ClaimTypes.Email, usuario.Correo),
            new Claim("role", "Usuario")
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class LoginDTO
{
    public string Correo { get; set; }
    public string Contrasena { get; set; }
}