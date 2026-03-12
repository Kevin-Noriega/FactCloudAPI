using FactCloudAPI.DTOs.Productos;

namespace FactCloudAPI.Services.Productos
{

    public interface IProductoService
    {
        Task<List<ProductoDto>> ObtenerAsync(int usuarioId);
        Task<ProductoDetalleDto?> ObtenerPorIdAsync(int id, int usuarioId);
        Task CrearAsync(ProductoCreateDto dto, int usuarioId);
        Task ActualizarAsync(int id, ProductoUpdateDto dto, int usuarioId);
        Task DesactivarAsync(int id, int usuarioId);
    }
}
