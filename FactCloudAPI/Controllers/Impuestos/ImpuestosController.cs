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

namespace NubeeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ImpuestosController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ImpuestosController(ApplicationDbContext db)
        {
            _db = db;
        }

        private int GetUsuarioId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? tipo)
        {
            var usuarioId = GetUsuarioId();

            var query = _db.Impuestos
                .Include(i => i.CuentaDebitoVentas)
                .Include(i => i.CuentaCreditoVentas)
                .Include(i => i.CuentaDebitoCompras)
                .Include(i => i.CuentaCreditoCompras)
                .Include(i => i.CuentaDevolucionVentas)
                .Include(i => i.CuentaDevolucionCompras)
                .Where(i => i.UsuarioId == null || i.UsuarioId == usuarioId) // ? fix
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(tipo))
                query = query.Where(i => i.TipoImpuesto == tipo);

            var impuestos = await query
                .OrderBy(i => i.TipoImpuesto)
                .ThenBy(i => i.Tarifa)
                .Select(i => MapToDto(i))
                .ToListAsync();

            return Ok(impuestos);
        }
        // -- GET /api/impuestos/{id} ---------------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuarioId = GetUsuarioId();
            var impuesto = await _db.Impuestos
                .Include(i => i.CuentaDebitoVentas)
                .Include(i => i.CuentaCreditoVentas)
                .Include(i => i.CuentaDebitoCompras)
                .Include(i => i.CuentaCreditoCompras)
                .Include(i => i.CuentaDevolucionVentas)
                .Include(i => i.CuentaDevolucionCompras)
                .FirstOrDefaultAsync(i => i.Id == id && i.UsuarioId == usuarioId);

            if (impuesto == null) return NotFound();
            return Ok(MapToDto(impuesto));
        }

        // -- POST /api/impuestos -------------------------------------------
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CrearImpuestoDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuarioId = GetUsuarioId();

            // Verificar código único
            if (await _db.Impuestos.AnyAsync(i => i.UsuarioId == usuarioId && i.Codigo == dto.Codigo))
                return Conflict(new { message = $"Ya existe un impuesto con el código {dto.Codigo}." });

            // Validar que las cuentas PUC existan y pertenezcan al usuario
            if (!await ValidarCuentasPUC(usuarioId,
                dto.CuentaDebitoVentasId, dto.CuentaCreditoVentasId,
                dto.CuentaDebitoComprasId, dto.CuentaCreditoComprasId,
                dto.CuentaDevolucionVentasId, dto.CuentaDevolucionComprasId))
                return BadRequest(new { message = "Una o más cuentas PUC no existen o no pertenecen a tu empresa." });

            var impuesto = new Impuesto
            {
                UsuarioId = usuarioId,
                Codigo = dto.Codigo,
                Nombre = dto.Nombre,
                TipoImpuesto = dto.TipoImpuesto,
                Tarifa = dto.Tarifa,
                PorValor = dto.PorValor,
                CodigoTributoDIAN = dto.CodigoTributoDIAN ?? InferirCodigoDIAN(dto.TipoImpuesto),
                CuentaDebitoVentasId = dto.CuentaDebitoVentasId,
                CuentaCreditoVentasId = dto.CuentaCreditoVentasId,
                CuentaDebitoComprasId = dto.CuentaDebitoComprasId,
                CuentaCreditoComprasId = dto.CuentaCreditoComprasId,
                CuentaDevolucionVentasId = dto.CuentaDevolucionVentasId,
                CuentaDevolucionComprasId = dto.CuentaDevolucionComprasId
            };

            _db.Impuestos.Add(impuesto);
            await _db.SaveChangesAsync();

            // Recargar con navegación
            await _db.Entry(impuesto).Reference(i => i.CuentaDebitoVentas).LoadAsync();
            await _db.Entry(impuesto).Reference(i => i.CuentaCreditoVentas).LoadAsync();
            await _db.Entry(impuesto).Reference(i => i.CuentaDebitoCompras).LoadAsync();
            await _db.Entry(impuesto).Reference(i => i.CuentaCreditoCompras).LoadAsync();

            return CreatedAtAction(nameof(GetById), new { id = impuesto.Id }, MapToDto(impuesto));
        }

        // -- PUT /api/impuestos/{id} ---------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActualizarImpuestoDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuarioId = GetUsuarioId();
            var impuesto = await _db.Impuestos
                .Include(i => i.CuentaDebitoVentas)
                .Include(i => i.CuentaCreditoVentas)
                .Include(i => i.CuentaDebitoCompras)
                .Include(i => i.CuentaCreditoCompras)
                .Include(i => i.CuentaDevolucionVentas)
                .Include(i => i.CuentaDevolucionCompras)
                .FirstOrDefaultAsync(i => i.Id == id && i.UsuarioId == usuarioId);

            if (impuesto == null) return NotFound();

            if (dto.Nombre != null) impuesto.Nombre = dto.Nombre;
            if (dto.Tarifa.HasValue) impuesto.Tarifa = dto.Tarifa.Value;
            if (dto.PorValor.HasValue) impuesto.PorValor = dto.PorValor.Value;
            if (dto.EnUso.HasValue) impuesto.EnUso = dto.EnUso.Value;

            if (dto.CuentaDebitoVentasId.HasValue) impuesto.CuentaDebitoVentasId = dto.CuentaDebitoVentasId;
            if (dto.CuentaCreditoVentasId.HasValue) impuesto.CuentaCreditoVentasId = dto.CuentaCreditoVentasId;
            if (dto.CuentaDebitoComprasId.HasValue) impuesto.CuentaDebitoComprasId = dto.CuentaDebitoComprasId;
            if (dto.CuentaCreditoComprasId.HasValue) impuesto.CuentaCreditoComprasId = dto.CuentaCreditoComprasId;
            if (dto.CuentaDevolucionVentasId.HasValue) impuesto.CuentaDevolucionVentasId = dto.CuentaDevolucionVentasId;
            if (dto.CuentaDevolucionComprasId.HasValue) impuesto.CuentaDevolucionComprasId = dto.CuentaDevolucionComprasId;

            await _db.SaveChangesAsync();
            return Ok(MapToDto(impuesto));
        }

        // -- DELETE /api/impuestos/{id} ------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioId = GetUsuarioId();
            var impuesto = await _db.Impuestos
                .FirstOrDefaultAsync(i => i.Id == id && i.UsuarioId == usuarioId);

            if (impuesto == null) return NotFound();

            // Verificar si está siendo usado en detalles de facturas
            var enUso = await _db.DetalleFacturaImpuestos
                .AnyAsync(d => d.ImpuestoId == id);

            if (enUso)
                return BadRequest(new { message = "Este impuesto está en uso en facturas existentes. Márcalo como inactivo en lugar de eliminarlo." });

            _db.Impuestos.Remove(impuesto);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // -- Helpers -------------------------------------------------------
        private async Task<bool> ValidarCuentasPUC(int usuarioId, params int?[] ids)
        {
            var idsValidos = ids.Where(i => i.HasValue).Select(i => i!.Value).Distinct().ToList();
            if (!idsValidos.Any()) return true;

            var count = await _db.CuentasContables
                .CountAsync(c => idsValidos.Contains(c.Id) && c.UsuarioId == usuarioId);
            return count == idsValidos.Count;
        }

        private static string? InferirCodigoDIAN(string tipo) => tipo switch
        {
            "IVA" => "01",
            "ICA" => "03",
            "INC" => "04",
            "Retefuente" => "05",
            "ReteICA" => "06",
            "ReteIVA" => "07",
            _ => null
        };

        private static ImpuestoDto MapToDto(Impuesto i) => new()
        {
            Id = i.Id,
            Codigo = i.Codigo,
            Nombre = i.Nombre,
            TipoImpuesto = i.TipoImpuesto,
            Tarifa = i.Tarifa,
            PorValor = i.PorValor,
            TarifaDisplay = i.TarifaDisplay,
            CodigoTributoDIAN = i.CodigoTributoDIAN,
            EnUso = i.EnUso,
            CuentaDebitoVentas = MapCuentaResumen(i.CuentaDebitoVentas),
            CuentaCreditoVentas = MapCuentaResumen(i.CuentaCreditoVentas),
            CuentaDebitoCompras = MapCuentaResumen(i.CuentaDebitoCompras),
            CuentaCreditoCompras = MapCuentaResumen(i.CuentaCreditoCompras),
            CuentaDevolucionVentas = MapCuentaResumen(i.CuentaDevolucionVentas),
            CuentaDevolucionCompras = MapCuentaResumen(i.CuentaDevolucionCompras)
        };

        private static CuentaContableResumenDto? MapCuentaResumen(CuentaContable? c) =>
            c == null ? null : new() { Id = c.Id, Codigo = c.Codigo, Nombre = c.Nombre, CodigoNombre = c.CodigoNombre };
    }
}
