using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using NubeeAPI.Data;
using NubeeAPI.DTOs.Habilitacion;
using NubeeAPI.Models;
using NubeeAPI.Models.Usuarios;
using NubeeAPI.Services.Factus;
using NubeeAPI.Services.Habilitacion;
using NubeeAPI.Services;
using System.Security.Cryptography;
using System.Text.Json;

namespace NubeeAPI.Services.Habilitacion
{
    /// <summary>
    /// Implementación del servicio de habilitación electrónica.
    /// Centraliza toda la lógica de negocio; el controller solo delega aquí.
    /// </summary>
    public class HabilitacionService : IHabilitacionService
    {
        private readonly ApplicationDbContext _db;
        private readonly IFactusService _factus;
        private readonly ILogger<HabilitacionService> _logger;

        public HabilitacionService(
            ApplicationDbContext db,
            IFactusService factus,
            ILogger<HabilitacionService> logger)
        {
            _db = db;
            _factus = factus;
            _logger = logger;
        }

        // ══════════════════════════════════════════════════════════════
        //  PASO 1 — Perfil empresa
        // ══════════════════════════════════════════════════════════════
        public async Task<PerfilEmpresaDto?> ObtenerPerfilEmpresaAsync(int usuarioId)
        {
            var negocio = await _db.Negocios
                .Include(n => n.ConfiguracionDIAN)
                .Include(n => n.PerfilTributario)
                .Include(n => n.RepresentanteLegal)
                .FirstOrDefaultAsync(n => n.UsuarioId == usuarioId);

            if (negocio == null) return null;

            return MapearPerfilDto(negocio);
        }

        public async Task ActualizarPerfilEmpresaAsync(int usuarioId, PerfilEmpresaDto dto)
        {
            var negocio = await _db.Negocios
                .Include(n => n.ConfiguracionDIAN)
                .Include(n => n.PerfilTributario)
                .Include(n => n.RepresentanteLegal)
                .FirstOrDefaultAsync(n => n.UsuarioId == usuarioId)
                ?? throw new InvalidOperationException("Negocio no encontrado.");

            // Negocio
            negocio.TipoPersona = dto.TipoPersona == "persona_natural" ? "PersonaNatural" : "PersonaJuridica";
            negocio.Nit = dto.NumeroIdentificacion;
            negocio.DvNit = dto.Dv;
            negocio.RazonSocial = dto.RazonSocial;
            negocio.NombreNegocio = dto.NombreComercial ?? negocio.NombreNegocio;
            negocio.Correo = dto.Correo;
            negocio.Direccion = dto.Direccion;
            negocio.Ciudad = dto.Ciudad;
            negocio.Departamento = dto.Departamento;
            negocio.Telefono = dto.Telefono;
            negocio.DatosFacturacionCompletos = true;

            // PerfilTributario
            var perfil = negocio.PerfilTributario ?? new PerfilTributario { NegocioId = negocio.Id };
            perfil.RegimenIvaCodigo = dto.RegimenIvaCodigo;
            perfil.ActividadEconomicaCIIU = dto.ActividadEconomicaCIIU;
            perfil.TributosJson = JsonSerializer.Serialize(dto.Tributos ?? new List<string>());
            perfil.ResponsabilidadesFiscalesJson = JsonSerializer.Serialize(dto.ResponsabilidadesFiscales ?? new List<string>());
            if (perfil.Id == 0) _db.PerfilesTributarios.Add(perfil);

            // RepresentanteLegal (solo persona jurídica)
            if (dto.TipoPersona == "empresa")
            {
                var rep = negocio.RepresentanteLegal ?? new RepresentanteLegal { NegocioId = negocio.Id };
                rep.Nombre = dto.RepresentanteNombre ?? rep.Nombre;
                rep.Apellidos = dto.RepresentanteApellidos ?? rep.Apellidos;
                rep.NumeroIdentificacion = dto.RepresentanteNumeroId ?? rep.NumeroIdentificacion;
                rep.CiudadExpedicion = dto.CiudadExpedicion;
                rep.CiudadResidencia = dto.CiudadResidencia;

                if (!string.IsNullOrWhiteSpace(dto.RepresentanteTipoId) &&
                    Enum.TryParse<TipoDocumento>(dto.RepresentanteTipoId, ignoreCase: true, out var tipo))
                    rep.TipoDocumento = tipo;

                if (rep.Id == 0) _db.RepresentantesLegales.Add(rep);
            }

            // ConfiguracionDian
            var config = negocio.ConfiguracionDIAN ?? new ConfiguracionDian { NegocioId = negocio.Id };
            if (!string.IsNullOrWhiteSpace(dto.CorreoAcceso))
                config.SoftwareProveedor = dto.CorreoAcceso;
            config.FechaActualizacion = DateTime.UtcNow;
            if (config.Id == 0) _db.ConfiguracionesDian.Add(config);

            await _db.SaveChangesAsync();
        }

