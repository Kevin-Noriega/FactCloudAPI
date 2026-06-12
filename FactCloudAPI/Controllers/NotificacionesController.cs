using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NubeeAPI.Services.Notificaciones;
using System.Security.Claims;

namespace NubeeAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificacionesController : ControllerBase
    {
        private readonly INotificacionService _service;

        public NotificacionesController(INotificacionService service)
        {
            _service = service;
        }

        private int UsuarioId =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        // GET: api/notificaciones?soloNoLeidas=false&page=1&pageSize=20
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] bool soloNoLeidas = false,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken ct = default)
        {
            var data = await _service.ObtenerAsync(UsuarioId, soloNoLeidas, page, pageSize, ct);
            var noLeidas = await _service.ContarNoLeidasAsync(UsuarioId, ct);
            return Ok(new { data, noLeidas, page, pageSize });
        }

        // GET: api/notificaciones/NoLeidas
        [HttpGet("NoLeidas")]
        public async Task<IActionResult> NoLeidas(CancellationToken ct = default)
        {
            var total = await _service.ContarNoLeidasAsync(UsuarioId, ct);
            return Ok(new { total });
        }

        // PUT: api/notificaciones/5/leer
        [HttpPut("{id:int}/leer")]
        public async Task<IActionResult> MarcarLeida(int id, CancellationToken ct = default)
        {
            var ok = await _service.MarcarLeidaAsync(id, UsuarioId, ct);
            return ok ? Ok(new { ok = true }) : NotFound(new { message = "Notificación no encontrada" });
        }

        // PUT: api/notificaciones/leer-todas
        [HttpPut("leer-todas")]
        public async Task<IActionResult> MarcarTodasLeidas(CancellationToken ct = default)
        {
            var actualizadas = await _service.MarcarTodasLeidasAsync(UsuarioId, ct);
            return Ok(new { actualizadas });
        }

        // DELETE: api/notificaciones/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id, CancellationToken ct = default)
        {
            var ok = await _service.EliminarAsync(id, UsuarioId, ct);
            return ok ? Ok(new { ok = true }) : NotFound(new { message = "Notificación no encontrada" });
        }
    }
}
