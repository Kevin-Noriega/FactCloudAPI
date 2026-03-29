using FactCloudAPI.DTOs.Clientes;

namespace FactCloudAPI.Services.Clientes
{
    public interface IClienteService
    {
        Task<List<ClienteDetalleDto>> ObtenerClientesAsync(int usuarioId);
        Task<ClienteDetalleDto?> ObtenerPorIdAsync(int id, int usuarioId);
        Task CrearAsync(ClienteCreateDto dto, int usuarioId);
        Task ActualizarAsync(int id, ClienteCreateDto dto, int usuarioId);
        Task DesactivarAsync(int id, int usuarioId);
        Task<bool> ActualizarParcialAsync(int id, ClienteUpdateDto dto, int usuarioId);
        Task<bool> ActivarAsync(int id, int usuarioId);

    }
}
