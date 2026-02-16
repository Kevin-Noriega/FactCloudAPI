using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Login;
using FactCloudAPI.DTOs.Usuarios;
using FactCloudAPI.Models;
using FactCloudAPI.Models.Planes;
using FactCloudAPI.Models.Suscripciones;
using FactCloudAPI.Models.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static FactCloudAPI.DTOs.Login.UsuarioLoginDto;

namespace FactCloudAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UsuariosController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        // ? GET: Obtener usuario por ID (para React Query)
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult> GetUsuario(int id)
        {
            Console.WriteLine("\n=== GET /Usuarios/{id} ===");
            Console.WriteLine($"ID solicitado: {id}");
            Console.WriteLine($"IsAuthenticated: {User.Identity?.IsAuthenticated}");
            Console.WriteLine($"Identity Name: {User.Identity?.Name}");

            if (User.Identity?.IsAuthenticated == true)
            {
                Console.WriteLine("Claims del usuario:");
                foreach (var claim in User.Claims)
                {
                    Console.WriteLine($"  - {claim.Type}: {claim.Value}");
                }
            }
            else
            {
                Console.WriteLine("❌ Usuario NO autenticado");
                return Unauthorized(new { message = "No autenticado" });
            }

            try
            {
                var usuario = await _context.Usuarios
                    .Include(u => u.Negocio)
                    .Include(u => u.Suscripciones.Where(s => s.Activa))
                        .ThenInclude(s => s.PlanFacturacion)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (usuario == null)
                {
                    Console.WriteLine($"❌ Usuario {id} no encontrado en BD");
                    return NotFound(new { message = "Usuario no encontrado" });
                }

                // Verificar que el usuario autenticado coincida
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Console.WriteLine($"UserID del token: {userIdClaim}");
                Console.WriteLine($"UserID solicitado: {id}");

                if (userIdClaim != id.ToString())
                {
                    Console.WriteLine("❌ ID no coincide - No autorizado");
                    return Unauthorized(new { message = "No autorizado para ver este usuario" });
                }

                var suscripcionActiva = usuario.Suscripciones
                    .FirstOrDefault(s => s.Activa && (s.FechaFin == null || s.FechaFin > DateTime.UtcNow));

                Console.WriteLine($"✅ Usuario encontrado: {usuario.Correo}");
                Console.WriteLine("=== GET EXITOSO ===\n");

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
                        tipoIdentificacion = usuario.TipoIdentificacion,
                        numeroIdentificacion = usuario.NumeroIdentificacion
                    },
                    negocio = usuario.Negocio != null ? new
                    {
                        id = usuario.Negocio.Id,
                        nombreNegocio = usuario.Negocio.NombreNegocio,
                        nit = usuario.Negocio.Nit,
                        dvNit = usuario.Negocio.DvNit,
                        direccion = usuario.Negocio.Direccion,
                        ciudad = usuario.Negocio.Ciudad,
                        departamento = usuario.Negocio.Departamento
                    } : null,
                    suscripcion = suscripcionActiva != null ? new
                    {
                        plan = suscripcionActiva.PlanFacturacion.Nombre,
                        documentosIncluidos = suscripcionActiva.PlanFacturacion.LimiteDocumentosAnuales,
                        documentosUsados = suscripcionActiva.DocumentosUsados,
                        documentosRestantes = (suscripcionActiva.PlanFacturacion.LimiteDocumentosAnuales ?? 0) - suscripcionActiva.DocumentosUsados,
                        fechaExpiracion = suscripcionActiva.FechaFin,
                        activa = suscripcionActiva.Activa
                    } : null
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetUsuario: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return StatusCode(500, new { error = ex.Message });
            }
        }


        [HttpPost("crear-y-activar")]
        [AllowAnonymous]
        public async Task<ActionResult> CrearYActivarUsuario([FromBody] CrearYActivarDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Verificar si el correo ya existe
                if (await _context.Usuarios.AnyAsync(u => u.Correo == dto.Correo))
                {
                    return BadRequest(new { error = "El correo ya está registrado" });
                }

                // 2. Hash de contraseña
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                // 3. Crear usuario
                var usuario = new Usuario
                {
                    Nombre = dto.Nombre,
                    Telefono = dto.Telefono,
                    Correo = dto.Correo,
                    ContrasenaHash = passwordHash,
                    TipoIdentificacion = dto.TipoIdentificacion,
                    NumeroIdentificacion = dto.NumeroIdentificacion,
                    Estado = true,
                    FechaRegistro = DateTime.UtcNow
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // 4. Crear negocio
                var negocio = new Negocio
                {
                    UsuarioId = usuario.Id,
                    NombreNegocio = dto.NombreNegocio,
                    Nit = dto.Nit,
                    DvNit = dto.DvNit,
                    Direccion = dto.Direccion,
                    Ciudad = dto.Ciudad,
                    Departamento = dto.Departamento,
                    Telefono = dto.TelefonoNegocio,
                    Correo = dto.CorreoNegocio
                };

                _context.Negocios.Add(negocio);
                await _context.SaveChangesAsync();

                // 5. Crear suscripción
                var plan = await _context.PlanesFacturacion.FindAsync(dto.PlanFacturacionId);
                if (plan == null)
                {
                    throw new Exception("Plan no encontrado");
                }

                var fechaInicio = DateTime.UtcNow;
                DateTime fechaFin;
                int documentosRestantes;

                // ? Determinar duración según si es mensual o anual (basado en el precio pagado)
                if (dto.TipoPago == "anual" || dto.PrecioPagado >= plan.PrecioAnual)
                {
                    fechaFin = fechaInicio.AddYears(1);
                    documentosRestantes = (plan.LimiteDocumentosAnuales ?? 0) * 12;
                }
                else
                {
                    fechaFin = fechaInicio.AddMonths(1);
                    documentosRestantes = plan.LimiteDocumentosAnuales ?? 0;
                }

                var suscripcion = new SuscripcionFacturacion
                {
                    UsuarioId = usuario.Id,
                    PlanFacturacionId = dto.PlanFacturacionId,
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    Activa = true,
                    TransaccionId = dto.TransaccionId,
                };

                _context.SuscripcionesFacturacion.Add(suscripcion);
                await _context.SaveChangesAsync();

                // 6. Commit de la transacción
                await transaction.CommitAsync();

                // 7. Generar token JWT
                var token = GenerarTokenJWT(usuario);

                return Ok(new
                {
                    token = token,
                    usuario = new
                    {
                        id = usuario.Id,
                        nombre = usuario.Nombre,
                        correo = usuario.Correo,
                        estado = usuario.Estado,
                        negocio = new
                        {
                            id = negocio.Id,
                            nombre = negocio.NombreNegocio,
                            nit = negocio.Nit
                        },
                        suscripcion = new
                        {
                            plan = plan.Nombre,
                            fechaInicio = fechaInicio,
                            fechaFin = fechaFin,
                            documentosRestantes = documentosRestantes
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { error = ex.Message });
            }
        }




        // ========== FASE 1: PRE-REGISTRO (Solo crear usuario inactivo) ==========
        [HttpPost]
        public async Task<ActionResult> PreRegistroUsuario([FromBody] PreRegistroDto dto)
        {
            try
            {
                // Validar correo único
                var existe = await _context.Usuarios
                    .AnyAsync(u => u.Correo == dto.Correo);

                if (existe)
                    return BadRequest("El correo ya está registrado");

                // Hash de contraseña
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                var usuario = new Usuario
                {
                    Nombre = dto.Nombre,
                    Apellido = dto.Apellido,
                    Correo = dto.Correo,
                    ContrasenaHash = passwordHash,
                    TipoIdentificacion = dto.TipoIdentificacion,
                    NumeroIdentificacion = dto.NumeroIdentificacion,
                    Telefono = dto.Telefono,

                    // ? Usuario inactivo hasta que pague
                    Estado = false,
                    FechaRegistro = DateTime.UtcNow
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    id = usuario.Id,
                    correo = usuario.Correo,
                    mensaje = "Pre-registro exitoso. Completa el pago para activar tu cuenta."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // ========== FASE 2: ACTIVAR CUENTA DESPUÉS DEL PAGO ==========
        [HttpPost("{usuarioId}/activar")]
        public async Task<ActionResult> ActivarCuentaConPago(
            int usuarioId,
            [FromBody] ActivacionCuentaDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1?? Obtener usuario
                var usuario = await _context.Usuarios
                    .Include(u => u.Negocio)
                    .FirstOrDefaultAsync(u => u.Id == usuarioId);

                if (usuario == null)
                    return NotFound("Usuario no encontrado");

                if (usuario.Estado)
                    return BadRequest("La cuenta ya está activa");

                // 2?? Obtener el plan seleccionado
                var plan = await _context.PlanesFacturacion
                    .FirstOrDefaultAsync(p => p.Id == dto.PlanFacturacionId);

                if (plan == null)
                    return NotFound("Plan no encontrado");

                // 3?? Crear Negocio (si no existe)
                if (usuario.Negocio == null)
                {
                    var negocio = new Negocio
                    {
                        NombreNegocio = dto.NombreNegocio ?? $"Negocio de {usuario.Nombre}",
                        Nit = dto.Nit,
                        DvNit = dto.DvNit,
                        Direccion = dto.Direccion,
                        Ciudad = dto.Ciudad,
                        Departamento = dto.Departamento,
                        Telefono = dto.TelefonoNegocio ?? usuario.Telefono,
                        Correo = dto.CorreoNegocio ?? usuario.Correo,
                        Pais = "CO",
                        UsuarioId = usuario.Id
                    };

                    _context.Negocios.Add(negocio);
                }

                // 4?? Crear Suscripción Activa
                var suscripcion = new SuscripcionFacturacion
                {
                    UsuarioId = usuario.Id,
                    PlanFacturacionId = plan.Id,
                    FechaInicio = DateTime.UtcNow,
                    FechaFin = DateTime.UtcNow.AddYears(1), // Plan anual
                    DocumentosUsados = 0,
                    Activa = true
                };

                _context.SuscripcionesFacturacion.Add(suscripcion);

                // 5?? Activar Usuario
                usuario.Estado = true;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // 6?? Generar Token JWT para login automático
                var token = GenerarTokenJWT(usuario);

                return Ok(new
                {
                    mensaje = "¡Cuenta activada exitosamente!",
                    token,
                    usuario = new
                    {
                        id = usuario.Id,
                        nombre = $"{usuario.Nombre} {usuario.Apellido}",
                        correo = usuario.Correo,
                        negocio = usuario.Negocio?.NombreNegocio
                    },
                    suscripcion = new
                    {
                        plan = plan.Nombre,
                        documentosIncluidos = plan.LimiteDocumentosAnuales,
                        fechaExpiracion = suscripcion.FechaFin
                    }
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // ========== LOGIN (Solo usuarios activos con suscripción) ==========
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.Correo) || string.IsNullOrEmpty(dto.Password))
                    return BadRequest("Email y contraseña son requeridos");

                // Obtener usuario con sus relaciones
                var usuario = await _context.Usuarios
                    .Include(u => u.Negocio)
                    .Include(u => u.Suscripciones.Where(s => s.Activa))
                        .ThenInclude(s => s.PlanFacturacion)
                    .FirstOrDefaultAsync(u => u.Correo == dto.Correo);

                if (usuario == null)
                    return NotFound("Usuario no encontrado");

                // ? Verificar que la cuenta esté activa
                if (!usuario.Estado)
                {
                    return Unauthorized(new
                    {
                        codigo = "CUENTA_INACTIVA",
                        mensaje = "Tu cuenta está pendiente de activación. Completa el pago para acceder."
                    });
                }

                // Verificar contraseña
                if (!BCrypt.Net.BCrypt.Verify(dto.Password, usuario.ContrasenaHash))
                    return Unauthorized("Contraseña incorrecta");

                // ? Verificar suscripción activa
                var suscripcionActiva = usuario.Suscripciones
                    .FirstOrDefault(s => s.Activa && (s.FechaFin == null || s.FechaFin > DateTime.UtcNow));

                if (suscripcionActiva == null)
                {
                    return Unauthorized(new
                    {
                        codigo = "SIN_SUSCRIPCION",
                        mensaje = "No tienes una suscripción activa. Renueva tu plan para continuar."
                    });
                }

                // ? Verificar límite de documentos (si aplica)
                if (suscripcionActiva.PlanFacturacion.LimiteDocumentosAnuales > 0 &&
                    suscripcionActiva.DocumentosUsados >= suscripcionActiva.PlanFacturacion.LimiteDocumentosAnuales)
                {
                    return Unauthorized(new
                    {
                        codigo = "LIMITE_DOCUMENTOS",
                        mensaje = $"Has alcanzado el límite de {suscripcionActiva.PlanFacturacion.LimiteDocumentosAnuales} documentos de tu plan."
                    });
                }

                var token = GenerarTokenJWT(usuario);

                return Ok(new
                {
                    token,
                    usuario = new
                    {
                        id = usuario.Id,
                        nombre = $"{usuario.Nombre} {usuario.Apellido}",
                        correo = usuario.Correo,
                        tipoIdentificacion = usuario.TipoIdentificacion,
                        numeroIdentificacion = usuario.NumeroIdentificacion
                    },
                    negocio = usuario.Negocio != null ? new
                    {
                        id = usuario.Negocio.Id,
                        nombre = usuario.Negocio.NombreNegocio,
                        nit = usuario.Negocio.Nit,
                        ciudad = usuario.Negocio.Ciudad
                    } : null,
                    suscripcion = new
                    {
                        plan = suscripcionActiva.PlanFacturacion.Nombre,
                        documentosIncluidos = suscripcionActiva.PlanFacturacion.LimiteDocumentosAnuales,
                        documentosUsados = suscripcionActiva.DocumentosUsados,
                        documentosRestantes = suscripcionActiva.PlanFacturacion.LimiteDocumentosAnuales - suscripcionActiva.DocumentosUsados,
                        fechaExpiracion = suscripcionActiva.FechaFin,
                        activa = suscripcionActiva.Activa
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // ========== MÉTODO AUXILIAR: Generar JWT ==========
        private string GenerarTokenJWT(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "clave-super-secreta-minimo-32-caracteres");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}"),
            };

            // Agregar claim de negocio si existe
            if (usuario.Negocio != null)
            {
                claims.Add(new Claim("NegocioId", usuario.Negocio.Id.ToString()));
                claims.Add(new Claim("NegocioNombre", usuario.Negocio.NombreNegocio));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }

}