        public async Task GuardarSoftwareAsync(int usuarioId, ConfiguracionSoftwareDto dto)
        {
            var negocio = await ObtenerNegocioOThrowAsync(usuarioId);

            var config = await _db.ConfiguracionesDian
                .FirstOrDefaultAsync(c => c.NegocioId == negocio.Id)
                ?? new ConfiguracionDian { NegocioId = negocio.Id };

            config.SoftwareProveedor = dto.NitFabricante;
            config.SoftwarePIN = dto.CodigoSoftware;
            config.FechaActualizacion = DateTime.UtcNow;

            if (config.Id == 0) _db.ConfiguracionesDian.Add(config);
            await _db.SaveChangesAsync();
        }

        // ══════════════════════════════════════════════════════════════
        //  PASO 2 — Certificado digital
        // ══════════════════════════════════════════════════════════════
        public async Task<CertificadoResponseDto> GuardarCertificadoAsync(
            int usuarioId, CertificadoDto dto)
        {
            var negocio = await ObtenerNegocioOThrowAsync(usuarioId);

            var cert = await _db.CertificadosDigitales
                .FirstOrDefaultAsync(c => c.NegocioId == negocio.Id)
                ?? new CertificadoDigital { NegocioId = negocio.Id };

            if (dto.Opcion == "propio")
            {
                if (string.IsNullOrWhiteSpace(dto.NombreArchivo))
                    throw new ArgumentException("Debes subir el archivo del certificado (.p12/.pfx).");
                if (string.IsNullOrWhiteSpace(dto.PasswordCertificado))
                    throw new ArgumentException("La contraseña del certificado es requerida.");

                cert.UsaCertificadoPropio = true;
                cert.UsaCertificadoNubee = false;
                cert.NombreArchivo = dto.NombreArchivo;
                // Hashear la contraseña con PBKDF2 antes de persistir
                cert.PasswordHash = HashearPassword(dto.PasswordCertificado);
                cert.FechaAceptacionCarta = null;
                cert.VersionCartaAceptada = null;
            }
            else if (dto.Opcion == "nubee")
            {
                if (!dto.AceptarExoneracion)
                    throw new ArgumentException("Debes aceptar la carta de exoneración para continuar.");

                cert.UsaCertificadoPropio = false;
                cert.UsaCertificadoNubee = true;
                cert.FechaAceptacionCarta = DateTime.UtcNow;
                cert.VersionCartaAceptada = dto.VersionCarta ?? "v1.0";
                cert.RutaCifrada = null;
                cert.PasswordHash = null;
                cert.NombreArchivo = null;
            }
            else
            {
                throw new ArgumentException($"Opción de certificado inválida: {dto.Opcion}");
            }

            cert.FechaActualizacion = DateTime.UtcNow;
            if (cert.Id == 0) _db.CertificadosDigitales.Add(cert);
            await _db.SaveChangesAsync();

            return new CertificadoResponseDto
            {
                Mensaje = "Certificado guardado correctamente.",
                UsaCertificadoPropio = cert.UsaCertificadoPropio,
                UsaCertificadoNubee = cert.UsaCertificadoNubee,
                NombreArchivo = cert.NombreArchivo,
                FechaAceptacionCarta = cert.FechaAceptacionCarta,
            };
        }

        public async Task<CertificadoResponseDto?> ObtenerCertificadoAsync(int usuarioId)
        {
            var negocio = await ObtenerNegocioOThrowAsync(usuarioId);
            var cert = await _db.CertificadosDigitales
                .FirstOrDefaultAsync(c => c.NegocioId == negocio.Id);

            if (cert == null) return null;

            return new CertificadoResponseDto
            {
                Mensaje = "OK",
                UsaCertificadoPropio = cert.UsaCertificadoPropio,
                UsaCertificadoNubee = cert.UsaCertificadoNubee,
                NombreArchivo = cert.NombreArchivo,
                FechaAceptacionCarta = cert.FechaAceptacionCarta,
            };
        }

