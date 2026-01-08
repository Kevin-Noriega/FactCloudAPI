namespace FactCloudAPI.Services
{
    public interface IEmailService
    {
        Task<bool> EnviarFacturaCliente(int facturaId);
    }
}
