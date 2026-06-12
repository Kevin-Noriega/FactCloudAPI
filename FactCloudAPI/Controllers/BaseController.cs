using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace NubeeAPI.Controllers
{
    /// <summary>
    /// Controlador base que centraliza la extracción del usuarioId desde el JWT.
    /// Todos los controllers que requieran tenant isolation deben heredar de este.
    /// </summary>
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected int? ObtenerUsuarioId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? User.FindFirst("sub")?.Value;
            return int.TryParse(claim, out var id) ? id : null;
        }

        protected IActionResult UsuarioNoAutenticado() =>
            Unauthorized(new { mensaje = "No autenticado.", codigo = "UNAUTHORIZED" });
    }
}
