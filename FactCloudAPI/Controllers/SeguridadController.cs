using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Usuarios;
using FactCloudAPI.Models.Sesiones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Security.Claims;

namespace FactCloudAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeguridadController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SeguridadController(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // CAMBIAR CONTRASEÑA
        [HttpPost("cambiar-contraseña")]
        public async Task<ActionResult> CambiarContraseña([FromBody] CambiarContraseñaDto dto)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var usuario = await _context.Usuarios.FindAsync(usuarioId);

                if (usuario == null)
                    return NotFound(new { error = "Usuario no encontrado" });

                // Verificar contraseña actual
                if (!BCrypt.Net.BCrypt.Verify(dto.ContraseñaActual, usuario.ContrasenaHash))
                    return BadRequest(new { error = "La contraseña actual es incorrecta" });

                // Validar nueva contraseña
                if (dto.NuevaContraseña != dto.ConfirmarContraseña)
                    return BadRequest(new { error = "Las contraseñas no coinciden" });

                if (dto.NuevaContraseña.Length < 6)
                    return BadRequest(new { error = "La contraseña debe tener al menos 6 caracteres" });

                // Verificar que no sea igual a la actual
                if (BCrypt.Net.BCrypt.Verify(dto.NuevaContraseña, usuario.ContrasenaHash))
                    return BadRequest(new { error = "La nueva contraseña debe ser diferente a la actual" });

                // Actualizar contraseña
                usuario.ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(dto.NuevaContraseña);
                await _context.SaveChangesAsync();

                // Registrar cambio en historial
                await RegistrarSesion(usuarioId, "Cambio de contraseña", true);

                return Ok(new { mensaje = "Contraseña actualizada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // OBTENER HISTORIAL DE SESIONES
        [HttpGet("historial-sesiones")]
        public async Task<ActionResult<List<HistorialSesionDto>>> GetHistorialSesiones(
            [FromQuery] int pagina = 1,
            [FromQuery] int limite = 20)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var total = await _context.HistorialSesiones
                    .Where(h => h.UsuarioId == usuarioId)
                    .CountAsync();

                var sesiones = await _context.HistorialSesiones
                    .Where(h => h.UsuarioId == usuarioId)
                    .OrderByDescending(h => h.FechaHora)
                    .Skip((pagina - 1) * limite)
                    .Take(limite)
                    .Select(h => new HistorialSesionDto
                    {
                        Id = h.Id,
                        FechaHora = h.FechaHora,
                        IpAddress = h.IpAddress,
                        Navegador = h.Navegador,
                        SistemaOperativo = h.SistemaOperativo,
                        Dispositivo = h.Dispositivo,
                        Ciudad = h.Ciudad,
                        Pais = h.Pais,
                        Exitoso = h.Exitoso,
                        SesionActual = h.SesionActual
                    })
                    .ToListAsync();

                return Ok(new
                {
                    sesiones,
                    total,
                    pagina,
                    totalPaginas = (int)Math.Ceiling(total / (double)limite)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        //  CERRAR SESIÓN ESPECÍFICA
        [HttpDelete("sesiones/{sesionId}")]
        public async Task<ActionResult> CerrarSesion(int sesionId)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var sesion = await _context.HistorialSesiones
                    .FirstOrDefaultAsync(h => h.Id == sesionId && h.UsuarioId == usuarioId);

                if (sesion == null)
                    return NotFound(new { error = "Sesión no encontrada" });

                if (sesion.SesionActual)
                    return BadRequest(new { error = "No puedes cerrar la sesión actual desde aquí" });

                _context.HistorialSesiones.Remove(sesion);
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = "Sesión cerrada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // CERRAR TODAS LAS SESIONES (excepto actual)
        [HttpPost("cerrar-todas-sesiones")]
        public async Task<ActionResult> CerrarTodasSesiones()
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var sesionesAEliminar = await _context.HistorialSesiones
                    .Where(h => h.UsuarioId == usuarioId && !h.SesionActual)
                    .ToListAsync();

                _context.HistorialSesiones.RemoveRange(sesionesAEliminar);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    mensaje = $"{sesionesAEliminar.Count} sesiones cerradas correctamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // Registrar sesión
        private async Task RegistrarSesion(int usuarioId, string accion = "Login", bool exitoso = true)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return;

            var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "Desconocida";
            var userAgent = httpContext.Request.Headers["User-Agent"].ToString();

            // Parse User Agent
            var uaParser = UAParser.Parser.GetDefault();
            var clientInfo = uaParser.Parse(userAgent);

            var sesion = new HistorialSesion
            {
                UsuarioId = usuarioId,
                FechaHora = DateTime.UtcNow,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                Navegador = $"{clientInfo.UA.Family} {clientInfo.UA.Major}",
                SistemaOperativo = $"{clientInfo.OS.Family} {clientInfo.OS.Major}",
                Dispositivo = clientInfo.Device.Family,
                Exitoso = exitoso,
                SesionActual = accion == "Login"
            };

            _context.HistorialSesiones.Add(sesion);
            await _context.SaveChangesAsync();
        }
      

    }

}

