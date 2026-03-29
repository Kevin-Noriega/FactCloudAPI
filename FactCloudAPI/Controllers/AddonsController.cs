// Controllers/AddonsController.cs
using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Addons;
using FactCloudAPI.Models.Planes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FactCloudAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AddonsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AddonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ── Helper: obtener userId del token ──────────────────────────
        private int? GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out var id) ? id : null;
        }

        // GET: api/addons
        // Devuelve todos los addons activos, marcando cuáles ya tiene el usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddonResponseDto>>> GetAddons()
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            // IDs de addons que el usuario ya tiene activos
            var contratados = await _context.UsuariosAddons
                .Where(ua => ua.UsuarioId == userId && ua.Activo)
                .Select(ua => ua.AddonId)
                .ToListAsync();

            var addons = await _context.Addons
                .Where(a => a.Activo)
                .OrderBy(a => a.Tipo)
                .ThenBy(a => a.Precio)
                .Select(a => new AddonResponseDto
                {
                    Id = a.Id,
                    Nombre = a.Nombre,
                    Descripcion = a.Descripcion,
                    Precio = a.Precio,
                    Unidad = a.Unidad,
                    Tipo = a.Tipo,
                    Color = a.Color,
                    Contratado = contratados.Contains(a.Id)
                })
                .ToListAsync();

            return Ok(addons);
        }

        // GET: api/addons/mis-addons
        // Devuelve los addons que tiene contratados el usuario autenticado
        [HttpGet("mis-addons")]
        public async Task<ActionResult<IEnumerable<UsuarioAddonResponseDto>>> GetMisAddons()
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var misAddons = await _context.UsuariosAddons
                .Include(ua => ua.Addon)
                .Where(ua => ua.UsuarioId == userId && ua.Activo)
                .OrderByDescending(ua => ua.FechaContratacion)
                .Select(ua => new UsuarioAddonResponseDto
                {
                    Id = ua.Id,
                    AddonId = ua.AddonId,
                    Nombre = ua.Addon.Nombre,
                    Descripcion = ua.Addon.Descripcion,
                    Precio = ua.Addon.Precio,
                    Unidad = ua.Addon.Unidad,
                    Tipo = ua.Addon.Tipo,
                    Color = ua.Addon.Color,
                    FechaContratacion = ua.FechaContratacion,
                    FechaVencimiento = ua.FechaVencimiento,
                    Activo = ua.Activo
                })
                .ToListAsync();

            return Ok(misAddons);
        }

        // POST: api/addons/agregar
        // Contrata uno o varios addons para el usuario autenticado
        [HttpPost("agregar")]
        public async Task<ActionResult> AgregarAddons([FromBody] AgregarAddonsRequest request)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            if (request.Addons == null || !request.Addons.Any())
                return BadRequest(new { mensaje = "Debes seleccionar al menos un complemento." });

            // Verificar que el usuario tiene suscripción activa
            var suscripcion = await _context.SuscripcionesFacturacion
                .Where(s => s.UsuarioId == userId && s.Activa)
                .FirstOrDefaultAsync();

            if (suscripcion == null)
                return BadRequest(new { mensaje = "Necesitas un plan activo para agregar complementos." });

            // IDs ya contratados (evitar duplicados)
            var yaContratados = await _context.UsuariosAddons
                .Where(ua => ua.UsuarioId == userId && ua.Activo && request.Addons.Contains(ua.AddonId))
                .Select(ua => ua.AddonId)
                .ToListAsync();

            var nuevosIds = request.Addons.Except(yaContratados).ToList();

            if (!nuevosIds.Any())
                return BadRequest(new { mensaje = "Todos los complementos seleccionados ya están contratados." });

            // Verificar que los addons existen y están activos
            var addonsValidos = await _context.Addons
                .Where(a => nuevosIds.Contains(a.Id) && a.Activo)
                .ToListAsync();

            if (addonsValidos.Count != nuevosIds.Count)
                return BadRequest(new { mensaje = "Uno o más complementos no son válidos." });

            // Crear registros UsuarioAddon
            var fechaVencimiento = suscripcion.FechaFin; // mismo vencimiento que la suscripción

            var nuevosRegistros = addonsValidos.Select(a => new UsuarioAddon
            {
                UsuarioId = userId.Value,
                AddonId = a.Id,
                FechaContratacion = DateTime.Now,
                FechaVencimiento = fechaVencimiento,
                Activo = true
            }).ToList();

            _context.UsuariosAddons.AddRange(nuevosRegistros);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = $"{nuevosRegistros.Count} complemento(s) agregado(s) exitosamente.",
                agregados = addonsValidos.Select(a => a.Nombre).ToList(),
                ignorados = yaContratados.Count > 0
                    ? $"{yaContratados.Count} complemento(s) ya estaban contratados."
                    : null
            });
        }

        // DELETE: api/addons/cancelar
        // Cancela (desactiva) un addon del usuario
        [HttpDelete("cancelar/{addonId}")]
        public async Task<ActionResult> CancelarAddon(int addonId)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var usuarioAddon = await _context.UsuariosAddons
                .FirstOrDefaultAsync(ua =>
                    ua.UsuarioId == userId &&
                    ua.AddonId == addonId &&
                    ua.Activo);

            if (usuarioAddon == null)
                return NotFound(new { mensaje = "No se encontró el complemento contratado." });

            usuarioAddon.Activo = false;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Complemento cancelado exitosamente." });
        }
    }

}
