using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NubeeAPI.DTOs.Habilitacion;
using NubeeAPI.Services.Factus;
using NubeeAPI.Services.Habilitacion;

namespace NubeeAPI.Controllers
{
    /// <summary>
    /// Controller de habilitación electrónica.
    /// NO contiene lógica de negocio — delega todo a IHabilitacionService.
    /// Responsabilidades aquí: autenticación JWT, validación de modelo, HTTP codes.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HabilitacionController : BaseController
    {
        private readonly IHabilitacionService _service;
        private readonly ILogger<HabilitacionController> _logger;

        public HabilitacionController(
            IHabilitacionService service,
            ILogger<HabilitacionController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // ══════════════════════════════════════════════════════════════
        //  GET api/Habilitacion/estado
        //  Devuelve el estado completo del wizard de habilitación.
        //  Usado por el frontend para posicionar el stepper al montar.
        // ══════════════════════════════════════════════════════════════
        [HttpGet("estado")]
        public async Task<IActionResult> GetEstado()
        {
            var uid = ObtenerUsuarioId();
            if (uid == null) return UsuarioNoAutenticado();

            var estado = await _service.ObtenerEstadoAsync(uid.Value);
            return Ok(estado);
        }

        // ══════════════════════════════════════════════════════════════
        //  GET api/Habilitacion/perfil-empresa
        // ══════════════════════════════════════════════════════════════
        [HttpGet("perfil-empresa")]
        public async Task<IActionResult> GetPerfilEmpresa()
        {
            var uid = ObtenerUsuarioId();
            if (uid == null) return UsuarioNoAutenticado();

            var dto = await _service.ObtenerPerfilEmpresaAsync(uid.Value);
            if (dto == null)
                return NotFound(new { mensaje = "No tienes un negocio registrado." });

            return Ok(dto);
        }

        // ══════════════════════════════════════════════════════════════
        //  PUT api/Habilitacion/perfil-empresa  (Paso 1)
        // ══════════════════════════════════════════════════════════════
        [HttpPut("perfil-empresa")]
        public async Task<IActionResult> ActualizarPerfilEmpresa([FromBody] PerfilEmpresaDto dto)
        {
            var uid = ObtenerUsuarioId();
            if (uid == null) return UsuarioNoAutenticado();

            try
            {
                await _service.ActualizarPerfilEmpresaAsync(uid.Value, dto);
                return Ok(new { mensaje = "Perfil actualizado correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
        }

        // ══════════════════════════════════════════════════════════════
        //  POST api/Habilitacion/software  (Paso 1 — Software DIAN)
        // ══════════════════════════════════════════════════════════════
        [HttpPost("software")]
        public async Task<IActionResult> GuardarSoftware([FromBody] ConfiguracionSoftwareDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var uid = ObtenerUsuarioId();
            if (uid == null) return UsuarioNoAutenticado();

            try
            {
                await _service.GuardarSoftwareAsync(uid.Value, dto);
                return Ok(new { mensaje = "Datos del software guardados correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // ══════════════════════════════════════════════════════════════
        //  GET api/Habilitacion/certificado  (Paso 2 — lectura)
        // ══════════════════════════════════════════════════════════════
        [HttpGet("certificado")]
        public async Task<IActionResult> GetCertificado()
        {
            var uid = ObtenerUsuarioId();
            if (uid == null) return UsuarioNoAutenticado();

            try
            {
                var cert = await _service.ObtenerCertificadoAsync(uid.Value);
                if (cert == null) return NotFound(new { mensaje = "No hay certificado configurado." });
                return Ok(cert);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
        }

        // ══════════════════════════════════════════════════════════════
        //  PUT api/Habilitacion/certificado  (Paso 2 — guardar)
        //  CORRECCIÓN: antes no existía este endpoint. El frontend
        //  en guardarEtapa2() solo validaba localmente sin llamar backend.
        // ══════════════════════════════════════════════════════════════
        [HttpPut("certificado")]
        public async Task<IActionResult> GuardarCertificado([FromBody] CertificadoDto dto)
        {
            var uid = ObtenerUsuarioId();
            if (uid == null) return UsuarioNoAutenticado();

            try
            {
                var result = await _service.GuardarCertificadoAsync(uid.Value, dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
        }

        // ══════════════════════════════════════════════════════════════
        //  POST api/Habilitacion/test-set  (Paso 3)
        //  CORRECCIÓN: antes guardaba en config.AmbienteDIAN (int).
        //  Ahora guarda correctamente en config.TestSetId (string).
        // ══════════════════════════════════════════════════════════════
        [HttpPost("test-set")]
        public async Task<IActionResult> GuardarTestSet([FromBody] TestSetDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var uid = ObtenerUsuarioId();
            if (uid == null) return UsuarioNoAutenticado();

            try
            {
                await _service.GuardarTestSetAsync(uid.Value, dto);
                return Ok(new { mensaje = "Test Set DIAN guardado correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // ══════════════════════════════════════════════════════════════
        //  POST api/Habilitacion/resolucion  (Paso 4)
        // ══════════════════════════════════════════════════════════════
        [HttpPost("resolucion")]
        public async Task<IActionResult> GuardarResolucion([FromBody] ResolucionDianDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var uid = ObtenerUsuarioId();
            if (uid == null) return UsuarioNoAutenticado();

            try
            {
                var resolucion = await _service.GuardarResolucionAsync(uid.Value, dto);
                return Ok(new
                {
                    mensaje = "Resolución registrada. Registra el rango en Factus para continuar.",
                    resolucionId = resolucion.Id,
                    siguientePaso = "POST api/Habilitacion/registrar-rango",
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // ══════════════════════════════════════════════════════════════
        //  POST api/Habilitacion/registrar-rango  (Paso 4b)
        // ══════════════════════════════════════════════════════════════
        [HttpPost("registrar-rango")]
        public async Task<IActionResult> RegistrarRango([FromBody] RegistrarRangoDto dto)
        {
            var uid = ObtenerUsuarioId();
            if (uid == null) return UsuarioNoAutenticado();

            try
            {
                var result = await _service.RegistrarRangoFactusAsync(uid.Value, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                var codigo = ex.Message.Contains("RESOLUCION_VENCIDA")
                    ? "RESOLUCION_VENCIDA" : "ERROR_NEGOCIO";
                return BadRequest(new { mensaje = ex.Message, codigo });
            }
            catch (FactusApiException ex)
            {
                _logger.LogError(ex,
                    "Error Factus registrando rango. Status={Status}", ex.StatusCode);
                return StatusCode(502, new
                {
                    mensaje = "Error al registrar el rango en Factus.",
                    detalle = ex.Message,
                    codigo = "FACTUS_ERROR",
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado registrando rango.");
                return StatusCode(500, new
                {
                    mensaje = "Error interno. Intenta de nuevo.",
                    codigo = "INTERNAL_ERROR",
                });
            }
        }

        // ══════════════════════════════════════════════════════════════
        //  GET api/Habilitacion/rango-actual  (Paso 5)
        //  Consulta el rango activo en Nubee + Factus (solo lectura).
        // ══════════════════════════════════════════════════════════════
        [HttpGet("rango-actual")]
        public async Task<IActionResult> GetRangoActual()
        {
            var uid = ObtenerUsuarioId();
            if (uid == null) return UsuarioNoAutenticado();

            try
            {
                var rango = await _service.ObtenerRangoActivoAsync(uid.Value);
                if (rango == null)
                    return NotFound(new
                    {
                        mensaje = "No tienes una resolución activa.",
                        codigo = "SIN_RESOLUCION",
                    });
                return Ok(rango);
            }
            catch (FactusApiException ex)
            {
                _logger.LogWarning(ex, "No se pudo consultar Factus para rango activo.");
                return StatusCode(502, new
                {
                    mensaje = "No se pudo obtener el estado de Factus. Los datos locales están disponibles.",
                    codigo = "FACTUS_UNAVAILABLE",
                });
            }
        }

        // ══════════════════════════════════════════════════════════════
        //  POST api/Habilitacion/finalizar  (Paso 6)
        // ══════════════════════════════════════════════════════════════
        [HttpPost("finalizar")]
        public async Task<IActionResult> Finalizar()
        {
            var uid = ObtenerUsuarioId();
            if (uid == null) return UsuarioNoAutenticado();

            try
            {
                await _service.FinalizarHabilitacionAsync(uid.Value);
                return Ok(new
                {
                    mensaje = "¡Habilitación completada! Ya puedes emitir facturas electrónicas.",
                    habilitacionCompleta = true,
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message, codigo = "VALIDACION_FALLIDA" });
            }
            catch (FactusApiException ex)
            {
                return StatusCode(502, new
                {
                    mensaje = "Error al verificar la empresa en Factus.",
                    detalle = ex.Message,
                    codigo = "FACTUS_ERROR",
                });
            }
        }
    }
}
