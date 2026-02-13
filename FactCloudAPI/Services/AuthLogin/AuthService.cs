using FactCloudAPI.Data;
using FactCloudAPI.Models;
using FactCloudAPI.DTOs.Login;
using FactCloudAPI.Services.AuthLogin;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<(string token, UsuarioLoginDto usuario)> LoginAsync(LoginDTO model)
    {
        var usuario = await _context.Usuarios
        .Include(u => u.Suscripciones) 
        .ThenInclude(s => s.PlanFacturacion) 
        .FirstOrDefaultAsync(u => u.Correo == model.Correo);
        if (usuario == null)
            throw new UnauthorizedAccessException("Correo no encontrado");

        if (!BCrypt.Net.BCrypt.Verify(model.Contrasena, usuario.ContrasenaHash))
            throw new UnauthorizedAccessException("Credenciales incorrectas");

        if (!usuario.Estado)
        {
            var diasRestantes = (int)(usuario.FechaDesactivacion.Value.AddDays(30) - DateTime.Now).TotalDays;
            throw new InvalidOperationException($"Cuenta suspendida. {diasRestantes} días restantes.");
        }
        var suscripcionActiva = usuario.Suscripciones
       ?.FirstOrDefault(s => s.Activa && s.FechaFin > DateTime.Now);
        

        var usuarioDto = new UsuarioLoginDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Correo = usuario.Correo,
            Estado = usuario.Estado,
            SuscripcionId = suscripcionActiva?.PlanFacturacionId ?? 0,
            PlanNombre = suscripcionActiva?.PlanFacturacion.Nombre ?? "", 
            DocumentosRestantes = suscripcionActiva?.DocumentosUsados ?? 0,
            FechaExpiracion = suscripcionActiva?.FechaFin,
            FechaDesactivacion = usuario.FechaDesactivacion
        };

        var token = GenerarToken(usuario);

        return (token, usuarioDto);
    }

    private string GenerarToken(Usuario usuario)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"])
        );

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
