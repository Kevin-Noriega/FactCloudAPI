using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Login;
using FactCloudAPI.Models;
using FactCloudAPI.Services.AuthLogin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IAuthService _authService; // ← Inyectar el service

    public AuthController(ApplicationDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService; // ← Inyectar
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDTO model)
    {
        // 1. Validar usuario
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Correo == model.Correo);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(model.Contrasena, usuario.ContrasenaHash))
            return Unauthorized(new { message = "Credenciales incorrectas" });
        // 2. Verificar estado
        if (!usuario.Estado)
        {
            var diasRestantes = (int)(usuario.FechaDesactivacion.Value.AddDays(30) - DateTime.Now).TotalDays;
            return StatusCode(423, new { diasRestantes, mensaje = "Reactivar cuenta" });
        }

        // 3. Generar DTO de respuesta
        var usuarioDto = new UsuarioLoginDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Correo = usuario.Correo,
            Estado = usuario.Estado,
            FechaDesactivacion = usuario.FechaDesactivacion
        };

        // 4. ✅ Usar el servicio (NO duplicar código)
        var accessToken = _authService.GenerarAccessToken(usuario);
        var jwtId = new JwtSecurityTokenHandler()
            .ReadJwtToken(accessToken)
            .Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

        // 5. ✅ Usar el servicio
        var refreshToken = _authService.GenerarRefreshToken();
        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            JwtId = jwtId,
            UsuarioId = usuario.Id,
            FechaCreacion = DateTime.UtcNow,
            FechaExpiracion = DateTime.UtcNow.AddDays(7)
        };

        _context.RefreshTokens.Add(refreshTokenEntity);
        await _context.SaveChangesAsync();

        // 6. Enviar refresh token como cookie HttpOnly
        Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });

        return Ok(new { token = accessToken, usuario = usuarioDto });
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken()
    {
        if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            return Unauthorized(new { message = "Refresh token no encontrado" });

        var storedToken = await _context.RefreshTokens
            .Include(rt => rt.Usuario)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (storedToken == null)
            return Unauthorized(new { message = "Refresh token inválido" });

        if (storedToken.Usado)
            return Unauthorized(new { message = "Refresh token ya fue usado" });

        if (storedToken.Revocado)
            return Unauthorized(new { message = "Refresh token revocado" });

        if (storedToken.FechaExpiracion < DateTime.UtcNow)
            return Unauthorized(new { message = "Refresh token expirado" });

        if (!storedToken.Usuario.Estado)
            return Unauthorized(new { message = "Usuario desactivado" });

        storedToken.Usado = true;
        _context.RefreshTokens.Update(storedToken);

        // ✅ Usar el servicio
        var nuevoAccessToken = _authService.GenerarAccessToken(storedToken.Usuario);
        var nuevoJwtId = new JwtSecurityTokenHandler()
            .ReadJwtToken(nuevoAccessToken)
            .Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

        // ✅ Usar el servicio
        var nuevoRefreshToken = _authService.GenerarRefreshToken();
        var nuevoRefreshTokenEntity = new RefreshToken
        {
            Token = nuevoRefreshToken,
            JwtId = nuevoJwtId,
            UsuarioId = storedToken.UsuarioId,
            FechaCreacion = DateTime.UtcNow,
            FechaExpiracion = DateTime.UtcNow.AddDays(7)
        };

        _context.RefreshTokens.Add(nuevoRefreshTokenEntity);
        await _context.SaveChangesAsync();

        Response.Cookies.Append("refreshToken", nuevoRefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });

        return Ok(new { token = nuevoAccessToken });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        if (userId == 0)
            return Unauthorized();

        var tokens = await _context.RefreshTokens
            .Where(rt => rt.UsuarioId == userId && !rt.Revocado)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.Revocado = true;
        }

        await _context.SaveChangesAsync();
        Response.Cookies.Delete("refreshToken");

        return Ok(new { message = "Sesión cerrada correctamente" });
    }
}

public class LoginDTO
{
    public string Correo { get; set; }
    public string Contrasena { get; set; }
}