        // ══════════════════════════════════════════════════════════════
        //  PASO 3 — Test Set DIAN
        //  CORRECCIÓN: TestSetId se guarda en su propio campo,
        //  NO en AmbienteDIAN (que es int 1/2).
        // ══════════════════════════════════════════════════════════════
        public async Task GuardarTestSetAsync(int usuarioId, TestSetDto dto)
        {
            var negocio = await ObtenerNegocioOThrowAsync(usuarioId);

            var config = await _db.ConfiguracionesDian
                .FirstOrDefaultAsync(c => c.NegocioId == negocio.Id)
                ?? throw new InvalidOperationException("Registra primero los datos del software (paso 1).");

            config.TestSetId = dto.TestSetId;
            config.FechaActualizacion = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        // ══════════════════════════════════════════════════════════════
        //  PASO 4 — Resolución DIAN
        // ══════════════════════════════════════════════════════════════
        public async Task<ResolucionDIAN> GuardarResolucionAsync(
            int usuarioId, ResolucionDianDto dto)
        {
            if (!DateTime.TryParse(dto.FechaInicio, out var fechaInicio))
                throw new ArgumentException("Formato de fecha de inicio inválido.");
            if (!DateTime.TryParse(dto.FechaFin, out var fechaFin))
                throw new ArgumentException("Formato de fecha de fin inválido.");
            if (fechaFin <= fechaInicio)
                throw new ArgumentException("La fecha de fin debe ser posterior a la de inicio.");

            var negocio = await _db.Negocios
                .Include(n => n.Resoluciones)
                .FirstOrDefaultAsync(n => n.UsuarioId == usuarioId)
                ?? throw new InvalidOperationException("Negocio no encontrado.");

            // Desactivar resoluciones anteriores
            foreach (var ant in negocio.Resoluciones.Where(r => r.Activa))
                ant.Activa = false;

            var nueva = new ResolucionDIAN
            {
                NegocioId = negocio.Id,
                NumeroAutorizacion = dto.NumeroAutorizacion,
                Prefijo = dto.Prefijo,
                RangoDesde = dto.RangoDesde,
                RangoHasta = dto.RangoHasta,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                ClaveTecnica = dto.ClaveTecnica,
                TipoAmbiente = int.Parse(dto.TipoAmbiente),
                Activa = true,
                FechaRegistro = DateTime.UtcNow,
            };

            _db.ResolucionesDIAN.Add(nueva);
            await _db.SaveChangesAsync();

            _logger.LogInformation(
                "Resolución {NumAut} registrada para negocio {Id}",
                nueva.NumeroAutorizacion, negocio.Id);

            return nueva;
        }

        // ══════════════════════════════════════════════════════════════
        //  PASO 4b — Registrar rango en Factus
        // ══════════════════════════════════════════════════════════════
        public async Task<object> RegistrarRangoFactusAsync(
            int usuarioId, RegistrarRangoDto dto)
        {
            var resolucion = await _db.ResolucionesDIAN
                .Include(r => r.Negocio)
                .FirstOrDefaultAsync(r => r.Id == dto.ResolucionId
                                       && r.Negocio!.UsuarioId == usuarioId)
                ?? throw new KeyNotFoundException("Resolución no encontrada.");

            if (!resolucion.EstaVigente)
                throw new InvalidOperationException(
                    $"La resolución venció el {resolucion.FechaFin:dd/MM/yyyy}. Código: RESOLUCION_VENCIDA");

            if (resolucion.FactusRangoId.HasValue && !dto.Forzar)
                return new
                {
                    mensaje = "Esta resolución ya está registrada en Factus.",
                    factusRangoId = resolucion.FactusRangoId,
                    yaRegistrada = true,
                };

            var rangoId = await _factus.RegistrarRangoAsync(resolucion, resolucion.Negocio!);
            resolucion.FactusRangoId = rangoId;
            await _db.SaveChangesAsync();

            _logger.LogInformation(
                "Negocio NIT {Nit} habilitado en Factus. RangoId={RangoId}",
                resolucion.Negocio!.Nit, rangoId);

            return new
            {
                mensaje = "Empresa habilitada en Factus. Ya puede emitir facturas.",
                factusRangoId = rangoId,
                prefijo = resolucion.Prefijo,
                rangoDesde = resolucion.RangoDesde,
                rangoHasta = resolucion.RangoHasta,
            };
        }

        // ══════════════════════════════════════════════════════════════
        //  PASO 5 — Rango activo (lectura)
        // ══════════════════════════════════════════════════════════════
        public async Task<RangoActivoResponseDto?> ObtenerRangoActivoAsync(int usuarioId)
        {
            var negocio = await _db.Negocios
                .Include(n => n.Resoluciones)
                .FirstOrDefaultAsync(n => n.UsuarioId == usuarioId);

            var resolucion = negocio?.ResolucionActiva;
            if (resolucion == null) return null;

            long? currentFactus = null;

            // Consultar consecutivo actual en Factus si existe el rango
            if (resolucion.FactusRangoId.HasValue)
            {
                try
                {
                    var rango = await _factus.ObtenerRangoActivoAsync(resolucion.FactusRangoId.Value);
                    currentFactus = rango?.current;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex,
                        "No se pudo obtener el consecutivo actual de Factus para rango {Id}",
                        resolucion.FactusRangoId);
                }
            }

            string estado = resolucion.FactusRangoId.HasValue
                ? (resolucion.EstaVigente ? "HABILITADO" : "RESOLUCION_VENCIDA")
                : "PENDIENTE_FACTUS";

            return new RangoActivoResponseDto
            {
                Prefijo = resolucion.Prefijo,
                RangoDesde = resolucion.RangoDesde,
                RangoHasta = resolucion.RangoHasta,
                CurrentFactus = currentFactus,
                Estado = estado,
                FactusRangoId = resolucion.FactusRangoId,
                DiasRestantes = resolucion.DiasRestantes,
                FechaFin = resolucion.FechaFin.ToString("yyyy-MM-dd"),
            };
        }

