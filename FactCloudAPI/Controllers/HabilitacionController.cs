using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NubeeAPI.Data;
using NubeeAPI.DTOs.Factus;
using NubeeAPI.DTOs.Habilitacion;
using NubeeAPI.Models;
using NubeeAPI.Models.DTOs;
using NubeeAPI.Models.Usuarios;
using NubeeAPI.Services;
using NubeeAPI.Services.Factus;
using System.Security.Claims;

namespace NubeeAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HabilitacionController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IFactusService _factusService;
        private readonly ILogger<HabilitacionController> _logger;

        public HabilitacionController(
            ApplicationDbContext db,
            IFactusService factusService,
            ILogger<HabilitacionController> logger)
        {
            _db = db;
            _factusService = factusService;
            _logger = logger;
        }

        private int? ObtenerUsuarioId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? User.FindFirst("sub")?.Value;
            return int.TryParse(claim, out var id) ? id : null;
        }

        private async Task<Negocio?> GetNegocioAsync(int usuarioId) =>
            await _db.Negocios
                .Include(n => n.Resoluciones)
                .FirstOrDefaultAsync(n => n.UsuarioId == usuarioId);

        // ══════════════════════════════════════════════════════
        //  GET api/Habilitacion/estado
        //  Estado completo del proceso de habilitación
        // ══════════════════════════════════════════════════════
        [HttpGet("estado")]
        public async Task<IActionResult> GetEstado()
        {
            var uid = ObtenerUsuarioId();
            if (uid == null) return Unauthorized();

            var negocio = await GetNegocioAsync(uid.Value);
            if (negocio == null)
                return BadRequest(new { mensaje = "Negocio no configurado", codigo = "SIN_NEGOCIO" });

            var config = await _db.ConfiguracionesDian
                                .FirstOrDefaultAsync(c => c.NegocioId == negocio.Id);
            var resolucion = negocio.ResolucionActiva;

            // Paso actual del wizard de habilitación
            string pasoActual = resolucion == null ? "SIN_RESOLUCION"
                              : !resolucion.EstaVigente ? "RESOLUCION_VENCIDA"
                              : resolucion.FactusRangoId == null ? "PENDIENTE_FACTUS"
                              : "HABILITADO";

            return Ok(new
            {
                // Datos negocio
                tieneNegocio = true,
                nit = negocio.Nit,
                razonSocial = negocio.RazonSocial,

                // Estado general
                pasoActual,
                habilitado = pasoActual == "HABILITADO",

                // Configuración software (paso 1)
                software = config == null ? null : new
                {
                    config.SoftwareProveedor,
                    config.SoftwarePIN,
                    config.AmbienteDIAN
                },

                // Resolución DIAN (paso 2)
                tieneResolucion = resolucion != null,
                resolucionVigente = resolucion?.EstaVigente ?? false,
                resolucion = resolucion == null ? null : new
                {
                    resolucion.Id,
                    resolucion.NumeroAutorizacion,
                    resolucion.Prefijo,
                    rangoDesde = resolucion.RangoDesde,
                    rangoHasta = resolucion.RangoHasta,
                    fechaInicio = resolucion.FechaInicio.ToString("yyyy-MM-dd"),
                    fechaFin = resolucion.FechaFin.ToString("yyyy-MM-dd"),
                    resolucion.TipoAmbiente,
                    diasRestantes = resolucion.DiasRestantes
                },

                // Factus (paso 3)
                habitadoEnFactus = resolucion?.FactusRangoId != null,
                factusRangoId = resolucion?.FactusRangoId
            });
        }

        // ══════════════════════════════════════════════════════
        //  POST api/Habilitacion/software
        //  Paso 1: guardar datos del software proveedor
        // ══════════════════════════════════════════════════════
        [HttpPost("software")]
        public async Task<IActionResult> GuardarSoftware([FromBody] ConfiguracionSoftwareDto dto)
        {
            var uid = ObtenerUsuarioId();
            if (uid == null) return Unauthorized();

            var negocio = await _db.Negocios.FirstOrDefaultAsync(n => n.UsuarioId == uid.Value);
            if (negocio == null)
                return BadRequest(new { mensaje = "No tienes un negocio registrado." });

            var config = await _db.ConfiguracionesDian
                .FirstOrDefaultAsync(c => c.NegocioId == negocio.Id)
                ?? new ConfiguracionDian { NegocioId = negocio.Id };

            config.SoftwareProveedor = dto.NitFabricante;
            config.SoftwarePIN = dto.CodigoSoftware;

            if (config.Id == 0) _db.ConfiguracionesDian.Add(config);
            await _db.SaveChangesAsync();

            return Ok(new { mensaje = "Software registrado correctamente." });
        }

        // ══════════════════════════════════════════════════════
        //  POST api/Habilitacion/test-set
        //  Paso 1b: guardar TestSetId DIAN (solo sandbox)
        // ══════════════════════════════════════════════════════
        [HttpPost("test-set")]
        public async Task<IActionResult> GuardarTestSet([FromBody] TestSetDto dto)
        {
            var uid = ObtenerUsuarioId();
            if (uid == null) return Unauthorized();

            var negocio = await _db.Negocios.FirstOrDefaultAsync(n => n.UsuarioId == uid.Value);
            if (negocio == null)
                return BadRequest(new { mensaje = "No tienes un negocio registrado." });

            var config = await _db.ConfiguracionesDian
                .FirstOrDefaultAsync(c => c.NegocioId == negocio.Id);
            if (config == null)
                return BadRequest(new { mensaje = "Registra primero los datos del software." });

            config.AmbienteDIAN = dto.TestSetId;
            await _db.SaveChangesAsync();

            return Ok(new { mensaje = "TestSetId guardado." });
        }

        // ══════════════════════════════════════════════════════
        //  POST api/Habilitacion/resolucion
        //  Paso 2: registrar resolución DIAN del negocio
        // ══════════════════════════════════════════════════════
        [HttpPost("resolucion")]
        public async Task<IActionResult> GuardarResolucion([FromBody] ResolucionDianDto dto)
        {
            var uid = ObtenerUsuarioId();
            if (uid == null) return Unauthorized();

            if (dto.NumeroAutorizacion?.Length != 14)
                return BadRequest(new { mensaje = "El número de autorización debe tener 14 dígitos." });

            var negocio = await GetNegocioAsync(uid.Value);
            if (negocio == null)
                return BadRequest(new { mensaje = "No tienes un negocio registrado." });

            // Desactivar resoluciones previas activas
            var anteriores = negocio.Resoluciones?
                .Where(r => r.Activa).ToList() ?? new();
            anteriores.ForEach(r => r.Activa = false);

            var nuevaResolucion = new ResolucionDIAN
            {
                NegocioId = negocio.Id,
                NumeroAutorizacion = dto.NumeroAutorizacion,
                Prefijo = dto.Prefijo,
                RangoDesde = dto.RangoDesde,
                RangoHasta = dto.RangoHasta,
                FechaInicio = DateTime.Parse(dto.FechaInicio),
                FechaFin = DateTime.Parse(dto.FechaFin),
                ClaveTecnica = dto.ClaveTecnica,
                TipoAmbiente = int.Parse(dto.TipoAmbiente),
                Activa = true,
                FechaRegistro = DateTime.UtcNow
            };

            _db.ResolucionesDIAN.Add(nuevaResolucion);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Resolución registrada. Ahora registra el rango en Factus.",
                resolucionId = nuevaResolucion.Id,
                siguientePaso = "POST api/Habilitacion/registrar-rango"
            });
        }

        // ══════════════════════════════════════════════════════
        //  POST api/Habilitacion/registrar-rango
        //  Paso 3: habilitar empresa en Factus
        // ══════════════════════════════════════════════════════
        [HttpPost("registrar-rango")]
        public async Task<IActionResult> RegistrarRango([FromBody] HabilitarEmpresaDto dto)
        {
            var uid = ObtenerUsuarioId();
            if (uid == null) return Unauthorized();

            var resolucion = await _db.ResolucionesDIAN
                .Include(r => r.Negocio)
                .FirstOrDefaultAsync(r => r.Id == dto.ResolucionId
                                       && r.Negocio!.UsuarioId == uid);

            if (resolucion == null)
                return NotFound(new { mensaje = "Resolución no encontrada." });

            if (!resolucion.EstaVigente)
                return BadRequest(new
                {
                    mensaje = $"La resolución venció el {resolucion.FechaFin:dd/MM/yyyy}",
                    codigo = "RESOLUCION_VENCIDA"
                });

            if (resolucion.FactusRangoId.HasValue && !dto.Forzar)
                return Ok(new
                {
                    mensaje = "Esta resolución ya está registrada en Factus.",
                    factusRangoId = resolucion.FactusRangoId,
                    yaRegistrada = true
                });

            try
            {
                var rangoId = await _factusService.RegistrarRangoAsync(resolucion, resolucion.Negocio!);
                resolucion.FactusRangoId = rangoId;
                await _db.SaveChangesAsync();

                _logger.LogInformation(
                    "Empresa NIT {Nit} habilitada en Factus con RangoId {RangoId}",
                    resolucion.Negocio!.Nit, rangoId);

                return Ok(new
                {
                    mensaje = "✅ Empresa habilitada en Factus. Ya puede emitir facturas.",
                    factusRangoId = rangoId,
                    prefijo = resolucion.Prefijo,
                    rangoDesde = resolucion.RangoDesde,
                    rangoHasta = resolucion.RangoHasta
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error registrando rango Factus para resolución {Id}", dto.ResolucionId);
                return StatusCode(500, new
                {
                    mensaje = "Error al registrar el rango en Factus.",
                    detalle = ex.Message
                });
            }
        }
    }
}