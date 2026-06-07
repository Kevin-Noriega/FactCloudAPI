using NubeeAPI.Data;
using NubeeAPI.DTOs.Login;
using NubeeAPI.Models;
using NubeeAPI.Services.AuthLogin;
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
    private readonly IAuthService _authService; // ? Inyectar el service

    public AuthController(ApplicationDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService; // ? Inyectar
    }
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDTO model)
    {
        Console.WriteLine("\n=== LOGIN REQUEST ===");
        Console.WriteLine($"Correo: {model.Correo}");

        // 1. Validar usuario
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Correo == model.Correo);

        if (usuario == null)
        {
            Console.WriteLine("✗ Usuario no encontrado");
            return Unauthorized(new { message = "Credenciales incorrectas" });
        }

        if (!BCrypt.Net.BCrypt.Verify(model.Contrasena, usuario.ContrasenaHash))
        {
            Console.WriteLine("✗ Contraseña incorrecta");
            return Unauthorized(new { message = "Credenciales incorrectas" });
        }

        Console.WriteLine($"✓ Usuario autenticado: {usuario.Correo}");

        // 2. Verificar estado
        if (!usuario.Estado)
        {
            Console.WriteLine("✗ Usuario desactivado");
            var diasRestantes = (int)(usuario.FechaDesactivacion!.Value.AddDays(30) - DateTime.Now).TotalDays;
            return StatusCode(423, new { diasRestantes, mensaje = "Reactivar cuenta" });
        }

        // 3. Buscar suscripciones activas y plan
        var suscripcionesActivas = await _context.SuscripcionesFacturacion
            .Include(s => s.PlanFacturacion)
            .Where(s => s.UsuarioId == usuario.Id && s.Activa)
            .OrderByDescending(s => s.FechaInicio)
            .ToListAsync();

        var ahora = DateTime.Now;

        // Suscripciones vigentes: sin fecha de fin (ilimitada) o con fecha de fin futura.
        var suscripcionesVigentes = suscripcionesActivas
            .Where(s => s.FechaFin == null || s.FechaFin > ahora)
            .ToList();

        // Acceso solo con una suscripción vigente. Se bloquea tanto al que se le venció
        // el plan como al que nunca tuvo uno. Los administradores quedan exentos.
        var esAdmin = string.Equals(usuario.Rol, "admin", StringComparison.OrdinalIgnoreCase);
        if (!esAdmin && !suscripcionesVigentes.Any())
        {
            var habiaSuscripciones = suscripcionesActivas.Any();
            var fechaExpiracion = habiaSuscripciones ? suscripcionesActivas.Max(s => s.FechaFin) : null;
            Console.WriteLine(habiaSuscripciones
                ? $"✗ Plan vencido (venció: {fechaExpiracion})"
                : "✗ Sin plan activo");
            return StatusCode(402, new
            {
                codigo = habiaSuscripciones ? "PLAN_VENCIDO" : "SIN_PLAN",
                fechaExpiracion,
                message = habiaSuscripciones
                    ? "Tu plan ha vencido. Renueva tu suscripción para continuar."
                    : "No tienes un plan activo. Adquiere un plan para continuar."
            });
        }

        // Plan principal: la suscripción de facturación (el POS es un add-on aparte).
        var suscripcion = suscripcionesVigentes
            .FirstOrDefault(s => s.PlanFacturacion != null && s.PlanFacturacion.Tipo != "POS")
            ?? suscripcionesVigentes.FirstOrDefault();

        var plan = suscripcion?.PlanFacturacion;

        // POS: el usuario tiene acceso si contrató cualquier módulo/plan con POS vigente.
        var tienePos = suscripcionesVigentes
            .Any(s => s.PlanFacturacion != null && s.PlanFacturacion.IncluyePOS);

        // 4. Generar DTO de respuesta
        var usuarioDto = new UsuarioLoginDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Correo = usuario.Correo,
            Estado = usuario.Estado,
            Rol = usuario.Rol ?? "usuario",
            FechaDesactivacion = usuario.FechaDesactivacion,

            SuscripcionId = suscripcion?.Id ?? 0,
            PlanNombre = plan?.Nombre ?? "Demo",
            // Aquí puedes poner tu cálculo real de documentos restantes si ya lo tienes
            DocumentosRestantes = 0,
            FechaExpiracion = suscripcion?.FechaFin,
            TienePos = tienePos,

            Plan = plan == null
                ? null
                : new PlanLoginDto
                {
                    Id = plan.Id,
                    Codigo = plan.Codigo,
                    Nombre = plan.Nombre,
                    IncluyePOS = plan.IncluyePOS
                }
        };

        // 5. Generar access token
        var accessToken = _authService.GenerarAccessToken(usuario);
        var jwtId = new JwtSecurityTokenHandler()
            .ReadJwtToken(accessToken)
            .Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

        Console.WriteLine($"✓ Access token generado - JTI: {jwtId}");

        // 6. Generar refresh token
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

        Console.WriteLine($"✓ Refresh token guardado en BD: {refreshToken[..30]}...");
        Console.WriteLine($"   Expira: {refreshTokenEntity.FechaExpiracion}");

        // 7. Enviar refresh token como cookie HttpOnly
        Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            Path = "/"
        });

        Console.WriteLine("✓ Cookie enviada al cliente");
        Console.WriteLine("=== LOGIN EXITOSO ===\n");

        return Ok(new { token = accessToken, usuario = usuarioDto });
    }



    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken()
    {
        Console.WriteLine("\n=== REFRESH TOKEN REQUEST ===");
        Console.WriteLine($"Request Path: {Request.Path}");
        Console.WriteLine($"Request Origin: {Request.Headers["Origin"]}");
        Console.WriteLine($"Cookies count: {Request.Cookies.Count}");

        foreach (var cookie in Request.Cookies)
        {
            var preview = cookie.Value.Length > 30 ? cookie.Value.Substring(0, 30) + "..." : cookie.Value;
            Console.WriteLine($"  Cookie: {cookie.Key} = {preview}");
        }

        if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            Console.WriteLine("? Cookie 'refreshToken' NO encontrada");
            return Unauthorized(new { message = "Refresh token no encontrado" });
        }

        Console.WriteLine($"? refreshToken encontrado: {refreshToken.Substring(0, 30)}...");

        var storedToken = await _context.RefreshTokens
            .Include(rt => rt.Usuario)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (storedToken == null)
        {
            Console.WriteLine("? Token NO existe en base de datos");

            // Debug: Mostrar �ltimos tokens en BD
            var recentTokens = await _context.RefreshTokens
                .OrderByDescending(t => t.FechaCreacion)
                .Take(3)
                .Select(t => new { t.Token, t.UsuarioId, t.Usado, t.FechaCreacion })
                .ToListAsync();

            Console.WriteLine("�ltimos 3 tokens en BD:");
            foreach (var t in recentTokens)
            {
                var preview = t.Token.Substring(0, 30) + "...";
                Console.WriteLine($"  - {preview} Usuario:{t.UsuarioId} Usado:{t.Usado} Creado:{t.FechaCreacion}");
            }

            return Unauthorized(new { message = "Refresh token inv�lido" });
        }

        Console.WriteLine($"? Token encontrado en BD - Usuario: {storedToken.Usuario.Correo}");

        if (storedToken.Usado)
        {
            Console.WriteLine("? Token ya fue usado");
            return Unauthorized(new { message = "Refresh token ya fue usado" });
        }

        if (storedToken.Revocado)
        {
            Console.WriteLine("? Token revocado");
            return Unauthorized(new { message = "Refresh token revocado" });
        }

        if (storedToken.FechaExpiracion < DateTime.UtcNow)
        {
            Console.WriteLine($"? Token expirado");
            Console.WriteLine($"   Expir�: {storedToken.FechaExpiracion}");
            Console.WriteLine($"   Ahora: {DateTime.UtcNow}");
            return Unauthorized(new { message = "Refresh token expirado" });
        }

        if (!storedToken.Usuario.Estado)
        {
            Console.WriteLine("? Usuario desactivado");
            return Unauthorized(new { message = "Usuario desactivado" });
        }

        // Plan vencido: mismas reglas que en /login. Si el usuario tuvo suscripciones
        // activas pero todas expiraron, se corta la sesión (no se renueva el token).
        var suscripcionesRefresh = await _context.SuscripcionesFacturacion
            .Where(s => s.UsuarioId == storedToken.UsuarioId && s.Activa)
            .ToListAsync();

        var ahoraRefresh = DateTime.Now;
        var tieneVigenteRefresh = suscripcionesRefresh
            .Any(s => s.FechaFin == null || s.FechaFin > ahoraRefresh);

        var esAdminRefresh = string.Equals(storedToken.Usuario.Rol, "admin", StringComparison.OrdinalIgnoreCase);
        if (!esAdminRefresh && !tieneVigenteRefresh)
        {
            var habiaSuscripciones = suscripcionesRefresh.Any();
            var fechaExpiracion = habiaSuscripciones ? suscripcionesRefresh.Max(s => s.FechaFin) : null;
            Console.WriteLine(habiaSuscripciones
                ? $"✗ Plan vencido en refresh (venció: {fechaExpiracion})"
                : "✗ Sin plan activo en refresh");
            return StatusCode(402, new
            {
                codigo = habiaSuscripciones ? "PLAN_VENCIDO" : "SIN_PLAN",
                fechaExpiracion,
                message = habiaSuscripciones
                    ? "Tu plan ha vencido. Renueva tu suscripción para continuar."
                    : "No tienes un plan activo. Adquiere un plan para continuar."
            });
        }

        Console.WriteLine("? Todas las validaciones OK - Generando nuevos tokens");

        // Marcar token actual como usado
        storedToken.Usado = true;
        _context.RefreshTokens.Update(storedToken);

        // Generar nuevo access token
        var nuevoAccessToken = _authService.GenerarAccessToken(storedToken.Usuario);
        var nuevoJwtId = new JwtSecurityTokenHandler()
            .ReadJwtToken(nuevoAccessToken)
            .Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

        // Generar nuevo refresh token
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

        Console.WriteLine($"? Nuevo token guardado en BD: {nuevoRefreshToken.Substring(0, 30)}...");

        // Enviar cookie con el nuevo refresh token.
        // ⚠️ Debe coincidir con la cookie de /login (Secure + SameSite=None) para que
        //    el navegador la reenvíe en peticiones cross-site (front:5173 → api:7149).
        Response.Cookies.Append("refreshToken", nuevoRefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            Path = "/"
        });

        Console.WriteLine("? Refresh completado exitosamente\n");

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

        return Ok(new { message = "Sesi�n cerrada correctamente" });
    }
}

public class LoginDTO
{
    public string Correo { get; set; }
    public string Contrasena { get; set; }
}
