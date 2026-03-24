using Microsoft.AspNetCore.Mvc;

namespace FactCloudAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificacionesController : ControllerBase
    {
        // GET: api/notificaciones
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new List<string>());
        }

        // GET: api/notificaciones/NoLeidas
        [HttpGet("NoLeidas")]
        public IActionResult NoLeidas()
        {
            return Ok(new { total = 0 });
        }
    }
}
