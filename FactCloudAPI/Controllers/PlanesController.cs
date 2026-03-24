using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Planes;
using FactCloudAPI.Models.Planes;
using FactCloudAPI.Models.Suscripciones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        [AllowAnonymous]
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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var suscripcion = await _context.SuscripcionesFacturacion
                .Include(s => s.PlanFacturacion)
                    .ThenInclude(p => p.Features)
                .Where(s => s.UsuarioId == int.Parse(userId) && s.Activa)
                .OrderByDescending(s => s.FechaInicio)
                .FirstOrDefaultAsync();

            if (suscripcion == null)
                return NotFound("No se encontró suscripción activa");

            var plan = suscripcion.PlanFacturacion;
            if (plan == null)
                return NotFound("El plan de la suscripción no existe");

            return Ok(new
            {
                id = plan.Id,
                nombre = plan.Nombre,
                descripcion = plan.Descripcion,
                precioMensual = plan.PrecioMensualFinal,
                precioAnual = plan.PrecioAnualFinal,
                facturasMensuales = plan.LimiteDocumentosAnuales,
                limiteDocumentosAnuales = plan.LimiteDocumentosAnuales,
                limiteUsuarios = plan.LimiteUsuarios,
                caracteristicas = plan.Features.Select(f => f.Texto).ToList()
            });
        }

        // GET: api/planes/estadisticas
        [HttpGet("estadisticas")]
        public async Task<ActionResult<object>> GetEstadisticas()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var id = int.Parse(userId);

            var suscripcion = await _context.SuscripcionesFacturacion
                .Include(s => s.PlanFacturacion)
                .Where(s => s.UsuarioId == id && s.Activa)
                .OrderByDescending(s => s.FechaInicio)
                .FirstOrDefaultAsync();

            if (suscripcion == null)
                return NotFound("No se encontró suscripción activa");

            if (suscripcion.PlanFacturacion == null)
                return NotFound("El plan de la suscripción no existe");

            var plan = suscripcion.PlanFacturacion;
            var fechaInicio = suscripcion.FechaInicio;

            // ── Contar documentos reales desde las tablas ──────────────────
            // Se suma cada tipo de documento creado desde el inicio
            // de la suscripción activa para el usuario autenticado.
            var facturas = await _context.Facturas
                .Where(f => f.UsuarioId == id && f.FechaEmision >= fechaInicio)
                .CountAsync();

            var notasCredito = await _context.NotasCredito
                .Where(n => n.UsuarioId == id && n.FechaElaboracion >= fechaInicio)
                .CountAsync();

            var notasDebito = await _context.NotasDebito
                .Where(n => n.UsuarioId == id && n.FechaElaboracion >= fechaInicio)
                .CountAsync();

            var documentosSoporte = await _context.DocumentosSoporte
                .Where(d => d.UsuarioId == id && d.Estado && d.FechaGeneracion >= fechaInicio)
                .CountAsync();

            var documentosUsados = facturas + notasCredito + notasDebito + documentosSoporte;

            // Sincronizar el campo para mantener consistencia
            if (suscripcion.DocumentosUsados != documentosUsados)
            {
                suscripcion.DocumentosUsados = documentosUsados;
                await _context.SaveChangesAsync();
            }

            var documentosLimite = plan.LimiteDocumentosAnuales ?? -1;
            var porcentajeUso = documentosLimite > 0
                ? (double)documentosUsados / documentosLimite * 100
                : 0;

            var diasRestantes = suscripcion.FechaFin.HasValue
                ? (suscripcion.FechaFin.Value - DateTime.Now).Days
                : 0;

            return Ok(new
            {
                documentosUsados,
                documentosLimite,
                porcentajeUso = Math.Round(porcentajeUso, 2),
                diasRestantes = Math.Max(0, diasRestantes),
                fechaInicio = suscripcion.FechaInicio,
                fechaFin = suscripcion.FechaFin,
                advertencia = porcentajeUso >= 80,
                desglose = new { facturas, notasCredito, notasDebito, documentosSoporte }
            });
        }

        // POST: api/planes/actualizar
        [HttpPost("actualizar")]
        public async Task<ActionResult> ActualizarPlan([FromBody] ActualizarPlanRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var id = int.Parse(userId);

            var planNuevo = await _context.PlanesFacturacion
                .FirstOrDefaultAsync(p => p.Id == request.PlanId);

            if (planNuevo == null)
                return NotFound("Plan no encontrado");

            var suscripcionActiva = await _context.SuscripcionesFacturacion
                .Where(s => s.UsuarioId == id && s.Activa)
                .FirstOrDefaultAsync();

            if (suscripcionActiva != null)
            {
                suscripcionActiva.Activa = false;
                suscripcionActiva.FechaFin = DateTime.Now;
            }

            var nuevaSuscripcion = new SuscripcionFacturacion
            {
                UsuarioId = id,
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