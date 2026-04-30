using NubeeAPI.Data;
using NubeeAPI.DTOs;
using NubeeAPI.DTOs.Impuestos;
using NubeeAPI.Models;
using NubeeAPI.Models.Impuestos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace NubeeAPI.Controllers.Impuestos
{
    [ApiController]
    [Route("api/autorretenciones")]
    [Authorize]
    public class AutoretencionesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public AutoretencionesController(ApplicationDbContext db)
        {
            _db = db;
        }

        private int GetUsuarioId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarioId = GetUsuarioId();

            var lista = await _db.Autorretenciones
                .Include(a => a.CuentaDebito)
                .Include(a => a.CuentaCredito)
                .Where(a => a.UsuarioId == null || a.UsuarioId == usuarioId) // ? fix
                .OrderBy(a => a.Codigo)
                .Select(a => MapToDto(a))
                .ToListAsync();

            return Ok(lista);
        }

        // -- GET /api/autorretenciones/{id} --------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuarioId = GetUsuarioId();
            var ar = await _db.Autorretenciones
                .Include(a => a.CuentaDebito)
                .Include(a => a.CuentaCredito)
                .FirstOrDefaultAsync(a => a.Id == id && a.UsuarioId == usuarioId);

            if (ar == null) return NotFound();
            return Ok(MapToDto(ar));
        }

        // -- POST /api/autorretenciones ------------------------------------
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CrearAutoretencionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuarioId = GetUsuarioId();

            if (await _db.Autorretenciones.AnyAsync(a => a.UsuarioId == usuarioId && a.Codigo == dto.Codigo))
                return Conflict(new { message = $"Ya existe una autorretención con el código {dto.Codigo}." });

            // Validar cuentas PUC
            if (dto.CuentaDebitoId.HasValue)
            {
                var existe = await _db.CuentasContables
                    .AnyAsync(c => c.Id == dto.CuentaDebitoId && c.UsuarioId == usuarioId);
                if (!existe) return BadRequest(new { message = "La cuenta débito PUC no existe." });
            }

            if (dto.CuentaCreditoId.HasValue)
            {
                var existe = await _db.CuentasContables
                    .AnyAsync(c => c.Id == dto.CuentaCreditoId && c.UsuarioId == usuarioId);
                if (!existe) return BadRequest(new { message = "La cuenta crédito PUC no existe." });
            }

            var ar = new Autoretencion
            {
                UsuarioId = usuarioId,
                Codigo = dto.Codigo,
                Nombre = dto.Nombre,
                TipoAutoretencion = dto.TipoAutoretencion,
                Tarifa = dto.Tarifa,
                BaseMinimaAplicacion = dto.BaseMinimaAplicacion,
                TipoBase = dto.TipoBase,
                CuentaDebitoId = dto.CuentaDebitoId,
                CuentaCreditoId = dto.CuentaCreditoId
            };

            _db.Autorretenciones.Add(ar);
            await _db.SaveChangesAsync();

            await _db.Entry(ar).Reference(a => a.CuentaDebito).LoadAsync();
            await _db.Entry(ar).Reference(a => a.CuentaCredito).LoadAsync();

            return CreatedAtAction(nameof(GetById), new { id = ar.Id }, MapToDto(ar));
        }

        // -- PUT /api/autorretenciones/{id} --------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActualizarAutoretencionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuarioId = GetUsuarioId();
            var ar = await _db.Autorretenciones
                .Include(a => a.CuentaDebito)
                .Include(a => a.CuentaCredito)
                .FirstOrDefaultAsync(a => a.Id == id && a.UsuarioId == usuarioId);

            if (ar == null) return NotFound();

            if (dto.Nombre != null) ar.Nombre = dto.Nombre;
            if (dto.Tarifa.HasValue) ar.Tarifa = dto.Tarifa.Value;
            if (dto.BaseMinimaAplicacion.HasValue) ar.BaseMinimaAplicacion = dto.BaseMinimaAplicacion;
            if (dto.EnUso.HasValue) ar.EnUso = dto.EnUso.Value;
            if (dto.CuentaDebitoId.HasValue) ar.CuentaDebitoId = dto.CuentaDebitoId;
            if (dto.CuentaCreditoId.HasValue) ar.CuentaCreditoId = dto.CuentaCreditoId;

            await _db.SaveChangesAsync();
            return Ok(MapToDto(ar));
        }

        // -- DELETE /api/autorretenciones/{id} -----------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioId = GetUsuarioId();
            var ar = await _db.Autorretenciones
                .FirstOrDefaultAsync(a => a.Id == id && a.UsuarioId == usuarioId);

            if (ar == null) return NotFound();

            _db.Autorretenciones.Remove(ar);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // -- Helpers -------------------------------------------------------
        private static AutoretencionDto MapToDto(Autoretencion a) => new()
        {
            Id = a.Id,
            Codigo = a.Codigo,
            Nombre = a.Nombre,
            TipoAutoretencion = a.TipoAutoretencion,
            Tarifa = a.Tarifa,
            TarifaDisplay = a.TarifaDisplay,
            BaseMinimaAplicacion = a.BaseMinimaAplicacion,
            TipoBase = a.TipoBase,
            EnUso = a.EnUso,
            CuentaDebito = a.CuentaDebito == null ? null : new()
            {
                Id = a.CuentaDebito.Id,
                Codigo = a.CuentaDebito.Codigo,
                Nombre = a.CuentaDebito.Nombre,
                CodigoNombre = a.CuentaDebito.CodigoNombre
            },
            CuentaCredito = a.CuentaCredito == null ? null : new()
            {
                Id = a.CuentaCredito.Id,
                Codigo = a.CuentaCredito.Codigo,
                Nombre = a.CuentaCredito.Nombre,
                CodigoNombre = a.CuentaCredito.CodigoNombre
            }
        };
    }
}