        // ══════════════════════════════════════════════════════════════
        //  PASO 6 — Estado y finalizar habilitación
        // ══════════════════════════════════════════════════════════════
        public async Task<EstadoHabilitacionDto> ObtenerEstadoAsync(int usuarioId)
        {
            var negocio = await _db.Negocios
                .Include(n => n.Resoluciones)
                .Include(n => n.CertificadoDigital)
                .Include(n => n.ConfiguracionDIAN)
                .FirstOrDefaultAsync(n => n.UsuarioId == usuarioId);

            if (negocio == null)
                return new EstadoHabilitacionDto
                {
                    TieneNegocio = false,
                    PasoActual = "SIN_NEGOCIO",
                    PorcentajeCompletado = 0,
                };

            var cert = negocio.CertificadoDigital;
            var config = negocio.ConfiguracionDIAN;
            var resolucion = negocio.ResolucionActiva;

            bool tieneCert = cert is { UsaCertificadoPropio: true } or { UsaCertificadoNubee: true };
            bool tieneTestSet = !string.IsNullOrWhiteSpace(config?.TestSetId);
            bool tieneResolucion = resolucion != null;
            bool resolucionVigente = resolucion?.EstaVigente ?? false;
            bool rangoFactus = resolucion?.FactusRangoId != null;
            bool habilitado = negocio.HabilitacionCompleta;
            bool empresaSincronizada = negocio.DatosFacturacionCompletos;

            // Determinar paso actual
            string paso = !empresaSincronizada ? "SIN_EMPRESA"
                        : !tieneCert ? "SIN_CERTIFICADO"
                        : !tieneTestSet ? "SIN_TEST_SET"
                        : !tieneResolucion ? "SIN_RESOLUCION"
                        : !resolucionVigente ? "RESOLUCION_VENCIDA"
                        : !rangoFactus ? "PENDIENTE_FACTUS"
                        : "HABILITADO";

            // Porcentaje: 6 pasos
            int completados = new[]
            {
                empresaSincronizada, tieneCert, tieneTestSet,
                tieneResolucion, rangoFactus, habilitado
            }.Count(v => v);

            return new EstadoHabilitacionDto
            {
                TieneNegocio = true,
                Nit = negocio.Nit,
                RazonSocial = negocio.RazonSocial,
                EmpresaSincronizada = empresaSincronizada,
                TieneCertificado = tieneCert,
                TieneTestSet = tieneTestSet,
                TieneResolucion = tieneResolucion,
                ResolucionVigente = resolucionVigente,
                RangoCreadoEnFactus = rangoFactus,
                HabilitacionCompleta = habilitado,
                PasoActual = paso,
                PorcentajeCompletado = (int)Math.Round(completados / 6.0 * 100),
                FactusRangoId = resolucion?.FactusRangoId,
                Resolucion = resolucion == null ? null : new ResolucionDetalleDto
                {
                    Id = resolucion.Id,
                    NumeroAutorizacion = resolucion.NumeroAutorizacion,
                    Prefijo = resolucion.Prefijo,
                    RangoDesde = resolucion.RangoDesde,
                    RangoHasta = resolucion.RangoHasta,
                    FechaInicio = resolucion.FechaInicio.ToString("yyyy-MM-dd"),
                    FechaFin = resolucion.FechaFin.ToString("yyyy-MM-dd"),
                    TipoAmbiente = resolucion.TipoAmbiente,
                    DiasRestantes = resolucion.DiasRestantes,
                },
            };
        }

