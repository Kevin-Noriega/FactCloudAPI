using FactCloudAPI.Data;
using FactCloudAPI.Models;
using FactCloudAPI.DTOs.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace FactCloudAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioListDto>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Select(u => new UsuarioListDto
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Correo = u.Correo,
                    Estado = u.Estado
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            // Devolver el usuario completo SIN la contraseña
            var usuarioDto = new
            {
                usuario.Id,
                usuario.Nombre,
                usuario.Apellido,
                usuario.Correo,
                usuario.Telefono,
                usuario.NitNegocio,
                usuario.DvNitNegocio,
                usuario.NombreNegocio,
                usuario.DireccionNegocio,
                usuario.CiudadNegocio,
                usuario.DepartamentoNegocio,
                usuario.CorreoNegocio,
                usuario.LogoNegocio,
                usuario.TipoIdentificacion,
                usuario.TipoPersona,
                usuario.NumeroIdentificacion,
                usuario.CodigoPostal,
                usuario.DepartamentoCodigo,
                usuario.CiudadCodigo,
                usuario.TelefonoNegocio,
                usuario.ActividadEconomicaCIIU,
                usuario.RegimenFiscal,
                usuario.RegimenTributario,
                usuario.Pais,
                usuario.SoftwareProveedor,
                usuario.SoftwarePIN,
                usuario.PrefijoAutorizadoDIAN,
                usuario.NumeroResolucionDIAN,
                usuario.FechaResolucionDIAN,
                usuario.RangoNumeracionDesde,
                usuario.RangoNumeracionHasta,
                usuario.AmbienteDIAN,
                usuario.FechaVigenciaInicio,
                usuario.FechaVigenciaFinal,
                usuario.ResponsabilidadesRut,
                usuario.Estado,
                usuario.FechaDesactivacion,
                usuario.FechaRegistro
            };

            return Ok(usuarioDto);
        }

        // GET: api/Usuarios/me
        [HttpGet("me")]
        public async Task<ActionResult> GetMe()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var usuario = await _context.Usuarios.FindAsync(userId);

            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            var usuarioDto = new
            {
                usuario.Id,
                usuario.Nombre,
                usuario.Apellido,
                usuario.Correo,
                usuario.Telefono,
                usuario.NitNegocio,
                usuario.DvNitNegocio,
                usuario.NombreNegocio,
                usuario.DireccionNegocio,
                usuario.CiudadNegocio,
                usuario.DepartamentoNegocio,
                usuario.CorreoNegocio,
                usuario.LogoNegocio,
                usuario.TipoIdentificacion,
                usuario.TipoPersona,
                usuario.NumeroIdentificacion,
                usuario.TelefonoNegocio,
                usuario.RegimenFiscal,
                usuario.RegimenTributario,
                usuario.Estado,
                usuario.FechaRegistro
            };

            return Ok(usuarioDto);
        }

        // POST: api/Usuarios
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> PostUsuario(CreateUsuarioDto dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Correo == dto.Correo))
                return BadRequest(new { mensaje = "El correo ya está registrado" });

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Correo = dto.Correo,
                Telefono = dto.Telefono,
                ContrasenaHash = dto.Password, // Debe venir hasheada
                FechaRegistro = DateTime.UtcNow,
                Estado = true
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Usuario creado correctamente", id = usuario.Id });
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, UpdateUsuarioDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            // Verificar que el correo no esté en uso por otro usuario
            if (dto.Correo != usuario.Correo)
            {
                if (await _context.Usuarios.AnyAsync(u => u.Correo == dto.Correo && u.Id != id))
                    return BadRequest(new { mensaje = "El correo ya está en uso" });
            }

            // Actualizar campos básicos
            usuario.Nombre = dto.Nombre;
            usuario.Apellido = dto.Apellido;
            usuario.Correo = dto.Correo;
            usuario.Telefono = dto.Telefono;

            // Actualizar campos del negocio
            usuario.NitNegocio = dto.NitNegocio;
            usuario.DvNitNegocio = dto.DvNitNegocio;
            usuario.NombreNegocio = dto.NombreNegocio;
            usuario.DireccionNegocio = dto.DireccionNegocio;
            usuario.CiudadNegocio = dto.CiudadNegocio;
            usuario.DepartamentoNegocio = dto.DepartamentoNegocio;
            usuario.CorreoNegocio = dto.CorreoNegocio;
            usuario.TelefonoNegocio = dto.TelefonoNegocio;
            usuario.LogoNegocio = dto.LogoNegocio;
            usuario.RegimenFiscal = dto.RegimenFiscal;
            usuario.RegimenTributario = dto.RegimenTributario;
            usuario.Estado = dto.Estado;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                    return NotFound();
                else
                    throw;
            }

            return Ok(new { mensaje = "Usuario actualizado correctamente" });
        }

        // PATCH: api/Usuarios/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUsuario(int id, [FromBody] JsonElement patchDoc)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            // Actualizar solo los campos que vienen en el request
            if (patchDoc.TryGetProperty("nombre", out JsonElement nombre))
                usuario.Nombre = nombre.GetString();

            if (patchDoc.TryGetProperty("apellido", out JsonElement apellido))
                usuario.Apellido = apellido.GetString();

            if (patchDoc.TryGetProperty("correo", out JsonElement correo))
            {
                var nuevoCorreo = correo.GetString();
                if (nuevoCorreo != usuario.Correo)
                {
                    if (await _context.Usuarios.AnyAsync(u => u.Correo == nuevoCorreo && u.Id != id))
                        return BadRequest(new { mensaje = "El correo ya está en uso" });
                    usuario.Correo = nuevoCorreo;
                }
            }

            if (patchDoc.TryGetProperty("telefono", out JsonElement telefono))
                usuario.Telefono = telefono.GetString();

            if (patchDoc.TryGetProperty("nombreNegocio", out JsonElement nombreNegocio))
                usuario.NombreNegocio = nombreNegocio.GetString();

            if (patchDoc.TryGetProperty("logoNegocio", out JsonElement logoNegocio))
                usuario.LogoNegocio = logoNegocio.GetString();

            if (patchDoc.TryGetProperty("nitNegocio", out JsonElement nitNegocio))
                usuario.NitNegocio = nitNegocio.GetString();

            if (patchDoc.TryGetProperty("dvNitNegocio", out JsonElement dvNitNegocio))
            {
                if (dvNitNegocio.ValueKind == JsonValueKind.Null)
                    usuario.DvNitNegocio = null;
                else
                    usuario.DvNitNegocio = dvNitNegocio.GetInt32();
            }

            if (patchDoc.TryGetProperty("direccionNegocio", out JsonElement direccionNegocio))
                usuario.DireccionNegocio = direccionNegocio.GetString();

            if (patchDoc.TryGetProperty("departamentoNegocio", out JsonElement departamentoNegocio))
                usuario.DepartamentoNegocio = departamentoNegocio.GetString();

            if (patchDoc.TryGetProperty("ciudadNegocio", out JsonElement ciudadNegocio))
                usuario.CiudadNegocio = ciudadNegocio.GetString();

            if (patchDoc.TryGetProperty("correoNegocio", out JsonElement correoNegocio))
                usuario.CorreoNegocio = correoNegocio.GetString();

            if (patchDoc.TryGetProperty("telefonoNegocio", out JsonElement telefonoNegocio))
                usuario.TelefonoNegocio = telefonoNegocio.GetString();

            if (patchDoc.TryGetProperty("regimenFiscal", out JsonElement regimenFiscal))
                usuario.RegimenFiscal = regimenFiscal.GetString();

            if (patchDoc.TryGetProperty("regimenTributario", out JsonElement regimenTributario))
                usuario.RegimenTributario = regimenTributario.GetString();

            if (patchDoc.TryGetProperty("tipoIdentificacion", out JsonElement tipoIdentificacion))
                usuario.TipoIdentificacion = tipoIdentificacion.GetString();

            if (patchDoc.TryGetProperty("tipoPersona", out JsonElement tipoPersona))
                usuario.TipoPersona = tipoPersona.GetString();

            if (patchDoc.TryGetProperty("numeroIdentificacion", out JsonElement numeroIdentificacion))
                usuario.NumeroIdentificacion = numeroIdentificacion.GetString();

            if (patchDoc.TryGetProperty("codigoPostal", out JsonElement codigoPostal))
                usuario.CodigoPostal = codigoPostal.GetString();

            if (patchDoc.TryGetProperty("actividadEconomicaCIIU", out JsonElement actividadEconomicaCIIU))
                usuario.ActividadEconomicaCIIU = actividadEconomicaCIIU.GetString();

            // Manejo del estado y fecha de desactivación
            if (patchDoc.TryGetProperty("estado", out JsonElement estado))
            {
                usuario.Estado = estado.GetBoolean();

                // Si se desactiva, guardar la fecha
                if (!usuario.Estado)
                {
                    usuario.FechaDesactivacion = DateTime.UtcNow;
                }
                // Si se activa, limpiar la fecha
                else
                {
                    usuario.FechaDesactivacion = null;
                }
            }

            // Manejar fechaDesactivacion explícitamente si viene en el request
            if (patchDoc.TryGetProperty("fechaDesactivacion", out JsonElement fechaDesactivacion))
            {
                if (fechaDesactivacion.ValueKind == JsonValueKind.Null)
                    usuario.FechaDesactivacion = null;
                else
                    usuario.FechaDesactivacion = fechaDesactivacion.GetDateTime();
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                    return NotFound();
                else
                    throw;
            }

            return Ok(new { mensaje = "Usuario actualizado correctamente" });
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Usuario eliminado correctamente" });
        }

        // Método helper
        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
