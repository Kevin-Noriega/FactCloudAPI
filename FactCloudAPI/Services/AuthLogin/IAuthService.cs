using NubeeAPI.DTOs.Login;
using NubeeAPI.Models;
namespace NubeeAPI.Services.AuthLogin
{
    public interface IAuthService
    {
        Task<(string token, UsuarioLoginDto usuario)> LoginAsync(LoginDTO model);
        string GenerarAccessToken(Usuario usuario); // ? Agregar
        string GenerarRefreshToken();                // ? Agregar
    }

}
