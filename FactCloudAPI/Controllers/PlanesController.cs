using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Planes;
using FactCloudAPI.Models.Planes;
using FactCloudAPI.Models.Suscripciones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FactCloudAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlanesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlanesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/planes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetPlanes()
        {
            var planes = await _context.PlanesFacturacion
                .Include(p => p.Features)
                .Where(p => p.Activo)
                .OrderBy(p => p.Id)
                .Select(p => new
                {
                    id = p.Id,
                    nombre = p.Nombre,
                    descripcion = p.Descripcion,
                    precioMensual = p.PrecioMensualFinal,
                    precioAnual = p.PrecioAnualFinal,
                    limiteDocumentosAnuales = p.LimiteDocumentosAnuales,
                    limiteUsuarios = p.LimiteUsuarios,
                    destacado = p.Destacado,
                    descuentoActivo = p.DescuentoActivo,
                    descuentoPorcentaje = p.DescuentoPorcentaje,
                    caracteristicas = p.Features.Select(f => f.Texto).ToList()
                })
                .ToListAsync();

            return Ok(planes);
        }

        // GET: api/planes/actual
        [HttpGet("actual")]
        public async Task<ActionResult<object>> GetPlanActual()
        {
            var userId = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var usuario = await _context.Usuarios
                .Include(u => u.SuscripcionActual)
                    .ThenInclude(s => s.PlanFacturacion)
                        .ThenInclude(p => p.Features)
                .FirstOrDefaultAsync(u => u.Id == int.Parse(userId));

            if (usuario == null)
                return NotFound("Usuario no encontrado");

            var suscripcionActual = usuario.SuscripcionActual;
            if (suscripcionActual == null)
                return NotFound("No se encontró suscripción activa");

            var plan = suscripcionActual.PlanFacturacion;

            return Ok(new
            {
                id = plan.Id,
                nombre = plan.Nombre,
                descripcion = plan.Descripcion,
                precioMensual = plan.PrecioMensualFinal,
                precioAnual = plan.PrecioAnualFinal,
                limiteDocumentosAnuales = plan.LimiteDocumentosAnuales,
                limiteUsuarios = plan.LimiteUsuarios,
                caracteristicas = plan.Features.Select(f => f.Texto).ToList()
            });
        }

        // GET: api/planes/estadisticas
        [HttpGet("estadisticas")]
        public async Task<ActionResult<object>> GetEstadisticas()
        {
            var userId = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var usuario = await _context.Usuarios
                .Include(u => u.SuscripcionActual)
                    .ThenInclude(s => s.PlanFacturacion)
                .FirstOrDefaultAsync(u => u.Id == int.Parse(userId));

            if (usuario?.SuscripcionActual == null)
                return NotFound("No se encontró suscripción activa");

            var suscripcion = usuario.SuscripcionActual;
            var plan = suscripcion.PlanFacturacion;

            var documentosLimite = plan.LimiteDocumentosAnuales ?? -1;
            var porcentajeUso = documentosLimite > 0
                ? (double)suscripcion.DocumentosUsados / documentosLimite * 100
                : 0;

            var diasRestantes = suscripcion.FechaFin.HasValue
                ? (suscripcion.FechaFin.Value - DateTime.Now).Days
                : 0;

            return Ok(new
            {
                documentosUsados = suscripcion.DocumentosUsados,
                documentosLimite = documentosLimite,
                porcentajeUso = Math.Round(porcentajeUso, 2),
                diasRestantes = Math.Max(0, diasRestantes),
                fechaInicio = suscripcion.FechaInicio,
                fechaFin = suscripcion.FechaFin,
                advertencia = porcentajeUso >= 80
            });
        }

        // POST: api/planes/actualizar
        [HttpPost("actualizar")]
        public async Task<ActionResult> ActualizarPlan([FromBody] ActualizarPlanRequest request)
        {
            var userId = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var usuario = await _context.Usuarios
                .Include(u => u.Suscripciones)
                .FirstOrDefaultAsync(u => u.Id == int.Parse(userId));

            if (usuario == null)
                return NotFound("Usuario no encontrado");

            var planNuevo = await _context.PlanesFacturacion
                .FirstOrDefaultAsync(p => p.Id == request.PlanId);

            if (planNuevo == null)
                return NotFound("Plan no encontrado");

            // Desactivar suscripción actual
            if (usuario.SuscripcionActual != null)
            {
                usuario.SuscripcionActual.Activa = false;
                usuario.SuscripcionActual.FechaFin = DateTime.Now;
            }

            // Crear nueva suscripción
            var nuevaSuscripcion = new SuscripcionFacturacion
            {
                UsuarioId = usuario.Id,
                PlanFacturacionId = request.PlanId,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddMonths(request.PeriodoAnual ? 12 : 1),
                DocumentosUsados = 0,
                Activa = true
            };

            _context.SuscripcionesFacturacion.Add(nuevaSuscripcion);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Plan actualizado exitosamente" });
        }
    }

    public class ActualizarPlanRequest
    {
        public int PlanId { get; set; }
        public bool PeriodoAnual { get; set; }
    }
}
