using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Habilitacion;
using FactCloudAPI.Models;
using FactCloudAPI.Models.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FactCloudAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HabilitacionController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public HabilitacionController(ApplicationDbContext db) => _db = db;

        private int? UsuarioId => int.TryParse(
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : null;

        private async Task<Negocio?> GetNegocioAsync(int usuarioId) =>
            await _db.Negocios.FirstOrDefaultAsync(n => n.UsuarioId == usuarioId);

        [HttpGet("estado")]
        public async Task<IActionResult> GetEstado()
        {
            var uid = UsuarioId;
            if (uid == null) return Unauthorized();

            var negocio = await GetNegocioAsync(uid.Value);

            ConfiguracionDian? config = null;
            ResolucionDIAN? resolucion = null;

            if (negocio != null)
            {
                config = await _db.ConfiguracionesDian
                    .FirstOrDefaultAsync(c => c.NegocioId == negocio.Id);

                resolucion = await _db.ResolucionesDIAN
                    .FirstOrDefaultAsync(r => r.NegocioId == negocio.Id && r.Activa);
            }

            string estado = "pendiente";
            if (resolucion != null) estado = "completado";

            return Ok(new
            {
                estadoFEV = estado,
                tieneNegocio = negocio != null,
                software = config == null ? null : new
                {
                    config.SoftwareProveedor,
                    config.SoftwarePIN,
                    config.PrefijoAutorizadoDIAN,
                    config.NumeroResolucionDIAN,
                    config.RangoNumeracionDesde,
                    config.RangoNumeracionHasta,
                    config.AmbienteDIAN,
                    fechaInicio = config.FechaVigenciaInicio?.ToString("yyyy-MM-dd"),
                    fechaFin = config.FechaVigenciaFinal?.ToString("yyyy-MM-dd")
                },
                resolucion = resolucion == null ? null : new
                {
                    resolucion.NumeroAutorizacion,
                    resolucion.Prefijo,
                    rangoDesde = resolucion.RangoDesde,
                    rangoHasta = resolucion.RangoHasta,
                    fechaInicio = resolucion.FechaInicio.ToString("yyyy-MM-dd"),
                    fechaFin = resolucion.FechaFin.ToString("yyyy-MM-dd"),
                    resolucion.TipoAmbiente
                }
            });
        }

        [HttpPost("software")]
        public async Task<IActionResult> GuardarSoftware([FromBody] ConfiguracionSoftwareDto dto)
        {
            var uid = UsuarioId;
            if (uid == null) return Unauthorized();

            var negocio = await GetNegocioAsync(uid.Value);
            if (negocio == null)
                return BadRequest(new { mensaje = "No tienes un negocio registrado." });

            var config = await _db.ConfiguracionesDian
                .FirstOrDefaultAsync(c => c.NegocioId == negocio.Id)
                ?? new ConfiguracionDian { NegocioId = negocio.Id };

            // Mapear DTO → campos reales
            config.SoftwareProveedor = dto.NitFabricante;
            config.SoftwarePIN = dto.CodigoSoftware;
           // config.PrefijoAutorizadoDIAN = dto.Prefijo ?? "";

            if (config.Id == 0) _db.ConfiguracionesDian.Add(config);
            await _db.SaveChangesAsync();
            return Ok(new { mensaje = "Software registrado correctamente." });
        }

        [HttpPost("test-set")]
        public async Task<IActionResult> GuardarTestSet([FromBody] TestSetDto dto)
        {
            var uid = UsuarioId;
            if (uid == null) return Unauthorized();

            var negocio = await GetNegocioAsync(uid.Value);
            if (negocio == null)
                return BadRequest(new { mensaje = "No tienes un negocio registrado." });

            var config = await _db.ConfiguracionesDian
                .FirstOrDefaultAsync(c => c.NegocioId == negocio.Id);

            if (config == null)
                return BadRequest(new { mensaje = "Registra primero los datos del software." });

            config.AmbienteDIAN = dto.TestSetId;
            await _db.SaveChangesAsync();
            return Ok(new { mensaje = "TestSetId guardado. Puedes proceder con las pruebas DIAN." });
        }

        [HttpPost("resolucion")]
        public async Task<IActionResult> GuardarResolucion([FromBody] ResolucionDianDto dto)
        {
            var uid = UsuarioId;
            if (uid == null) return Unauthorized();

            if (dto.NumeroAutorizacion?.Length != 14)
                return BadRequest(new { mensaje = "El número de autorización debe tener 14 dígitos." });

            var negocio = await GetNegocioAsync(uid.Value);
            if (negocio == null)
                return BadRequest(new { mensaje = "No tienes un negocio registrado." });

            // Desactivar resoluciones previas del negocio
            var anteriores = await _db.ResolucionesDIAN
                .Where(r => r.NegocioId == negocio.Id && r.Activa)
                .ToListAsync();
            anteriores.ForEach(r => r.Activa = false);

            _db.ResolucionesDIAN.Add(new ResolucionDIAN
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
                Activa = true
            });

            await _db.SaveChangesAsync();
            return Ok(new { mensaje = "Resolución registrada. Habilitación completa." });
        }
    }
}