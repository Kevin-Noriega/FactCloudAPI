using NubeeAPI.Models;

namespace NubeeAPI.Services.Notificaciones
{
    /// <summary>
    /// Crea, consulta y emite notificaciones in-app. Cada notificación se persiste
    /// y se envía en tiempo real al usuario destino por SignalR.
    /// </summary>
    public interface INotificacionService
    {
        Task<Notificacion> CrearAsync(
            int usuarioId,
            string tipo,
            string titulo,
            string mensaje,
            string? categoria = null,
            int? referenciaId = null,
            string? enlace = null,
            CancellationToken ct = default);

        Task<List<Notificacion>> ObtenerAsync(
            int usuarioId, bool soloNoLeidas, int page, int pageSize, CancellationToken ct = default);

        Task<int> ContarNoLeidasAsync(int usuarioId, CancellationToken ct = default);

        Task<bool> MarcarLeidaAsync(int id, int usuarioId, CancellationToken ct = default);

        Task<int> MarcarTodasLeidasAsync(int usuarioId, CancellationToken ct = default);

        Task<bool> EliminarAsync(int id, int usuarioId, CancellationToken ct = default);
    }
}
