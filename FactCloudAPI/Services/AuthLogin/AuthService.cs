using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Login;
using FactCloudAPI.Models;
using FactCloudAPI.Services.AuthLogin;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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

        var token = GenerarAccessToken(usuario);

        return (token, usuarioDto);
    }

    // MÉTODO 1: Generar Access Token (corta duración)
    public string GenerarAccessToken(Usuario usuario)
    {
        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, usuario.Correo),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // ID único del token
        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
        new Claim(ClaimTypes.Email, usuario.Correo),
        new Claim(ClaimTypes.Role, "Usuario") // O rol real desde tu tabla
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15), // ← CORTA DURACIÓN (15-30 min)
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // MÉTODO 2: Generar Refresh Token (string aleatorio seguro)
    public string GenerarRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }



}
