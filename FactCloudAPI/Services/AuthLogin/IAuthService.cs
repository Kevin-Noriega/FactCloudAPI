using FactCloudAPI.DTOs.Login;
using FactCloudAPI.Models;
namespace FactCloudAPI.Services.AuthLogin
{
    public interface IAuthService
    {
        Task<(string token, UsuarioLoginDto usuario)> LoginAsync(LoginDTO model);
        string GenerarAccessToken(Usuario usuario); // ← Agregar
        string GenerarRefreshToken();                // ← Agregar
    }

}
