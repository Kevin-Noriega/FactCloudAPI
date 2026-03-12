using FactCloudAPI.Data;
using FactCloudAPI.Models;
using FactCloudAPI.Models.Sesiones;
using Microsoft.AspNetCore.Http;

namespace FactCloudAPI.Services.Seguridad
{
    public class SeguridadService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SeguridadService(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task RegistrarSesion(int usuarioId, string accion, bool exitoso = true)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return;

            var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "Desconocida";
            var userAgent = httpContext.Request.Headers["User-Agent"].ToString();

            var sesion = new HistorialSesion
            {
                UsuarioId = usuarioId,
                FechaHora = DateTime.UtcNow,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                Navegador = "Detectado",
                SistemaOperativo = "Detectado",
                Dispositivo = "Detectado",
                Exitoso = exitoso,
                SesionActual = true
            };

            _context.HistorialSesiones.Add(sesion);
            await _context.SaveChangesAsync();
        }
    }
}
