using NubeeAPI.DTOs.Usuarios;
using NubeeAPI.Models;
namespace NubeeAPI.Services.Usuarios
{
    public interface IUsuarioService
    {
        Task<List<UsuarioListDto>> GetAllAsync();
        Task<UsuarioDetalleDto> GetByIdAsync(int id);
        Task<UsuarioDetalleDto> GetMeAsync(int userId);
        Task<int> CreateAsync(CreateUsuarioDto dto);
        Task UpdateAsync(int id, UpdateUsuarioDto dto);
        Task DeleteAsync(int id);
        Task<(Usuario usuario, string token)> CrearYActivarAsync(CrearYActivarDto dto);

    }
}
