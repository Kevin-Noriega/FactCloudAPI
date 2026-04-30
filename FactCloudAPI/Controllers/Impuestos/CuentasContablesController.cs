using NubeeAPI.Data;
using NubeeAPI.DTOs;
using NubeeAPI.DTOs.Impuestos;
using NubeeAPI.Models;
using NubeeAPI.Models.Impuestos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace NubeeAPI.Controllers.Impuestos
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CuentasContablesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CuentasContablesController(ApplicationDbContext db)
        {
            _db = db;
        }

        private int GetUsuarioId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // -- GET /api/cuentascontables -------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? clase,
            [FromQuery] int? nivel,
            [FromQuery] string? buscar,
            [FromQuery] bool? soloActivas,
            [FromQuery] bool? soloMovimiento)
        {
            var usuarioId = GetUsuarioId();

            // ? CAMBIO 1: incluye cuentas globales (NULL) + las del usuario
            var query = _db.CuentasContables
                .Where(c => c.UsuarioId == null || c.UsuarioId == usuarioId)
                .AsQueryable();

            if (clase.HasValue)
                query = query.Where(c => c.ClasePUC == clase.Value);

            if (nivel.HasValue)
                query = query.Where(c => c.Nivel == nivel.Value);

            if (soloActivas == true)
                query = query.Where(c => c.Activa);

            if (soloMovimiento == true)
                query = query.Where(c => c.PermiteMovimiento);

            if (!string.IsNullOrWhiteSpace(buscar))
                query = query.Where(c =>
                    c.Codigo.Contains(buscar) ||
                    c.Nombre.ToLower().Contains(buscar.ToLower()));

            var cuentas = await query
                .OrderBy(c => c.Codigo)
                .Select(c => new CuentaContableDto
                {
                    Id = c.Id,
                    Codigo = c.Codigo,
                    Nombre = c.Nombre,
                    CodigoNombre = c.CodigoNombre,
                    Descripcion = c.Descripcion,
                    Nivel = c.Nivel,
                    CodigoPadre = c.CodigoPadre,
                    ClasePUC = c.ClasePUC,
                    NombreClase = c.NombreClase,
                    Naturaleza = c.Naturaleza,
                    TipoAjuste = c.TipoAjuste,
                    PermiteMovimiento = c.PermiteMovimiento,
                    RequiereTercero = c.RequiereTercero,
                    RequiereCentroCosto = c.RequiereCentroCosto,
                    RequiereDocumento = c.RequiereDocumento,
                    Activa = c.Activa
                })
                .ToListAsync();

            return Ok(cuentas);
        }

        // -- GET /api/cuentascontables/{id} --------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuarioId = GetUsuarioId();

            // ? CAMBIO 2: permite leer cuentas globales (NULL) o propias
            var cuenta = await _db.CuentasContables
                .FirstOrDefaultAsync(c => c.Id == id &&
                    (c.UsuarioId == null || c.UsuarioId == usuarioId));

            if (cuenta == null) return NotFound();

            return Ok(MapToDto(cuenta));
        }

        // -- GET /api/cuentascontables/buscar?q=caja -----------------------
        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest("El parámetro 'q' es obligatorio.");

            var usuarioId = GetUsuarioId();

            // ? CAMBIO 3: incluye cuentas globales (NULL) + las del usuario
            var resultados = await _db.CuentasContables
                .Where(c => (c.UsuarioId == null || c.UsuarioId == usuarioId) &&
                            c.Activa &&
                            (c.Codigo.StartsWith(q) ||
                             c.Nombre.ToLower().Contains(q.ToLower())))
                .OrderBy(c => c.Codigo)
                .Take(20)
                .Select(c => new CuentaContableResumenDto
                {
                    Id = c.Id,
                    Codigo = c.Codigo,
                    Nombre = c.Nombre,
                    CodigoNombre = c.CodigoNombre
                })
                .ToListAsync();

            return Ok(resultados);
        }

        // -- POST /api/cuentascontables ------------------------------------
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CrearCuentaContableDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuarioId = GetUsuarioId();

            // Verifica duplicado tanto en globales como en las propias del usuario
            var existe = await _db.CuentasContables
                .AnyAsync(c => (c.UsuarioId == null || c.UsuarioId == usuarioId)
                               && c.Codigo == dto.Codigo);
            if (existe)
                return Conflict(new { message = $"Ya existe una cuenta con el código {dto.Codigo}." });

            var nivel = dto.Nivel ?? CalcularNivel(dto.Codigo);
            var clase = dto.ClasePUC ?? int.Parse(dto.Codigo[..1]);

            var cuenta = new CuentaContable
            {
                UsuarioId = usuarioId,
                Codigo = dto.Codigo,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Nivel = nivel,
                CodigoPadre = dto.CodigoPadre ?? InferirCodigoPadre(dto.Codigo),
                ClasePUC = clase,
                Naturaleza = dto.Naturaleza,
                TipoAjuste = dto.TipoAjuste,
                PermiteMovimiento = dto.PermiteMovimiento,
                RequiereTercero = dto.RequiereTercero,
                RequiereCentroCosto = dto.RequiereCentroCosto,
                RequiereDocumento = dto.RequiereDocumento
            };

            _db.CuentasContables.Add(cuenta);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = cuenta.Id }, MapToDto(cuenta));
        }

        // -- PUT /api/cuentascontables/{id} --------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActualizarCuentaContableDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuarioId = GetUsuarioId();

            // Solo permite editar cuentas propias (no las globales del PUC)
            var cuenta = await _db.CuentasContables
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == usuarioId);

            if (cuenta == null) return NotFound();

            cuenta.Nombre = dto.Nombre;
            cuenta.Descripcion = dto.Descripcion;
            if (dto.Naturaleza != null) cuenta.Naturaleza = dto.Naturaleza;
            if (dto.TipoAjuste != null) cuenta.TipoAjuste = dto.TipoAjuste;
            if (dto.PermiteMovimiento.HasValue) cuenta.PermiteMovimiento = dto.PermiteMovimiento.Value;
            if (dto.RequiereTercero.HasValue) cuenta.RequiereTercero = dto.RequiereTercero.Value;
            if (dto.RequiereCentroCosto.HasValue) cuenta.RequiereCentroCosto = dto.RequiereCentroCosto.Value;
            if (dto.RequiereDocumento.HasValue) cuenta.RequiereDocumento = dto.RequiereDocumento.Value;
            if (dto.Activa.HasValue) cuenta.Activa = dto.Activa.Value;

            await _db.SaveChangesAsync();
            return Ok(MapToDto(cuenta));
        }

        // -- DELETE /api/cuentascontables/{id} -----------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioId = GetUsuarioId();

            // Solo permite eliminar cuentas propias (no las globales del PUC)
            var cuenta = await _db.CuentasContables
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == usuarioId);

            if (cuenta == null) return NotFound();

            var enUso = await _db.Impuestos.AnyAsync(i =>
                i.UsuarioId == usuarioId && (
                    i.CuentaDebitoVentasId == id ||
                    i.CuentaCreditoVentasId == id ||
                    i.CuentaDebitoComprasId == id ||
                    i.CuentaCreditoComprasId == id));

            if (enUso)
                return BadRequest(new { message = "Esta cuenta está en uso por uno o más impuestos. Desactívala en lugar de eliminarla." });

            _db.CuentasContables.Remove(cuenta);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // -- Helpers -------------------------------------------------------
        private static int CalcularNivel(string codigo) => codigo.Length switch
        {
            1 => 1,
            2 => 2,
            4 => 3,
            6 => 4,
            _ => 5
        };

        private static string? InferirCodigoPadre(string codigo) => codigo.Length switch
        {
            1 => null,
            2 => codigo[..1],
            4 => codigo[..2],
            6 => codigo[..4],
            _ => codigo[..6]
        };

        private static CuentaContableDto MapToDto(CuentaContable c) => new()
        {
            Id = c.Id,
            Codigo = c.Codigo,
            Nombre = c.Nombre,
            CodigoNombre = c.CodigoNombre,
            Descripcion = c.Descripcion,
            Nivel = c.Nivel,
            CodigoPadre = c.CodigoPadre,
            ClasePUC = c.ClasePUC,
            NombreClase = c.NombreClase,
            Naturaleza = c.Naturaleza,
            TipoAjuste = c.TipoAjuste,
            PermiteMovimiento = c.PermiteMovimiento,
            RequiereTercero = c.RequiereTercero,
            RequiereCentroCosto = c.RequiereCentroCosto,
            RequiereDocumento = c.RequiereDocumento,
            Activa = c.Activa
        };
    }
}
