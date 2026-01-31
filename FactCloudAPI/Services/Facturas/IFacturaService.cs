using FactCloudAPI.DTOs.Facturas;

namespace FactCloudAPI.Services.Facturas
{
    public interface IFacturaService
    {
        Task<List<FacturaDto>> ObtenerAsync(int usuarioId);
        Task<FacturaDetalleDTO?> ObtenerPorIdAsync(int id, int usuarioId);
        Task<int> CrearAsync(FacturaCreateDto dto, int usuarioId);
        Task RegistrarPagoAsync(int id, FacturaPagoDto dto, int usuarioId);
        Task EnviarClienteAsync(int id, int usuarioId);
        Task EnviarDIANAsync(int id, int usuarioId);
    }

}
