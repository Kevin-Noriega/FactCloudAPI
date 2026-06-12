using NubeeAPI.Data;
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

        // -- Helper tenant seguro -----------------------------------------
        private int GetUsuarioId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(claim) || !int.TryParse(claim, out var id))
                throw new UnauthorizedAccessException("Token inválido.");
            return id;
        }

        // -- GET /api/cuentascontables  (lista plana con filtros) ----------
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? clase,
            [FromQuery] int? nivel,
            [FromQuery] string? buscar,
            [FromQuery] bool? soloActivas,
            [FromQuery] bool? soloMovimiento,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamano = 100)
        {
            var usuarioId = GetUsuarioId();

            var query = _db.CuentasContables
                .Where(c => c.UsuarioId == null || c.UsuarioId == usuarioId)
                .AsQueryable();

            if (clase.HasValue) query = query.Where(c => c.ClasePUC == clase.Value);
            if (nivel.HasValue) query = query.Where(c => c.Nivel == nivel.Value);
            if (soloActivas == true) query = query.Where(c => c.Activa);
            if (soloMovimiento == true) query = query.Where(c => c.PermiteMovimiento);

            if (!string.IsNullOrWhiteSpace(buscar))
                query = query.Where(c =>
                    c.Codigo.Contains(buscar) ||
                    EF.Functions.Like(c.Nombre.ToLower(), $"%{buscar.ToLower()}%"));

            var total = await query.CountAsync();

            var cuentas = await query
                .OrderBy(c => c.Codigo)
                .Skip((pagina - 1) * tamano)
                .Take(tamano)
                .Select(c => MapToDto(c))
                .ToListAsync();

            return Ok(new { items = cuentas, total, pagina, tamano });
        }

        // -- GET /api/cuentascontables/arbol  (árbol jerárquico completo) --
        [HttpGet("arbol")]
        public async Task<IActionResult> GetArbol([FromQuery] int? clase)
        {
            var usuarioId = GetUsuarioId();

            var query = _db.CuentasContables
                .Where(c => c.UsuarioId == null || c.UsuarioId == usuarioId)
                .AsQueryable();

            if (clase.HasValue)
                query = query.Where(c => c.ClasePUC == clase.Value);

            var todas = await query
                .OrderBy(c => c.Codigo)
                .Select(c => MapToDto(c))
                .ToListAsync();

            var arbol = ConstruirArbol(todas);
            return Ok(arbol);
        }

        // -- GET /api/cuentascontables/{id} --------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuarioId = GetUsuarioId();
            var cuenta = await _db.CuentasContables
                .FirstOrDefaultAsync(c => c.Id == id &&
                    (c.UsuarioId == null || c.UsuarioId == usuarioId));

            if (cuenta == null) return NotFound(new { message = "Cuenta no encontrada." });
            return Ok(MapToDto(cuenta));
        }

        // -- GET /api/cuentascontables/buscar?q=caja -----------------------
        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string q, [FromQuery] int limite = 20)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest(new { message = "El parámetro 'q' es obligatorio." });

            var usuarioId = GetUsuarioId();

            var resultados = await _db.CuentasContables
                .Where(c => (c.UsuarioId == null || c.UsuarioId == usuarioId) &&
                             c.Activa &&
                            (c.Codigo.StartsWith(q) ||
                             EF.Functions.Like(c.Nombre.ToLower(), $"%{q.ToLower()}%")))
                .OrderBy(c => c.Codigo)
                .Take(limite)
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
        // Crea Clase, Grupo, Cuenta, Subcuenta o Auxiliar
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CrearCuentaContableDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuarioId = GetUsuarioId();

            // Validar código no vacío
            if (string.IsNullOrWhiteSpace(dto.Codigo))
                return BadRequest(new { message = "El código es obligatorio." });

            // Verificar duplicado (globales + propias)
            var existe = await _db.CuentasContables
                .AnyAsync(c => (c.UsuarioId == null || c.UsuarioId == usuarioId)
                               && c.Codigo == dto.Codigo);
            if (existe)
                return Conflict(new { message = $"Ya existe una cuenta con el código '{dto.Codigo}'." });

            // Calcular nivel y clase automáticamente si no vienen
            var nivel = dto.Nivel ?? CalcularNivel(dto.Codigo);
            var clase = dto.ClasePUC ?? InferirClase(dto.Codigo);
            var padre = dto.CodigoPadre ?? InferirCodigoPadre(dto.Codigo);
            var nat = dto.Naturaleza ?? InferirNaturaleza(clase);

            // Validar que el padre exista (salvo que sea nivel 1)
            if (nivel > 1 && !string.IsNullOrEmpty(padre))
            {
                var padreExiste = await _db.CuentasContables
                    .AnyAsync(c => (c.UsuarioId == null || c.UsuarioId == usuarioId)
                                   && c.Codigo == padre);
                if (!padreExiste)
                    return BadRequest(new
                    {
                        message = $"No existe la cuenta padre con código '{padre}'. " +
                                  "Debe crear primero la cuenta padre."
                    });
            }

            var cuenta = new CuentaContable
            {
                UsuarioId = usuarioId,
                Codigo = dto.Codigo.Trim(),
                Nombre = dto.Nombre.Trim(),
                Descripcion = dto.Descripcion,
                Nivel = nivel,
                CodigoPadre = padre,
                ClasePUC = clase,
                Naturaleza = nat,
                TipoAjuste = dto.TipoAjuste ?? "M",
                PermiteMovimiento = dto.PermiteMovimiento,
                RequiereTercero = dto.RequiereTercero,
                RequiereCentroCosto = dto.RequiereCentroCosto,
                RequiereDocumento = dto.RequiereDocumento,
                Activa = true,
                FechaCreacion = DateTime.UtcNow
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

            // Solo edita cuentas propias, no globales del PUC
            var cuenta = await _db.CuentasContables
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == usuarioId);

            if (cuenta == null)
                return NotFound(new { message = "Cuenta no encontrada o no tienes permiso para editarla." });

            if (!string.IsNullOrWhiteSpace(dto.Nombre)) cuenta.Nombre = dto.Nombre.Trim();
            if (dto.Descripcion != null) cuenta.Descripcion = dto.Descripcion;
            if (!string.IsNullOrWhiteSpace(dto.Naturaleza)) cuenta.Naturaleza = dto.Naturaleza;
            if (!string.IsNullOrWhiteSpace(dto.TipoAjuste)) cuenta.TipoAjuste = dto.TipoAjuste;
            if (dto.PermiteMovimiento.HasValue) cuenta.PermiteMovimiento = dto.PermiteMovimiento.Value;
            if (dto.RequiereTercero.HasValue) cuenta.RequiereTercero = dto.RequiereTercero.Value;
            if (dto.RequiereCentroCosto.HasValue) cuenta.RequiereCentroCosto = dto.RequiereCentroCosto.Value;
            if (dto.RequiereDocumento.HasValue) cuenta.RequiereDocumento = dto.RequiereDocumento.Value;
            if (dto.Activa.HasValue) cuenta.Activa = dto.Activa.Value;

            await _db.SaveChangesAsync();
            return Ok(MapToDto(cuenta));
        }

        // -- PATCH /api/cuentascontables/{id}/activar ----------------------
        [HttpPatch("{id}/activar")]
        public async Task<IActionResult> ToggleActiva(int id, [FromBody] bool activa)
        {
            var usuarioId = GetUsuarioId();
            var cuenta = await _db.CuentasContables
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == usuarioId);

            if (cuenta == null) return NotFound();

            cuenta.Activa = activa;
            await _db.SaveChangesAsync();
            return Ok(new { id, activa });
        }

        // -- DELETE /api/cuentascontables/{id} -----------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioId = GetUsuarioId();

            var cuenta = await _db.CuentasContables
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == usuarioId);

            if (cuenta == null)
                return NotFound(new { message = "Cuenta no encontrada o no es una cuenta propia." });

            // Verificar hijos
            var tieneHijos = await _db.CuentasContables
                .AnyAsync(c => c.CodigoPadre == cuenta.Codigo &&
                               (c.UsuarioId == null || c.UsuarioId == usuarioId));
            if (tieneHijos)
                return BadRequest(new
                {
                    message = "Esta cuenta tiene subcuentas. Elimina primero las subcuentas."
                });

            // Verificar uso en impuestos
            var enUsoImpuesto = await _db.Impuestos.AnyAsync(i =>
                i.CuentaDebitoVentasId == id || i.CuentaCreditoVentasId == id ||
                i.CuentaDebitoComprasId == id || i.CuentaCreditoComprasId == id ||
                i.CuentaDevolucionVentasId == id || i.CuentaDevolucionComprasId == id);

            if (enUsoImpuesto)
                return BadRequest(new
                {
                    message = "Esta cuenta está en uso en impuestos. Desactívala en lugar de eliminarla."
                });

            // Soft delete: marcar inactiva (más seguro que eliminar físicamente)
            cuenta.Activa = false;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // ----------------------------------------------------------------
        // HELPERS PRIVADOS
        // ----------------------------------------------------------------

        private static int CalcularNivel(string codigo) => codigo.Trim().Length switch
        {
            1 => 1,   // Clase:     1
            2 => 2,   // Grupo:     11
            4 => 3,   // Cuenta:    1105
            6 => 4,   // Subcuenta: 110505
            _ => 5    // Auxiliar:  11050501+
        };

        private static int InferirClase(string codigo)
        {
            if (string.IsNullOrEmpty(codigo)) return 1;
            return int.TryParse(codigo[..1], out var c) ? c : 1;
        }

        private static string? InferirCodigoPadre(string codigo) => codigo.Trim().Length switch
        {
            1 => null,
            2 => codigo[..1],
            4 => codigo[..2],
            6 => codigo[..4],
            _ => codigo[..6]
        };

        private static string InferirNaturaleza(int clase) => clase switch
        {
            1 or 5 or 6 or 7 or 8 => "D",  // Activo, Gastos, Costos ? Débito
            _ => "C"                         // Pasivo, Patrimonio, Ingresos ? Crédito
        };

        // Construye árbol jerárquico desde lista plana
        private static List<CuentaContableArbolDto> ConstruirArbol(
            List<CuentaContableDto> todas)
        {
            var mapa = todas.ToDictionary(c => c.Codigo);
            var raices = new List<CuentaContableArbolDto>();

            var nodos = todas.ToDictionary(
                c => c.Codigo,
                c => new CuentaContableArbolDto
                {
                    Id = c.Id,
                    Codigo = c.Codigo,
                    Nombre = c.Nombre,
                    CodigoNombre = c.CodigoNombre,
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
                    Activa = c.Activa,
                    Hijos = new List<CuentaContableArbolDto>()
                });

            foreach (var nodo in nodos.Values)
            {
                if (nodo.CodigoPadre != null && nodos.TryGetValue(nodo.CodigoPadre, out var padre))
                    padre.Hijos.Add(nodo);
                else
                    raices.Add(nodo);
            }

            return raices;
        }

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
