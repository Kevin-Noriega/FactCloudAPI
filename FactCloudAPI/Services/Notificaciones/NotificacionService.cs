using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NubeeAPI.Data;
using NubeeAPI.Models;

namespace NubeeAPI.Services.Notificaciones
{
    /// <summary>
    /// Implementación de <see cref="INotificacionService"/>. Persiste la notificación
    /// en BD y la empuja por SignalR al grupo del usuario destino. La emisión en
    /// tiempo real nunca debe tumbar la operación de negocio: si SignalR falla, se
    /// loguea y se continúa (la notificación queda guardada de todos modos).
    /// </summary>
    public class NotificacionService : INotificacionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificacionesHub> _hub;
        private readonly ILogger<NotificacionService> _logger;

        public NotificacionService(
            ApplicationDbContext context,
            IHubContext<NotificacionesHub> hub,
            ILogger<NotificacionService> logger)
        {
            _context = context;
            _hub = hub;
            _logger = logger;
        }

        public async Task<Notificacion> CrearAsync(
            int usuarioId,
            string tipo,
            string titulo,
            string mensaje,
            string? categoria = null,
            int? referenciaId = null,
            string? enlace = null,
            CancellationToken ct = default)
        {
            var noti = new Notificacion
            {
                UsuarioId = usuarioId,
                Tipo = NormalizarTipo(tipo),
                Titulo = Recortar(titulo, 200),
                Mensaje = Recortar(mensaje, 500),
                Categoria = categoria,
                ReferenciaId = referenciaId,
                Enlace = enlace,
                Leida = false,
                FechaCreacion = DateTime.UtcNow,
            };

            _context.Notificaciones.Add(noti);
            await _context.SaveChangesAsync(ct);

            // Emisión en tiempo real (best-effort).
            try
            {
                var noLeidas = await ContarNoLeidasAsync(usuarioId, ct);
                await _hub.Clients.User(usuarioId.ToString()).SendAsync("nueva", new
                {
                    id = noti.Id,
                    tipo = noti.Tipo,
                    categoria = noti.Categoria,
                    titulo = noti.Titulo,
                    mensaje = noti.Mensaje,
                    referenciaId = noti.ReferenciaId,
                    enlace = noti.Enlace,
                    leida = noti.Leida,
                    fechaCreacion = noti.FechaCreacion,
                    noLeidas,
                }, ct);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,
                    "No se pudo emitir la notificación {Id} por SignalR al usuario {UsuarioId}",
                    noti.Id, usuarioId);
            }

            return noti;
        }

        public async Task<List<Notificacion>> ObtenerAsync(
            int usuarioId, bool soloNoLeidas, int page, int pageSize, CancellationToken ct = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 20;

            var query = _context.Notificaciones
                .Where(n => n.UsuarioId == usuarioId);

            if (soloNoLeidas)
                query = query.Where(n => !n.Leida);

            return await query
                .OrderByDescending(n => n.FechaCreacion)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public Task<int> ContarNoLeidasAsync(int usuarioId, CancellationToken ct = default) =>
            _context.Notificaciones.CountAsync(n => n.UsuarioId == usuarioId && !n.Leida, ct);

        public async Task<bool> MarcarLeidaAsync(int id, int usuarioId, CancellationToken ct = default)
        {
            var noti = await _context.Notificaciones
                .FirstOrDefaultAsync(n => n.Id == id && n.UsuarioId == usuarioId, ct);
            if (noti == null) return false;
            if (!noti.Leida)
            {
                noti.Leida = true;
                await _context.SaveChangesAsync(ct);
            }
            return true;
        }

        public async Task<int> MarcarTodasLeidasAsync(int usuarioId, CancellationToken ct = default)
        {
            var pendientes = await _context.Notificaciones
                .Where(n => n.UsuarioId == usuarioId && !n.Leida)
                .ToListAsync(ct);
            foreach (var n in pendientes) n.Leida = true;
            if (pendientes.Count > 0)
                await _context.SaveChangesAsync(ct);
            return pendientes.Count;
        }

        public async Task<bool> EliminarAsync(int id, int usuarioId, CancellationToken ct = default)
        {
            var noti = await _context.Notificaciones
                .FirstOrDefaultAsync(n => n.Id == id && n.UsuarioId == usuarioId, ct);
            if (noti == null) return false;
            _context.Notificaciones.Remove(noti);
            await _context.SaveChangesAsync(ct);
            return true;
        }

        private static string NormalizarTipo(string? tipo) => tipo?.ToLowerInvariant() switch
        {
            "success" or "warning" or "info" or "error" => tipo!.ToLowerInvariant(),
            _ => "info",
        };

        private static string Recortar(string? texto, int max)
        {
            texto ??= string.Empty;
            return texto.Length <= max ? texto : texto.Substring(0, max);
        }
    }
}
