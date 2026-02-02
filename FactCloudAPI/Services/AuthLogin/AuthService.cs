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
            .FirstOrDefaultAsync(u => u.Correo == model.Correo);

        if (usuario == null || usuario.ContrasenaHash != model.Contrasena)
            throw new UnauthorizedAccessException("Credenciales incorrectas");

        if (!usuario.Estado)
        {
            var diasRestantes = (int)(usuario.FechaDesactivacion.Value.AddDays(30) - DateTime.Now).TotalDays;
            throw new InvalidOperationException(diasRestantes.ToString());
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