        public async Task FinalizarHabilitacionAsync(int usuarioId)
        {
            var negocio = await _db.Negocios
                .Include(n => n.Resoluciones)
                .Include(n => n.CertificadoDigital)
                .Include(n => n.ConfiguracionDIAN)
                .FirstOrDefaultAsync(n => n.UsuarioId == usuarioId)
                ?? throw new InvalidOperationException("Negocio no encontrado.");

            var resolucion = negocio.ResolucionActiva;

            if (resolucion == null)
                throw new InvalidOperationException("No tienes una resolución DIAN registrada.");

            if (!resolucion.EstaVigente)
                throw new InvalidOperationException("La resolución DIAN está vencida.");

            if (!resolucion.FactusRangoId.HasValue)
                throw new InvalidOperationException(
                    "El rango no está registrado en Factus. Completa el paso 4.");

            // Verificar que la empresa exista en Factus
            var empresaOk = await _factus.VerificarEmpresaAsync(negocio.Nit ?? "");
            if (!empresaOk)
                throw new InvalidOperationException(
                    "No se encontró la empresa en Factus. Verifica la sincronización del perfil.");

            // Marcar como habilitado
            negocio.HabilitacionCompleta = true;
            await _db.SaveChangesAsync();

            _logger.LogInformation(
                "Habilitación completada para negocio NIT {Nit}", negocio.Nit);
        }

        // ══════════════════════════════════════════════════════════════
        //  Helpers privados
        // ══════════════════════════════════════════════════════════════
        private async Task<Negocio> ObtenerNegocioOThrowAsync(int usuarioId) =>
            await _db.Negocios.FirstOrDefaultAsync(n => n.UsuarioId == usuarioId)
            ?? throw new InvalidOperationException("No tienes un negocio registrado.");

        private PerfilEmpresaDto MapearPerfilDto(Negocio n)
        {
            var p = n.PerfilTributario;
            var rep = n.RepresentanteLegal;

            List<string> parseJson(string? json)
            {
                if (string.IsNullOrWhiteSpace(json)) return new();
                try { return JsonSerializer.Deserialize<List<string>>(json) ?? new(); }
                catch { return new(); }
            }

            return new PerfilEmpresaDto
            {
                TipoPersona = n.TipoPersona == "PersonaNatural" ? "persona_natural" : "empresa",
                TipoIdentificacion = n.Nit != null ? "NIT" : "CC",
                NumeroIdentificacion = n.Nit,
                Dv = n.DvNit,
                RazonSocial = n.RazonSocial,
                NombreComercial = n.NombreNegocio,
                Correo = n.Correo,
                Direccion = n.Direccion,
                Ciudad = n.Ciudad,
                Departamento = n.Departamento,
                Telefono = n.Telefono,
                RegimenIvaCodigo = p?.RegimenIvaCodigo,
                ActividadEconomicaCIIU = p?.ActividadEconomicaCIIU,
                Tributos = parseJson(p?.TributosJson),
                ResponsabilidadesFiscales = parseJson(p?.ResponsabilidadesFiscalesJson),
                RepresentanteNombre = rep?.Nombre,
                RepresentanteApellidos = rep?.Apellidos,
                RepresentanteTipoId = rep?.TipoDocumento.ToString(),
                RepresentanteNumeroId = rep?.NumeroIdentificacion,
                CiudadExpedicion = rep?.CiudadExpedicion,
                CiudadResidencia = rep?.CiudadResidencia,
                CorreoAcceso = n.ConfiguracionDIAN?.SoftwareProveedor,
            };
        }

        /// <summary>Hashea la contraseña con PBKDF2 + salt aleatorio.</summary>
        private static string HashearPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            byte[] hash = KeyDerivation.Pbkdf2(
                password, salt,
                KeyDerivationPrf.HMACSHA256,
                iterationCount: 100_000,
                numBytesRequested: 32);

            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }
    }
}
