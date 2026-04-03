using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Usuarios;
using FactCloudAPI.Models.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FactCloudAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NegociosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NegociosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ═══════════════════════════════════════════════════════════════════
        // GET api/negocios/mio
        // Devuelve el perfil completo con estado de cada sección
        // ═══════════════════════════════════════════════════════════════════
        [HttpGet("mio")]
        public async Task<ActionResult<NegocioCompletoResponse>> GetMiNegocio()
        {
            var usuarioId = ObtenerUsuarioId();

            var negocio = await _context.Negocios
                .Include(n => n.PerfilTributario)
                .Include(n => n.RepresentanteLegal)
                .Include(n => n.ConfiguracionDIAN)
                .FirstOrDefaultAsync(n => n.UsuarioId == usuarioId);

            if (negocio == null)
                return NotFound(new { mensaje = "No tienes un negocio registrado." });

            return Ok(MapearRespuestaCompleta(negocio));
        }

        // ═══════════════════════════════════════════════════════════════════
        // POST api/negocios
        // PASO 1: Crear negocio con datos generales
        // ═══════════════════════════════════════════════════════════════════
        [HttpPost]
        public async Task<ActionResult<NegocioCompletoResponse>> Crear(
            [FromBody] NegocioDatosGeneralesDto dto)
        {
            var usuarioId = ObtenerUsuarioId();

            if (await _context.Negocios.AnyAsync(n => n.UsuarioId == usuarioId))
                return Conflict(new { mensaje = "Ya tienes un negocio registrado. Usa PUT para editarlo." });

            if (await _context.Negocios.AnyAsync(n => n.NumeroIdentificacionE == dto.NumeroIdentificacionE))
                return Conflict(new { mensaje = "Ya existe un negocio con ese NIT/identificación." });

            var negocio = new Negocio
            {
                UsuarioId = usuarioId,
                TipoSujeto = (TipoSujeto)dto.TipoSujeto,
                TipoDocumento = (TipoDocumento)dto.TipoDocumento,
                NombreComercial = dto.NombreComercial,
                RazonSocial = dto.RazonSocial,
                PrimerNombre = dto.PrimerNombre,
                SegundoNombre = dto.SegundoNombre,
                PrimerApellido = dto.PrimerApellido,
                SegundoApellido = dto.SegundoApellido,
                NumeroIdentificacionE = dto.NumeroIdentificacionE,
                DvNit = dto.DvNit,
                Direccion = dto.Direccion,
                Ciudad = dto.Ciudad,
                Departamento = dto.Departamento,
                Pais = dto.Pais,
                Telefono = dto.Telefono,
                CorreoElectronico = dto.CorreoElectronico,
                CorreoRecepcionDian = dto.CorreoRecepcionDian,
                DatosFacturacionCompletos = false
            };

            _context.Negocios.Add(negocio);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMiNegocio), MapearRespuestaCompleta(negocio));
        }

        // ═══════════════════════════════════════════════════════════════════
        // PUT api/negocios/mio/datos-generales
        // PASO 1 (edición): Actualiza datos generales del negocio
        // ═══════════════════════════════════════════════════════════════════
        [HttpPut("mio/datos-generales")]
        public async Task<ActionResult<NegocioCompletoResponse>> ActualizarDatosGenerales(
            [FromBody] NegocioDatosGeneralesDto dto)
        {
            var negocio = await ObtenerMiNegocioAsync();
            if (negocio == null) return NotFound(new { mensaje = "No tienes un negocio registrado." });

            negocio.TipoSujeto = (TipoSujeto)dto.TipoSujeto;
            negocio.TipoDocumento = (TipoDocumento)dto.TipoDocumento;
            negocio.NombreComercial = dto.NombreComercial;
            negocio.RazonSocial = dto.RazonSocial;
            negocio.PrimerNombre = dto.PrimerNombre;
            negocio.SegundoNombre = dto.SegundoNombre;
            negocio.PrimerApellido = dto.PrimerApellido;
            negocio.SegundoApellido = dto.SegundoApellido;
            negocio.NumeroIdentificacionE = dto.NumeroIdentificacionE;
            negocio.DvNit = dto.DvNit;
            negocio.Direccion = dto.Direccion;
            negocio.Ciudad = dto.Ciudad;
            negocio.Departamento = dto.Departamento;
            negocio.Pais = dto.Pais;
            negocio.Telefono = dto.Telefono;
            negocio.CorreoElectronico = dto.CorreoElectronico;
            negocio.CorreoRecepcionDian = dto.CorreoRecepcionDian;

            await _context.SaveChangesAsync();
            return Ok(MapearRespuestaCompleta(negocio));
        }

        // ═══════════════════════════════════════════════════════════════════
        // PUT api/negocios/mio/perfil-tributario
        // PASO 2: Guardar o actualizar perfil tributario
        // ═══════════════════════════════════════════════════════════════════
        [HttpPut("mio/perfil-tributario")]
        public async Task<ActionResult<NegocioCompletoResponse>> GuardarPerfilTributario(
            [FromBody] PerfilTributarioDto dto)
        {
            var negocio = await ObtenerMiNegocioAsync(
                incluirPerfil: true,
                incluirRep: true,
                incluirDian: true);

            if (negocio == null) return NotFound(new { mensaje = "No tienes un negocio registrado." });

            if (negocio.PerfilTributario == null)
            {
                negocio.PerfilTributario = new PerfilTributario
                {
                    NegocioId = negocio.Id
                };
                _context.PerfilesTributarios.Add(negocio.PerfilTributario);
            }

            negocio.PerfilTributario.RegimenIvaCodigo = dto.RegimenIvaCodigo;
            negocio.PerfilTributario.ActividadEconomicaCIIU = dto.ActividadEconomicaCIIU;
            negocio.PerfilTributario.TributosJson = dto.TributosJson;
            negocio.PerfilTributario.ResponsabilidadesFiscalesJson = dto.ResponsabilidadesFiscalesJson;

            await ActualizarCompletitudAsync(negocio);
            await _context.SaveChangesAsync();

            return Ok(MapearRespuestaCompleta(negocio));
        }

        // ═══════════════════════════════════════════════════════════════════
        // PUT api/negocios/mio/representante-legal
        // PASO 3: Guardar o actualizar representante legal
        // ═══════════════════════════════════════════════════════════════════
        [HttpPut("mio/representante-legal")]
        public async Task<ActionResult<NegocioCompletoResponse>> GuardarRepresentanteLegal(
            [FromBody] RepresentanteLegalDto dto)
        {
            var negocio = await ObtenerMiNegocioAsync(
                incluirPerfil: true,
                incluirRep: true,
                incluirDian: true);

            if (negocio == null) return NotFound(new { mensaje = "No tienes un negocio registrado." });

            if (negocio.RepresentanteLegal == null)
            {
                negocio.RepresentanteLegal = new RepresentanteLegal
                {
                    NegocioId = negocio.Id
                };
                _context.RepresentantesLegales.Add(negocio.RepresentanteLegal);
            }

            negocio.RepresentanteLegal.Nombre = dto.Nombre;
            negocio.RepresentanteLegal.Apellidos = dto.Apellidos;
            negocio.RepresentanteLegal.TipoDocumento = (TipoDocumento)dto.TipoDocumento;
            negocio.RepresentanteLegal.NumeroIdentificacion = dto.NumeroIdentificacion;
            negocio.RepresentanteLegal.CiudadExpedicion = dto.CiudadExpedicion;
            negocio.RepresentanteLegal.CiudadResidencia = dto.CiudadResidencia;

            await ActualizarCompletitudAsync(negocio);
            await _context.SaveChangesAsync();

            return Ok(MapearRespuestaCompleta(negocio));
        }

        // ═══════════════════════════════════════════════════════════════════
        // PUT api/negocios/mio/configuracion-dian
        // PASO 4: Guardar o actualizar configuración DIAN / software / numeración
        // ═══════════════════════════════════════════════════════════════════
        [HttpPut("mio/configuracion-dian")]
        public async Task<ActionResult<NegocioCompletoResponse>> GuardarConfiguracionDian(
            [FromBody] ConfiguracionDianDto dto)
        {
            var negocio = await ObtenerMiNegocioAsync(
                incluirPerfil: true,
                incluirRep: true,
                incluirDian: true);

            if (negocio == null) return NotFound(new { mensaje = "No tienes un negocio registrado." });

            if (negocio.ConfiguracionDIAN == null)
            {
                negocio.ConfiguracionDIAN = new ConfiguracionDian
                {
                    NegocioId = negocio.Id
                };
                _context.ConfiguracionesDian.Add(negocio.ConfiguracionDIAN);
            }

            negocio.ConfiguracionDIAN.SoftwareProveedor = dto.SoftwareProveedor;
            negocio.ConfiguracionDIAN.SoftwarePIN = dto.SoftwarePIN;
            negocio.ConfiguracionDIAN.PrefijoAutorizadoDIAN = dto.PrefijoAutorizadoDIAN;
            negocio.ConfiguracionDIAN.NumeroResolucionDIAN = dto.NumeroResolucionDIAN;
            negocio.ConfiguracionDIAN.RangoNumeracionDesde = dto.RangoNumeracionDesde;
            negocio.ConfiguracionDIAN.RangoNumeracionHasta = dto.RangoNumeracionHasta;
            negocio.ConfiguracionDIAN.AmbienteDIAN = dto.AmbienteDIAN;
            negocio.ConfiguracionDIAN.FechaVigenciaInicio = dto.FechaVigenciaInicio;
            negocio.ConfiguracionDIAN.FechaVigenciaFinal = dto.FechaVigenciaFinal;

            await ActualizarCompletitudAsync(negocio);
            await _context.SaveChangesAsync();

            return Ok(MapearRespuestaCompleta(negocio));
        }

        // ═══════════════════════════════════════════════════════════════════
        // HELPERS PRIVADOS
        // ═══════════════════════════════════════════════════════════════════

        private int ObtenerUsuarioId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? User.FindFirst("sub")?.Value;
            if (!int.TryParse(claim, out var id))
                throw new UnauthorizedAccessException("Token inválido.");
            return id;
        }

        private async Task<Negocio?> ObtenerMiNegocioAsync(
            bool incluirPerfil = false,
            bool incluirRep = false,
            bool incluirDian = false)
        {
            var usuarioId = ObtenerUsuarioId();
            var query = _context.Negocios.AsQueryable();

            if (incluirPerfil) query = query.Include(n => n.PerfilTributario);
            if (incluirRep) query = query.Include(n => n.RepresentanteLegal);
            if (incluirDian) query = query.Include(n => n.ConfiguracionDIAN);

            return await query.FirstOrDefaultAsync(n => n.UsuarioId == usuarioId);
        }

        private static Task ActualizarCompletitudAsync(Negocio negocio)
        {
            negocio.DatosFacturacionCompletos =
                negocio.PerfilTributario != null &&
                negocio.RepresentanteLegal != null &&
                negocio.ConfiguracionDIAN != null;
            return Task.CompletedTask;
        }

        private static NegocioCompletoResponse MapearRespuestaCompleta(Negocio negocio)
        {
            var completados = new List<string>();
            var pendientes = new List<string>();

            // Siempre tiene datos generales si existe
            completados.Add("datos-generales");

            void Evaluar(bool tiene, string paso)
            {
                if (tiene) completados.Add(paso);
                else pendientes.Add(paso);
            }

            Evaluar(negocio.PerfilTributario != null, "perfil-tributario");
            Evaluar(negocio.RepresentanteLegal != null, "representante-legal");
            Evaluar(negocio.ConfiguracionDIAN != null, "configuracion-dian");

            return new NegocioCompletoResponse
            {
                Id = negocio.Id,
                DatosFacturacionCompletos = negocio.DatosFacturacionCompletos,
                PasosCompletados = completados,
                PasosPendientes = pendientes,

                DatosGenerales = new NegocioDatosGeneralesDto
                {
                    TipoSujeto = (int)negocio.TipoSujeto,
                    TipoDocumento = (int)negocio.TipoDocumento,
                    NombreComercial = negocio.NombreComercial,
                    RazonSocial = negocio.RazonSocial,
                    PrimerNombre = negocio.PrimerNombre,
                    SegundoNombre = negocio.SegundoNombre,
                    PrimerApellido = negocio.PrimerApellido,
                    SegundoApellido = negocio.SegundoApellido,
                    NumeroIdentificacionE = negocio.NumeroIdentificacionE,
                    DvNit = negocio.DvNit,
                    Direccion = negocio.Direccion,
                    Ciudad = negocio.Ciudad,
                    Departamento = negocio.Departamento,
                    Pais = negocio.Pais,
                    Telefono = negocio.Telefono,
                    CorreoElectronico = negocio.CorreoElectronico,
                    CorreoRecepcionDian = negocio.CorreoRecepcionDian,
                },

                PerfilTributario = negocio.PerfilTributario == null ? null :
                    new PerfilTributarioDto
                    {
                        RegimenIvaCodigo = negocio.PerfilTributario.RegimenIvaCodigo,
                        ActividadEconomicaCIIU = negocio.PerfilTributario.ActividadEconomicaCIIU,
                        TributosJson = negocio.PerfilTributario.TributosJson,
                        ResponsabilidadesFiscalesJson = negocio.PerfilTributario.ResponsabilidadesFiscalesJson,
                    },

                RepresentanteLegal = negocio.RepresentanteLegal == null ? null :
                    new RepresentanteLegalDto
                    {
                        Nombre = negocio.RepresentanteLegal.Nombre,
                        Apellidos = negocio.RepresentanteLegal.Apellidos,
                        TipoDocumento = (int)negocio.RepresentanteLegal.TipoDocumento,
                        NumeroIdentificacion = negocio.RepresentanteLegal.NumeroIdentificacion,
                        CiudadExpedicion = negocio.RepresentanteLegal.CiudadExpedicion,
                        CiudadResidencia = negocio.RepresentanteLegal.CiudadResidencia,
                    },

                ConfiguracionDian = negocio.ConfiguracionDIAN == null ? null :
                    new ConfiguracionDianDto
                    {
                        SoftwareProveedor = negocio.ConfiguracionDIAN.SoftwareProveedor,
                        SoftwarePIN = negocio.ConfiguracionDIAN.SoftwarePIN,
                        PrefijoAutorizadoDIAN = negocio.ConfiguracionDIAN.PrefijoAutorizadoDIAN,
                        NumeroResolucionDIAN = negocio.ConfiguracionDIAN.NumeroResolucionDIAN,
                        RangoNumeracionDesde = negocio.ConfiguracionDIAN.RangoNumeracionDesde,
                        RangoNumeracionHasta = negocio.ConfiguracionDIAN.RangoNumeracionHasta,
                        AmbienteDIAN = negocio.ConfiguracionDIAN.AmbienteDIAN,
                        FechaVigenciaInicio = negocio.ConfiguracionDIAN.FechaVigenciaInicio,
                        FechaVigenciaFinal = negocio.ConfiguracionDIAN.FechaVigenciaFinal,
                    }
            };
        }
    }
}
