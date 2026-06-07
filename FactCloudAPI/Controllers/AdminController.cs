using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NubeeAPI.Data;
using NubeeAPI.DTOs.Admin;
using NubeeAPI.Models;
using NubeeAPI.Models.Planes;
using NubeeAPI.Models.Usuarios;
using NubeeAPI.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using static NubeeAPI.Models.Factura;

namespace NubeeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;
        private readonly ConfiguracionService _config;

        public AdminController(ApplicationDbContext context, ILogger<AdminController> logger, ConfiguracionService config)
        {
            _context = context;
            _logger = logger;
            _config = config;
        }

        // ════════════════════════════════════════════════════════════
        // USUARIOS
        // ════════════════════════════════════════════════════════════

        /// <summary>GET /api/Admin/usuarios — Lista completa de usuarios del sistema.</summary>
        [HttpGet("usuarios")]
        public async Task<ActionResult<IEnumerable<UsuarioAdminDto>>> GetUsuarios(
            [FromQuery] string? busqueda,
            [FromQuery] string? rol,
            [FromQuery] bool? estado)
        {
            var query = _context.Usuarios
                .Include(u => u.Negocio)
                .Include(u => u.Suscripciones.Where(s => s.Activa))
                    .ThenInclude(s => s.PlanFacturacion)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                var q = busqueda.ToLower();
                query = query.Where(u =>
                    u.Nombre.ToLower().Contains(q) ||
                    (u.Apellido != null && u.Apellido.ToLower().Contains(q)) ||
                    u.Correo.ToLower().Contains(q));
            }

            if (!string.IsNullOrWhiteSpace(rol))
                query = query.Where(u => u.Rol == rol.ToLower());

            if (estado.HasValue)
                query = query.Where(u => u.Estado == estado.Value);

            var usuarios = await query
                .OrderByDescending(u => u.FechaRegistro)
                .Select(u => new UsuarioAdminDto
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Correo = u.Correo,
                    Telefono = u.Telefono,
                    Estado = u.Estado,
                    Rol = u.Rol ?? "usuario",
                    FechaRegistro = u.FechaRegistro,
                    FechaDesactivacion = u.FechaDesactivacion,
                    NombreNegocio = u.Negocio != null ? u.Negocio.NombreNegocio : null,
                    PlanNombre = u.Suscripciones
                        .Where(s => s.Activa)
                        .Select(s => s.PlanFacturacion.Nombre)
                        .FirstOrDefault(),
                    TieneSuscripcionActiva = u.Suscripciones.Any(s => s.Activa && (s.FechaFin == null || s.FechaFin > DateTime.UtcNow))
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        /// <summary>GET /api/Admin/usuarios/{id} — Detalle completo de un usuario.</summary>
        [HttpGet("usuarios/{id:int}")]
        public async Task<ActionResult> GetUsuarioDetalle(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Negocio)
                .Include(u => u.Suscripciones)
                    .ThenInclude(s => s.PlanFacturacion)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario is null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            var suscripcionActiva = usuario.Suscripciones
                .FirstOrDefault(s => s.Activa && (s.FechaFin == null || s.FechaFin > DateTime.UtcNow));

            return Ok(new
            {
                usuario = new
                {
                    id = usuario.Id,
                    nombre = usuario.Nombre,
                    apellido = usuario.Apellido,
                    correo = usuario.Correo,
                    telefono = usuario.Telefono,
                    estado = usuario.Estado,
                    rol = usuario.Rol ?? "usuario",
                    fechaRegistro = usuario.FechaRegistro,
                    fechaDesactivacion = usuario.FechaDesactivacion,
                    tipoIdentificacion = usuario.TipoIdentificacion,
                    numeroIdentificacion = usuario.NumeroIdentificacion
                },
                negocio = usuario.Negocio != null ? new
                {
                    id = usuario.Negocio.Id,
                    nombreNegocio = usuario.Negocio.NombreNegocio,
                    nit = usuario.Negocio.Nit,
                    ciudad = usuario.Negocio.Ciudad,
                    departamento = usuario.Negocio.Departamento
                } : null,
                suscripcion = suscripcionActiva != null ? new
                {
                    plan = suscripcionActiva.PlanFacturacion?.Nombre,
                    documentosUsados = suscripcionActiva.DocumentosUsados,
                    documentosIncluidos = suscripcionActiva.PlanFacturacion?.LimiteDocumentosAnuales,
                    fechaExpiracion = suscripcionActiva.FechaFin,
                    activa = suscripcionActiva.Activa
                } : null
            });
        }

        /// <summary>POST /api/Admin/usuarios — Crear usuario directamente como admin.</summary>
        [HttpPost("usuarios")]
        public async Task<ActionResult> CrearUsuario([FromBody] CrearUsuarioAdminDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _context.Usuarios.AnyAsync(u => u.Correo == dto.Correo))
                return BadRequest(new { mensaje = "El correo ya está registrado" });

            var rolValido = new[] { "usuario", "admin" };
            if (!rolValido.Contains(dto.Rol.ToLower()))
                return BadRequest(new { mensaje = "Rol inválido. Use: usuario o admin" });

            var usuario = new Usuario
            {
                Nombre = dto.Nombre.Trim(),
                Apellido = dto.Apellido?.Trim(),
                Correo = dto.Correo.Trim().ToLower(),
                ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena),
                Telefono = dto.Telefono,
                Rol = dto.Rol.ToLower(),
                Estado = dto.Estado,
                FechaRegistro = DateTime.UtcNow
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Crear negocio vacío para que el sistema funcione
            var negocio = new Negocio
            {
                UsuarioId = usuario.Id,
                NombreNegocio = $"Negocio de {usuario.Nombre}",
                Pais = "CO"
            };
            _context.Negocios.Add(negocio);
            await _context.SaveChangesAsync();

            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            _logger.LogInformation("Admin {AdminId} creó usuario {UsuarioId} ({Correo}) con rol {Rol}",
                adminId, usuario.Id, usuario.Correo, usuario.Rol);

            return StatusCode(201, new
            {
                mensaje = "Usuario creado correctamente",
                id = usuario.Id,
                correo = usuario.Correo,
                rol = usuario.Rol
            });
        }

        /// <summary>PATCH /api/Admin/usuarios/{id} — Editar datos de un usuario.</summary>
        [HttpPatch("usuarios/{id:int}")]
        public async Task<ActionResult> EditarUsuario(int id, [FromBody] EditarUsuarioAdminDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario is null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            // Verificar correo duplicado si cambia
            if (!string.IsNullOrWhiteSpace(dto.Correo) &&
                dto.Correo != usuario.Correo &&
                await _context.Usuarios.AnyAsync(u => u.Correo == dto.Correo && u.Id != id))
            {
                return BadRequest(new { mensaje = "El correo ya está en uso por otro usuario" });
            }

            if (!string.IsNullOrWhiteSpace(dto.Nombre)) usuario.Nombre = dto.Nombre.Trim();
            if (!string.IsNullOrWhiteSpace(dto.Apellido)) usuario.Apellido = dto.Apellido.Trim();
            if (!string.IsNullOrWhiteSpace(dto.Correo)) usuario.Correo = dto.Correo.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(dto.Telefono)) usuario.Telefono = dto.Telefono;
            if (dto.Estado.HasValue) usuario.Estado = dto.Estado.Value;

            if (!string.IsNullOrWhiteSpace(dto.Rol))
            {
                var rolValido = new[] { "usuario", "admin" };
                if (!rolValido.Contains(dto.Rol.ToLower()))
                    return BadRequest(new { mensaje = "Rol inválido. Use: usuario o admin" });
                usuario.Rol = dto.Rol.ToLower();
            }

            if (!string.IsNullOrWhiteSpace(dto.Contrasena))
                usuario.ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena);

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Usuario actualizado correctamente", id = usuario.Id });
        }

        /// <summary>DELETE /api/Admin/usuarios/{id} — Eliminar usuario del sistema.</summary>
        [HttpDelete("usuarios/{id:int}")]
        public async Task<ActionResult> EliminarUsuario(int id)
        {
            // Prevenir que el admin se elimine a sí mismo
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (id == adminId)
                return BadRequest(new { mensaje = "No puedes eliminar tu propia cuenta" });

            var usuario = await _context.Usuarios
                .Include(u => u.Negocio)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario is null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            _logger.LogWarning("Admin {AdminId} eliminó usuario {UsuarioId} ({Correo})",
                adminId, id, usuario.Correo);

            return Ok(new { mensaje = $"Usuario {usuario.Correo} eliminado correctamente" });
        }

        /// <summary>PATCH /api/Admin/usuarios/{id}/rol — Cambiar rol de un usuario.</summary>
        [HttpPatch("usuarios/{id:int}/rol")]
        public async Task<ActionResult> CambiarRol(int id, [FromBody] CambiarRolDto dto)
        {
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (id == adminId)
                return BadRequest(new { mensaje = "No puedes cambiar tu propio rol" });

            var rolValido = new[] { "usuario", "admin" };
            if (!rolValido.Contains(dto.Rol.ToLower()))
                return BadRequest(new { mensaje = "Rol inválido. Use: usuario o admin" });

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario is null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            var rolAnterior = usuario.Rol;
            usuario.Rol = dto.Rol.ToLower();
            await _context.SaveChangesAsync();

            _logger.LogInformation("Admin {AdminId} cambió rol de usuario {UsuarioId} de '{RolAnterior}' a '{RolNuevo}'",
                adminId, id, rolAnterior, usuario.Rol);

            return Ok(new
            {
                mensaje = $"Rol actualizado de '{rolAnterior}' a '{usuario.Rol}'",
                id = usuario.Id,
                correo = usuario.Correo,
                rol = usuario.Rol
            });
        }

        /// <summary>PATCH /api/Admin/usuarios/{id}/estado — Activar o desactivar cuenta.</summary>
        [HttpPatch("usuarios/{id:int}/estado")]
        public async Task<ActionResult> CambiarEstado(int id, [FromBody] EstadoAdminDto dto)
        {
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (id == adminId)
                return BadRequest(new { mensaje = "No puedes desactivar tu propia cuenta" });

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario is null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            usuario.Estado = dto.Estado;
            usuario.FechaDesactivacion = dto.Estado ? null : DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = dto.Estado ? "Cuenta activada" : "Cuenta desactivada",
                estado = usuario.Estado,
                fechaDesactivacion = usuario.FechaDesactivacion
            });
        }

        /// <summary>POST /api/Admin/usuarios/{id}/reset-password — Resetear contraseña.</summary>
        [HttpPost("usuarios/{id:int}/reset-password")]
        public async Task<ActionResult> ResetPassword(int id, [FromBody] ResetPasswordAdminDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario is null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            usuario.ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(dto.NuevaContrasena);
            await _context.SaveChangesAsync();

            // Revocar todos los refresh tokens del usuario (forzar re-login)
            var tokens = await _context.RefreshTokens
                .Where(rt => rt.UsuarioId == id && !rt.Revocado)
                .ToListAsync();
            tokens.ForEach(t => t.Revocado = true);
            await _context.SaveChangesAsync();

            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            _logger.LogWarning("Admin {AdminId} reseteó contraseña de usuario {UsuarioId} y revocó {TokenCount} tokens",
                adminId, id, tokens.Count);

            return Ok(new { mensaje = "Contraseña reseteada. El usuario deberá iniciar sesión de nuevo." });
        }

        // ════════════════════════════════════════════════════════════
        // CLIENTES — Admin ve TODOS
        // ════════════════════════════════════════════════════════════

        /// <summary>GET /api/Admin/clientes — Listar todos los clientes del sistema.</summary>
        [HttpGet("clientes")]
        public async Task<ActionResult> GetClientes([FromQuery] string? busqueda)
        {
            var query = _context.Clientes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                var q = busqueda.ToLower();
                query = query.Where(c =>
                    c.Nombre.ToLower().Contains(q) ||
                    (c.Correo != null && c.Correo.ToLower().Contains(q)) ||
                    (c.NumeroIdentificacion != null && c.NumeroIdentificacion.Contains(q)));
            }

            var clientes = await query
                .OrderByDescending(c => c.Id)
                .Select(c => new
                {
                    c.Id,
                    c.Nombre,
                    c.Correo,
                    Telefono = c.TelefonoFacturacion,
                    Nit = c.NumeroIdentificacion,
                    c.Direccion,
                    c.Ciudad,
                    c.UsuarioId
                })
                .ToListAsync();

            return Ok(clientes);
        }

        // ════════════════════════════════════════════════════════════
        // FACTURAS — Admin ve TODAS
        // ════════════════════════════════════════════════════════════

        /// <summary>GET /api/Admin/facturas — Listar todas las facturas del sistema.</summary>
        [HttpGet("facturas")]
        public async Task<ActionResult> GetFacturas(
            [FromQuery] string? estado,
            [FromQuery] int? usuarioId)
        {
            // LEFT JOIN con Clientes en una sola query (evita N+1)
            var query =
                from f in _context.Facturas
                join c in _context.Clientes on f.ClienteId equals c.Id into clientes
                from c in clientes.DefaultIfEmpty()
                select new { f, NombreCliente = c != null ? c.Nombre : null };

            if (!string.IsNullOrWhiteSpace(estado))
                if (!string.IsNullOrWhiteSpace(estado))
                {
                    if (!Enum.TryParse<EstadoFactura>(estado, ignoreCase: true, out var estadoEnum))
                        return BadRequest(new { mensaje = $"Estado inválido. Use: {string.Join(", ", Enum.GetNames<EstadoFactura>())}" });

                    query = query.Where(x => x.f.Estado == estadoEnum);
                }

            if (usuarioId.HasValue)
                query = query.Where(x => x.f.UsuarioId == usuarioId.Value);

            var facturas = await query
                .OrderByDescending(x => x.f.FechaEmision)
                .Select(x => new
                {
                    x.f.Id,
                    x.f.NumeroFactura,
                    x.f.Prefijo,
                    x.f.FechaEmision,
                    x.f.Estado,
                    x.f.TotalFactura,
                    x.f.Subtotal,
                    x.f.TotalIVA,
                    x.f.TotalDescuentos,
                    x.f.FormaPago,
                    x.f.ClienteId,
                    x.f.UsuarioId,
                    NombreCliente = x.NombreCliente
                })
                .ToListAsync();

            return Ok(facturas);
        }

        /// <summary>PATCH /api/Admin/facturas/{id}/estado — Cambiar estado de una factura.</summary>
        [HttpPatch("facturas/{id:int}/estado")]
        public async Task<ActionResult> CambiarEstadoFactura(int id, [FromBody] EstadoFacturaDto dto)
        {
            var factura = await _context.Facturas.FindAsync(id);
            if (factura is null)
                return NotFound(new { mensaje = "Factura no encontrada" });

            var estadosValidos = new[] { "Borrador", "Pendiente", "Emitida", "Pagada", "Anulada", "Cancelada" };
            if (!estadosValidos.Contains(dto.Estado))
                return BadRequest(new { mensaje = $"Estado inválido. Use: {string.Join(", ", estadosValidos)}" });

            if (!Enum.TryParse<EstadoFactura>(dto.Estado, ignoreCase: true, out var estadoEnum))
                return BadRequest(new { mensaje = $"Estado inválido. Use: {string.Join(", ", Enum.GetNames<EstadoFactura>())}" });

            var estadoAnterior = factura.Estado;
            factura.Estado = estadoEnum;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = $"Estado cambiado de '{estadoAnterior}' a '{factura.Estado}'",
                id = factura.Id,
                estado = factura.Estado
            });
        }

        /// <summary>DELETE /api/Admin/facturas/{id} — Eliminar factura.</summary>
        [HttpDelete("facturas/{id:int}")]
        public async Task<ActionResult> EliminarFactura(int id)
        {
            var factura = await _context.Facturas
                .Include(f => f.DetalleFacturas)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (factura is null)
                return NotFound(new { mensaje = "Factura no encontrada" });

            if (factura.Estado == EstadoFactura.Emitida)
                return BadRequest(new { mensaje = "No se puede eliminar una factura emitida ante la DIAN" });

            _context.Facturas.Remove(factura);
            await _context.SaveChangesAsync();

            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            _logger.LogWarning("Admin {AdminId} eliminó factura {FacturaId} (estado={Estado})",
                adminId, id, factura.Estado);

            return Ok(new { mensaje = "Factura eliminada correctamente" });
        }

        // ════════════════════════════════════════════════════════════
        // PRODUCTOS — Admin ve TODOS
        // ════════════════════════════════════════════════════════════

        /// <summary>GET /api/Admin/productos — Listar todos los productos del sistema.</summary>
        [HttpGet("productos")]
        public async Task<ActionResult> GetProductos([FromQuery] string? busqueda)
        {
            var query = _context.Productos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                var q = busqueda.ToLower();
                query = query.Where(p =>
                    p.Nombre.ToLower().Contains(q) ||
                    (p.CodigoInterno != null && p.CodigoInterno.ToLower().Contains(q)));
            }

            var productos = await query
                .OrderBy(p => p.Nombre)
                .Select(p => new
                {
                    p.Id,
                    p.Nombre,
                    p.Descripcion,
                    p.PrecioUnitario,
                    p.UnidadMedida,
                    p.ImpuestoCargo,
                    p.IncluyeIVA,
                    p.CodigoInterno,
                    p.CodigoBarras,
                    p.UsuarioId
                })
                .ToListAsync();

            return Ok(productos);
        }

        /// <summary>POST /api/Admin/productos — Crear producto asignado a un usuario.</summary>
        [HttpPost("productos")]
        public async Task<ActionResult> CrearProducto([FromBody] CrearProductoAdminDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Id == dto.UsuarioId);
            if (!usuarioExiste)
                return BadRequest(new { mensaje = "Usuario no encontrado" });

            var producto = new Producto
            {
                UsuarioId = dto.UsuarioId,
                Nombre = dto.Nombre.Trim(),
                Descripcion = dto.Descripcion,
                PrecioUnitario = dto.PrecioUnitario,
                UnidadMedida = dto.UnidadMedida ?? "UND",
                CodigoInterno = dto.CodigoInterno,
                CodigoBarras = dto.CodigoBarras,
                EsServicio = dto.EsServicio
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { mensaje = "Producto creado correctamente", id = producto.Id });
        }

        /// <summary>PATCH /api/Admin/productos/{id} — Editar producto como admin.</summary>
        [HttpPatch("productos/{id:int}")]
        public async Task<ActionResult> EditarProducto(int id, [FromBody] EditarProductoAdminDto dto)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto is null)
                return NotFound(new { mensaje = "Producto no encontrado" });

            if (!string.IsNullOrWhiteSpace(dto.Nombre)) producto.Nombre = dto.Nombre.Trim();
            if (dto.Descripcion != null) producto.Descripcion = dto.Descripcion;
            if (dto.PrecioUnitario.HasValue) producto.PrecioUnitario = dto.PrecioUnitario.Value;
            if (!string.IsNullOrWhiteSpace(dto.UnidadMedida)) producto.UnidadMedida = dto.UnidadMedida;
            if (!string.IsNullOrWhiteSpace(dto.CodigoInterno)) producto.CodigoInterno = dto.CodigoInterno;
            if (!string.IsNullOrWhiteSpace(dto.CodigoBarras)) producto.CodigoBarras = dto.CodigoBarras;

            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Producto actualizado correctamente", id = producto.Id });
        }

        /// <summary>DELETE /api/Admin/productos/{id} — Eliminar producto.</summary>
        [HttpDelete("productos/{id:int}")]
        public async Task<ActionResult> EliminarProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto is null)
                return NotFound(new { mensaje = "Producto no encontrado" });

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Producto eliminado correctamente" });
        }

        // ════════════════════════════════════════════════════════════
        // SUSCRIPCIONES — Admin gestiona TODAS
        // ════════════════════════════════════════════════════════════

        /// <summary>GET /api/Admin/suscripciones — Listar todas las suscripciones.</summary>
        [HttpGet("suscripciones")]
        public async Task<ActionResult> GetSuscripciones([FromQuery] bool? activa)
        {
            var query = _context.SuscripcionesFacturacion
                .Include(s => s.Usuario)
                .Include(s => s.PlanFacturacion)
                .AsQueryable();

            if (activa.HasValue)
                query = query.Where(s => s.Activa == activa.Value);

            var suscripciones = await query
                .OrderByDescending(s => s.FechaInicio)
                .Select(s => new
                {
                    s.Id,
                    s.UsuarioId,
                    usuarioNombre = s.Usuario != null ? $"{s.Usuario.Nombre} {s.Usuario.Apellido}" : "—",
                    usuarioCorreo = s.Usuario != null ? s.Usuario.Correo : null,
                    s.PlanFacturacionId,
                    planNombre = s.PlanFacturacion != null ? s.PlanFacturacion.Nombre : "—",
                    s.FechaInicio,
                    s.FechaFin,
                    s.DocumentosUsados,
                    s.Activa,
                    s.TransaccionId
                })
                .ToListAsync();

            return Ok(suscripciones);
        }

        /// <summary>POST /api/Admin/suscripciones — Asignar plan a usuario.</summary>
        [HttpPost("suscripciones")]
        public async Task<ActionResult> CrearSuscripcion([FromBody] CrearSuscripcionAdminDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);
            if (usuario is null) return NotFound(new { mensaje = "Usuario no encontrado" });

            var plan = await _context.PlanesFacturacion.FindAsync(dto.PlanId);
            if (plan is null) return NotFound(new { mensaje = "Plan no encontrado" });
            if (!plan.Activo) return BadRequest(new { mensaje = "El plan está inactivo" });

            // Desactivar suscripciones activas anteriores
            var suscActivas = await _context.SuscripcionesFacturacion
                .Where(s => s.UsuarioId == dto.UsuarioId && s.Activa)
                .ToListAsync();
            suscActivas.ForEach(s => s.Activa = false);

            var inicio = dto.FechaInicio ?? DateTime.UtcNow;
            var fin = dto.FechaFin ?? inicio.AddYears(1);

            var suscripcion = new NubeeAPI.Models.Suscripciones.SuscripcionFacturacion
            {
                UsuarioId = dto.UsuarioId,
                PlanFacturacionId = dto.PlanId,
                FechaInicio = inicio,
                FechaFin = fin,
                Activa = true,
                DocumentosUsados = 0
            };

            _context.SuscripcionesFacturacion.Add(suscripcion);
            await _context.SaveChangesAsync();

            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            await RegistrarAuditoria(adminId, "ASIGNAR_PLAN",
                $"Plan '{plan.Nombre}' asignado a usuario {usuario.Correo} (suscripción #{suscripcion.Id})");

            return StatusCode(201, new { mensaje = "Suscripción creada correctamente", id = suscripcion.Id });
        }

        /// <summary>PATCH /api/Admin/suscripciones/{id}/cancelar — Cancelar suscripción.</summary>
        [HttpPatch("suscripciones/{id:int}/cancelar")]
        public async Task<ActionResult> CancelarSuscripcion(int id)
        {
            var suscripcion = await _context.SuscripcionesFacturacion
                .Include(s => s.PlanFacturacion)
                .Include(s => s.Usuario)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (suscripcion is null) return NotFound(new { mensaje = "Suscripción no encontrada" });

            suscripcion.Activa = false;
            suscripcion.FechaFin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            await RegistrarAuditoria(adminId, "CANCELAR_SUSCRIPCION",
                $"Suscripción #{id} de '{suscripcion.Usuario?.Correo}' cancelada");

            return Ok(new { mensaje = "Suscripción cancelada correctamente" });
        }

        /// <summary>DELETE /api/Admin/suscripciones/{id} — Eliminar suscripción.</summary>
        [HttpDelete("suscripciones/{id:int}")]
        public async Task<ActionResult> EliminarSuscripcion(int id)
        {
            var suscripcion = await _context.SuscripcionesFacturacion.FindAsync(id);
            if (suscripcion is null) return NotFound(new { mensaje = "Suscripción no encontrada" });

            _context.SuscripcionesFacturacion.Remove(suscripcion);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Suscripción eliminada correctamente" });
        }

        // ════════════════════════════════════════════════════════════
        // ESTADÍSTICAS DEL SISTEMA
        // ════════════════════════════════════════════════════════════

        /// <summary>GET /api/Admin/estadisticas — Métricas globales del sistema.</summary>
        [HttpGet("estadisticas")]
        public async Task<ActionResult> GetEstadisticas()
        {
            var añoActual = DateTime.UtcNow.Year;
            var ahora = DateTime.UtcNow;

            // Conteos simples — secuenciales (DbContext no es thread-safe)
            var totalUsuarios        = await _context.Usuarios.CountAsync();
            var usuariosActivos      = await _context.Usuarios.CountAsync(u => u.Estado);
            var usuariosAdmin        = await _context.Usuarios.CountAsync(u => u.Rol == "admin");
            var totalClientes        = await _context.Clientes.CountAsync();
            var totalProductos       = await _context.Productos.CountAsync();
            var suscripcionesActivas = await _context.SuscripcionesFacturacion
                .CountAsync(s => s.Activa && (s.FechaFin == null || s.FechaFin > ahora));

            // Estadísticas de facturas agrupadas por estado
            var statsFacturas = await _context.Facturas
                .GroupBy(f => f.Estado)
                .Select(g => new { estado = g.Key, count = g.Count(), total = g.Sum(f => (decimal?)f.TotalFactura) ?? 0 })
                .ToListAsync();

            var totalFacturas      = statsFacturas.Sum(s => s.count);
            var totalIngresos      = statsFacturas.Sum(s => s.total);
            var facturasEmitidas = statsFacturas.Where(s => s.estado == Factura.EstadoFactura.Emitida || s.estado == Factura.EstadoFactura.Pagada).Sum(s => s.count);

            var facturasPendientes = statsFacturas.Where(s => s.estado == Factura.EstadoFactura.Pendiente || s.estado == Factura.EstadoFactura.Borrador)
                                                  .Sum(s => s.count);

            var facturasAnuladas = statsFacturas.Where(s => s.estado == Factura.EstadoFactura.Anulada
                                                            || s.estado == Factura.EstadoFactura.Cancelada)
                                                  .Sum(s => s.count);
            // Ventas por mes del año actual
            var ventasPorMes = await _context.Facturas
                .Where(f => f.FechaEmision.Year == añoActual)
                .GroupBy(f => f.FechaEmision.Month)
                .Select(g => new { mes = g.Key, total = g.Sum(f => (decimal?)f.TotalFactura) ?? 0, cantidad = g.Count() })
                .OrderBy(x => x.mes)
                .ToListAsync();

            // Suscriptores por plan
            var ventasPorPlan = await _context.SuscripcionesFacturacion
                .Where(s => s.Activa)
                .GroupBy(s => s.PlanFacturacion.Nombre)
                .Select(g => new { plan = g.Key ?? "Sin plan", cantidad = g.Count() })
                .OrderByDescending(x => x.cantidad)
                .ToListAsync();

            // Top 5 clientes por valor facturado
            var topClientes = await _context.Facturas
                .Where(f => f.ClienteId != null)
                .GroupBy(f => new { f.ClienteId })
                .Select(g => new
                {
                    clienteId = g.Key.ClienteId,
                    total = g.Sum(f => (decimal?)f.TotalFactura) ?? 0,
                    count = g.Count()
                })
                .OrderByDescending(x => x.total)
                .Take(5)
                .ToListAsync();

            // Obtener nombres de clientes
            var clienteIds = topClientes.Select(c => c.clienteId).Where(id => id != null).Cast<int>().ToList();
            var clienteNombres = await _context.Clientes
                .Where(c => clienteIds.Contains(c.Id))
                .Select(c => new { c.Id, c.Nombre })
                .ToListAsync();

            var topClientesConNombre = topClientes.Select(c => new
            {
                nombre = clienteNombres.FirstOrDefault(n => n.Id == c.clienteId)?.Nombre ?? $"Cliente #{c.clienteId}",
                total = c.total,
                count = c.count
            }).ToList();

            // Tipos de productos más usados en facturas
            var tiposProductos = await _context.DetalleFacturas
                .GroupBy(d => d.UnidadMedida ?? "UNIDAD")
                .Select(g => new { tipo = g.Key, cantidad = g.Count() })
                .OrderByDescending(x => x.cantidad)
                .Take(6)
                .ToListAsync();

            // Tipos de documentos emitidos
            var cntFacturas   = await _context.Facturas.CountAsync();
            var cntNotasC     = await _context.NotasCredito.CountAsync();
            var cntNotasD     = await _context.NotasDebito.CountAsync();
            var cntDocSoporte = await _context.DocumentosSoporte.CountAsync();

            var tiposDocumentos = new[]
            {
                new { tipo = "Facturas",          cantidad = cntFacturas },
                new { tipo = "Notas Crédito",     cantidad = cntNotasC },
                new { tipo = "Notas Débito",      cantidad = cntNotasD },
                new { tipo = "Doc. Soporte",      cantidad = cntDocSoporte }
            };

            _logger.LogInformation("Estadísticas admin generadas — {Usuarios} usuarios, {Facturas} facturas",
                totalUsuarios, totalFacturas);

            return Ok(new
            {
                usuarios = new
                {
                    total           = totalUsuarios,
                    activos         = usuariosActivos,
                    inactivos       = totalUsuarios - usuariosActivos,
                    administradores = usuariosAdmin
                },
                negocio = new
                {
                    totalClientes,
                    totalProductos,
                    suscripcionesActivas
                },
                facturacion = new
                {
                    totalFacturas,
                    emitidas   = facturasEmitidas,
                    pendientes = facturasPendientes,
                    anuladas   = facturasAnuladas,
                    totalIngresos,
                    porEstado  = statsFacturas
                },
                ventasPorMes,
                ventasPorPlan,
                topClientes = topClientesConNombre,
                tiposProductos,
                tiposDocumentos,
                generadoEn = ahora
            });
        }

        // ════════════════════════════════════════════════════════════
        // PLANES — CRUD completo
        // ════════════════════════════════════════════════════════════

        /// <summary>Valida y normaliza el tipo de plan: solo "FACTURACION" o "POS".</summary>
        private static string NormalizarTipo(string? tipo)
        {
            var t = (tipo ?? "").Trim().ToUpperInvariant();
            return t == "POS" ? "POS" : "FACTURACION";
        }

        /// <summary>GET /api/Admin/planes — Lista todos los planes (activos e inactivos).</summary>
        [HttpGet("planes")]
        public async Task<ActionResult> GetPlanes()
        {
            var planes = await _context.PlanesFacturacion
                .Include(p => p.Features)
                .OrderBy(p => p.Id)
                .Select(p => new
                {
                    id = p.Id,
                    codigo = p.Codigo,
                    nombre = p.Nombre,
                    tipo = p.Tipo,
                    descripcion = p.Descripcion,
                    precioAnual = p.PrecioAnual,
                    precioMensual = p.PrecioMensualFinal,
                    precioAnualFinal = p.PrecioAnualFinal,
                    limiteDocumentosAnuales = p.LimiteDocumentosAnuales,
                    limiteUsuarios = p.LimiteUsuarios,
                    destacado = p.Destacado,
                    incluyePOS = p.IncluyePOS,
                    incluyeInventario = p.IncluyeInventario,
                    incluyeNomina = p.IncluyeNomina,
                    incluyeContabilidad = p.IncluyeContabilidad,
                    incluyeSucursales = p.IncluyeSucursales,
                    descuentoActivo = p.DescuentoActivo,
                    descuentoPorcentaje = p.DescuentoPorcentaje,
                    activo = p.Activo,
                    caracteristicas = p.Features.Select(f => new { f.Id, f.Texto, f.Tooltip }).ToList()
                })
                .ToListAsync();

            return Ok(planes);
        }

        /// <summary>POST /api/Admin/planes — Crear nuevo plan.</summary>
        [HttpPost("planes")]
        public async Task<ActionResult> CrearPlan([FromBody] PlanAdminDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var plan = new PlanFacturacion
            {
                Codigo = dto.Codigo.ToUpper().Trim(),
                Nombre = dto.Nombre.Trim(),
                Tipo = NormalizarTipo(dto.Tipo),
                Descripcion = dto.Descripcion,
                PrecioAnual = dto.PrecioAnual,
                LimiteDocumentosAnuales = dto.LimiteDocumentosAnuales,
                LimiteUsuarios = dto.LimiteUsuarios,
                Destacado = dto.Destacado,
                DescuentoActivo = dto.DescuentoActivo,
                DescuentoPorcentaje = dto.DescuentoPorcentaje,
                Activo = dto.Activo,
                IncluyePOS = dto.IncluyePOS,
                IncluyeInventario = dto.IncluyeInventario,
                IncluyeNomina = dto.IncluyeNomina,
                IncluyeContabilidad = dto.IncluyeContabilidad,
                IncluyeSucursales = dto.IncluyeSucursales,
                Features = dto.Caracteristicas.Select(t => new PlanFeature { Texto = t }).ToList()
            };

            _context.PlanesFacturacion.Add(plan);
            await _context.SaveChangesAsync();

            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            await RegistrarAuditoria(adminId, "CREAR_PLAN", $"Plan '{plan.Nombre}' (ID={plan.Id}) creado");

            return StatusCode(201, new { mensaje = "Plan creado correctamente", id = plan.Id });
        }

        /// <summary>PUT /api/Admin/planes/{id} — Editar plan existente.</summary>
        [HttpPut("planes/{id:int}")]
        public async Task<ActionResult> EditarPlan(int id, [FromBody] PlanAdminDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var plan = await _context.PlanesFacturacion
                .Include(p => p.Features)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plan is null) return NotFound(new { mensaje = "Plan no encontrado" });

            plan.Codigo = dto.Codigo.ToUpper().Trim();
            plan.Nombre = dto.Nombre.Trim();
            plan.Tipo = NormalizarTipo(dto.Tipo);
            plan.Descripcion = dto.Descripcion;
            plan.PrecioAnual = dto.PrecioAnual;
            plan.LimiteDocumentosAnuales = dto.LimiteDocumentosAnuales;
            plan.LimiteUsuarios = dto.LimiteUsuarios;
            plan.Destacado = dto.Destacado;
            plan.DescuentoActivo = dto.DescuentoActivo;
            plan.DescuentoPorcentaje = dto.DescuentoPorcentaje;
            plan.IncluyePOS = dto.IncluyePOS;
            plan.IncluyeInventario = dto.IncluyeInventario;
            plan.IncluyeNomina = dto.IncluyeNomina;
            plan.IncluyeContabilidad = dto.IncluyeContabilidad;
            plan.IncluyeSucursales = dto.IncluyeSucursales;
            plan.Activo = dto.Activo;

            // Reemplazar características
            _context.RemoveRange(plan.Features);
            plan.Features = dto.Caracteristicas.Select(t => new PlanFeature { PlanFacturacionId = id, Texto = t }).ToList();

            await _context.SaveChangesAsync();

            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            await RegistrarAuditoria(adminId, "EDITAR_PLAN", $"Plan '{plan.Nombre}' (ID={id}) actualizado");

            return Ok(new { mensaje = "Plan actualizado correctamente" });
        }

        /// <summary>PATCH /api/Admin/planes/{id}/toggle — Activar/desactivar plan.</summary>
        [HttpPatch("planes/{id:int}/toggle")]
        public async Task<ActionResult> TogglePlan(int id, [FromBody] TogglePlanDto dto)
        {
            var plan = await _context.PlanesFacturacion.FindAsync(id);
            if (plan is null) return NotFound(new { mensaje = "Plan no encontrado" });

            plan.Activo = dto.Activo;
            await _context.SaveChangesAsync();

            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            await RegistrarAuditoria(adminId, dto.Activo ? "ACTIVAR_PLAN" : "DESACTIVAR_PLAN",
                $"Plan '{plan.Nombre}' (ID={id}) {(dto.Activo ? "activado" : "desactivado")}");

            return Ok(new { mensaje = $"Plan {(dto.Activo ? "activado" : "desactivado")} correctamente", activo = plan.Activo });
        }

        /// <summary>DELETE /api/Admin/planes/{id} — Eliminar plan (solo si no tiene suscripciones activas).</summary>
        [HttpDelete("planes/{id:int}")]
        public async Task<ActionResult> EliminarPlan(int id)
        {
            var plan = await _context.PlanesFacturacion
                .Include(p => p.Suscripciones)
                .Include(p => p.Features)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plan is null) return NotFound(new { mensaje = "Plan no encontrado" });

            if (plan.Suscripciones.Any(s => s.Activa))
                return BadRequest(new { mensaje = "No se puede eliminar un plan con suscripciones activas" });

            _context.PlanesFacturacion.Remove(plan);
            await _context.SaveChangesAsync();

            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            await RegistrarAuditoria(adminId, "ELIMINAR_PLAN", $"Plan '{plan.Nombre}' (ID={id}) eliminado");

            return Ok(new { mensaje = "Plan eliminado correctamente" });
        }

        // ════════════════════════════════════════════════════════════
        // AUDITORÍA
        // ════════════════════════════════════════════════════════════

        /// <summary>GET /api/Admin/auditoria — Historial de acciones admin.</summary>
        [HttpGet("auditoria")]
        public async Task<ActionResult> GetAuditoria([FromQuery] int pagina = 1, [FromQuery] int tamano = 20)
        {
            pagina = Math.Max(1, pagina);
            tamano = Math.Clamp(tamano, 5, 100);

            var query = _context.AuditoriaAdmin
                .Include(a => a.Admin)
                .OrderByDescending(a => a.FechaHora);

            var total = await query.CountAsync();
            var registros = await query
                .Skip((pagina - 1) * tamano)
                .Take(tamano)
                .Select(a => new
                {
                    a.Id,
                    adminNombre = a.Admin != null ? $"{a.Admin.Nombre} {a.Admin.Apellido}" : "Sistema",
                    a.AdminId,
                    a.Accion,
                    a.Detalle,
                    a.FechaHora
                })
                .ToListAsync();

            return Ok(new { total, pagina, tamano, registros });
        }

        // ════════════════════════════════════════════════════════════
        // CONFIGURACIÓN DEL SISTEMA
        // ════════════════════════════════════════════════════════════

        /// <summary>GET /api/Admin/configuracion — Config global + todos los módulos.</summary>
        [HttpGet("configuracion")]
        public async Task<ActionResult> GetConfiguracion()
        {
            var totalUsuarios = await _context.Usuarios.CountAsync();
            var planesActivos = await _context.PlanesFacturacion.CountAsync(p => p.Activo);
            var suscripcionesActivas = await _context.SuscripcionesFacturacion.CountAsync(s => s.Activa);
            var totalFacturas = await _context.Facturas.CountAsync();
            var totalClientes = await _context.Clientes.CountAsync();
            var uptime = DateTime.UtcNow - _config.FechaInicio;

            return Ok(new
            {
                sistema = new
                {
                    version = "1.0.0",
                    ambiente = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production",
                    totalUsuarios,
                    planesActivos,
                    suscripcionesActivas,
                    totalFacturas,
                    totalClientes,
                    fechaServidor = DateTime.UtcNow,
                    uptimeHoras = Math.Round(uptime.TotalHours, 1)
                },
                empresa = _config.Empresa,
                smtp = new
                {
                    _config.Smtp.Host,
                    _config.Smtp.Puerto,
                    _config.Smtp.Usuario,
                    _config.Smtp.Remitente,
                    _config.Smtp.NombreRemitente,
                    _config.Smtp.UsarTls,
                    tieneContrasena = !string.IsNullOrEmpty(_config.Smtp.Contrasena)
                },
                dian = _config.Dian,
                apariencia = _config.Apariencia,
                mantenimiento = _config.Mantenimiento
            });
        }

        [HttpPost("configuracion/empresa")]
        public async Task<ActionResult> GuardarEmpresa([FromBody] ConfiguracionEmpresa dto)
        {
            _config.ActualizarEmpresa(dto);
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            await RegistrarAuditoria(adminId, "CONF_EMPRESA", "Configuración de empresa actualizada");
            return Ok(new { mensaje = "Configuración de empresa guardada" });
        }

        [HttpPost("configuracion/smtp")]
        public async Task<ActionResult> GuardarSmtp([FromBody] ConfiguracionSmtp dto)
        {
            _config.ActualizarSmtp(dto);
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            await RegistrarAuditoria(adminId, "CONF_SMTP", $"SMTP configurado: {dto.Host}:{dto.Puerto}");
            return Ok(new { mensaje = "Configuración SMTP guardada" });
        }

        [HttpPost("configuracion/smtp/probar")]
        public async Task<ActionResult> ProbarSmtp([FromBody] ProbarSmtpDto dto)
        {
            var (ok, mensaje) = await _config.ProbarSmtp(dto.CorreoDestino);
            return ok ? Ok(new { mensaje }) : BadRequest(new { mensaje });
        }

        [HttpPost("configuracion/dian")]
        public async Task<ActionResult> GuardarDian([FromBody] AdminConfigDian dto)
        {
            _config.ActualizarDian(dto);
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            await RegistrarAuditoria(adminId, "CONF_DIAN", $"DIAN: {dto.Ambiente} | Res. {dto.NumeroResolucion} | Prefijo {dto.Prefijo}");
            return Ok(new { mensaje = "Configuración DIAN guardada" });
        }

        [HttpPost("configuracion/apariencia")]
        public async Task<ActionResult> GuardarApariencia([FromBody] ConfiguracionApariencia dto)
        {
            _config.ActualizarApariencia(dto);
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            await RegistrarAuditoria(adminId, "CONF_APARIENCIA", $"Apariencia: {dto.NombrePlataforma}");
            return Ok(new { mensaje = "Configuración de apariencia guardada" });
        }

        [HttpPatch("configuracion/mantenimiento")]
        public async Task<ActionResult> ToggleMantenimiento([FromBody] ConfiguracionMantenimiento dto)
        {
            _config.ActualizarMantenimiento(dto);
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            await RegistrarAuditoria(adminId, "CONF_MANTENIMIENTO",
                dto.ModoMantenimiento ? "Modo mantenimiento ACTIVADO" : "Modo mantenimiento DESACTIVADO");
            return Ok(new { mensaje = dto.ModoMantenimiento ? "Modo mantenimiento activado" : "Modo mantenimiento desactivado" });
        }

        // Helper privado para registrar auditoría
        private async Task RegistrarAuditoria(int adminId, string accion, string detalle)
        {
            try
            {
                _context.AuditoriaAdmin.Add(new AuditoriaAdmin
                {
                    AdminId = adminId,
                    Accion = accion,
                    Detalle = detalle,
                    FechaHora = DateTime.UtcNow
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo registrar auditoría: {Accion}", accion);
            }
        }
    }

    // DTOs locales al controlador
    public class EstadoAdminDto
    {
        public bool Estado { get; set; }
    }

    public class EstadoFacturaDto
    {
        public string Estado { get; set; } = string.Empty;
    }

    public class PlanAdminDto
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = "FACTURACION";
        public string? Descripcion { get; set; }
        public decimal PrecioAnual { get; set; }
        public int? LimiteDocumentosAnuales { get; set; }
        public int? LimiteUsuarios { get; set; }
        public bool Destacado { get; set; }
        public bool DescuentoActivo { get; set; }
        public int? DescuentoPorcentaje { get; set; }
        public bool Activo { get; set; } = true;
        public bool IncluyePOS { get; set; } = false;
        public bool IncluyeInventario { get; set; } = false;
        public bool IncluyeNomina { get; set; } = false;
        public bool IncluyeContabilidad { get; set; } = false;
        public bool IncluyeSucursales { get; set; } = false;
        public List<string> Caracteristicas { get; set; } = new();
    }

    public class TogglePlanDto
    {
        public bool Activo { get; set; }
    }

    public class EditarProductoAdminDto
    {
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal? PrecioUnitario { get; set; }
        public string? UnidadMedida { get; set; }
        public string? CodigoInterno { get; set; }
        public string? CodigoBarras { get; set; }
    }

    public class CrearProductoAdminDto
    {
        [Required] public int UsuarioId { get; set; }
        [Required, MaxLength(500)] public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string? UnidadMedida { get; set; }
        public string? CodigoInterno { get; set; }
        public string? CodigoBarras { get; set; }
        public bool EsServicio { get; set; }
    }

    public class CrearSuscripcionAdminDto
    {
        [Required] public int UsuarioId { get; set; }
        [Required] public int PlanId { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }

    public class ProbarSmtpDto
    {
        [Required, EmailAddress] public string CorreoDestino { get; set; } = string.Empty;
    }
}
