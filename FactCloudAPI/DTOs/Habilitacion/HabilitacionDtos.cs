using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NubeeAPI.DTOs.Habilitacion
{
    // ══════════════════════════════════════════════════════════════════
    //  PASO 1 — Software / Configuración DIAN
    // ══════════════════════════════════════════════════════════════════
    public class ConfiguracionSoftwareDto
    {
        [Required(ErrorMessage = "El NIT del fabricante es requerido.")]
        public string NitFabricante { get; set; } = string.Empty;

        [Required(ErrorMessage = "El código de software es requerido.")]
        public string CodigoSoftware { get; set; } = string.Empty;
    }

    // ══════════════════════════════════════════════════════════════════
    //  PASO 2 — Certificado digital
    // ══════════════════════════════════════════════════════════════════

    /// <summary>DTO para guardar la opción de certificado elegida por el usuario.</summary>
    public class CertificadoDto
    {
        /// <summary>"propio" | "nubee"</summary>
        [Required]
        public string Opcion { get; set; } = string.Empty;

        // ── Opción "propio" ───────────────────────────────────────────
        /// <summary>Nombre del archivo (.p12/.pfx). La ruta real se gestiona en el servidor.</summary>
        public string? NombreArchivo { get; set; }

        /// <summary>Contraseña en texto plano (solo en tránsito; el backend la hashea inmediatamente).</summary>
        public string? PasswordCertificado { get; set; }

        // ── Opción "nubee" ────────────────────────────────────────────
        public bool AceptarExoneracion { get; set; }
        public string? VersionCarta { get; set; }
    }

    /// <summary>Respuesta del endpoint de certificado.</summary>
    public class CertificadoResponseDto
    {
        public string Mensaje { get; set; } = string.Empty;
        public bool UsaCertificadoPropio { get; set; }
        public bool UsaCertificadoNubee { get; set; }
        public string? NombreArchivo { get; set; }
        public DateTime? FechaAceptacionCarta { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════
    //  PASO 3 — Test Set DIAN
    // ══════════════════════════════════════════════════════════════════
    public class TestSetDto
    {
        [Required(ErrorMessage = "El código del set de pruebas es requerido.")]
        [MinLength(5, ErrorMessage = "El TestSetId debe tener al menos 5 caracteres.")]
        public string TestSetId { get; set; } = string.Empty;

        /// <summary>Número de resolución del set de pruebas (para registro interno).</summary>
        public string? ResolucionPrueba { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════
    //  PASO 4 — Resolución DIAN + Rango Factus
    // ══════════════════════════════════════════════════════════════════
    public class ResolucionDianDto
    {
        [Required(ErrorMessage = "El número de autorización es requerido.")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "El número de autorización debe tener exactamente 14 dígitos.")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "El número de autorización solo debe contener dígitos.")]
        public string NumeroAutorizacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El prefijo es requerido.")]
        [RegularExpression(@"^[a-zA-Z]{1,4}$", ErrorMessage = "El prefijo debe ser alfabético y tener entre 1 y 4 caracteres.")]
        public string Prefijo { get; set; } = string.Empty;

        [Range(1, long.MaxValue, ErrorMessage = "El rango desde debe ser mayor a 0.")]
        public long RangoDesde { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "El rango hasta debe ser mayor a 0.")]
        public long RangoHasta { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es requerida.")]
        public string FechaInicio { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de fin es requerida.")]
        public string FechaFin { get; set; } = string.Empty;

        public string? ClaveTecnica { get; set; }

        [Required(ErrorMessage = "El tipo de ambiente es requerido.")]
        [RegularExpression(@"^[12]$", ErrorMessage = "El tipo de ambiente debe ser 1 (Producción) o 2 (Habilitación).")]
        public string TipoAmbiente { get; set; } = "1";
    }

    public class RegistrarRangoDto
    {
        [Required]
        public int ResolucionId { get; set; }

        /// <summary>true para forzar re-registro aunque ya exista FactusRangoId.</summary>
        public bool Forzar { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════
    //  PASO 5 — Rango activo (solo lectura)
    // ══════════════════════════════════════════════════════════════════
    public class RangoActivoResponseDto
    {
        public string? Prefijo { get; set; }
        public long RangoDesde { get; set; }
        public long RangoHasta { get; set; }
        public long? CurrentFactus { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int? FactusRangoId { get; set; }
        public int DiasRestantes { get; set; }
        public string FechaFin { get; set; } = string.Empty;
    }

    // ══════════════════════════════════════════════════════════════════
    //  PASO 6 — Estado de habilitación
    // ══════════════════════════════════════════════════════════════════
    public class EstadoHabilitacionDto
    {
        public bool TieneNegocio { get; set; }
        public string? Nit { get; set; }
        public string? RazonSocial { get; set; }

        // Indicadores por paso
        public bool EmpresaSincronizada { get; set; }
        public bool TieneCertificado { get; set; }
        public bool TieneTestSet { get; set; }
        public bool TieneResolucion { get; set; }
        public bool ResolucionVigente { get; set; }
        public bool RangoCreadoEnFactus { get; set; }
        public bool HabilitacionCompleta { get; set; }

        /// <summary>
        /// Paso actual del wizard:
        /// SIN_NEGOCIO | SIN_CERTIFICADO | SIN_TEST_SET | SIN_RESOLUCION |
        /// RESOLUCION_VENCIDA | PENDIENTE_FACTUS | HABILITADO
        /// </summary>
        public string PasoActual { get; set; } = string.Empty;

        public int PorcentajeCompletado { get; set; }

        // Detalle de la resolución activa
        public ResolucionDetalleDto? Resolucion { get; set; }
        public int? FactusRangoId { get; set; }
    }

    public class ResolucionDetalleDto
    {
        public int Id { get; set; }
        public string NumeroAutorizacion { get; set; } = string.Empty;
        public string? Prefijo { get; set; }
        public long RangoDesde { get; set; }
        public long RangoHasta { get; set; }
        public string FechaInicio { get; set; } = string.Empty;
        public string FechaFin { get; set; } = string.Empty;
        public int TipoAmbiente { get; set; }
        public int DiasRestantes { get; set; }
    }

 

    // ══════════════════════════════════════════════════════════════════
    //  FACTUS — DTOs de integración
    // ══════════════════════════════════════════════════════════════════
    public class FactusRegistrarRangoRequestDto
    {
        /// <summary>Código de tipo de documento Factus: 21 = Factura electrónica.</summary>
        public int document { get; set; } = 21;
        public string? prefix { get; set; }
        public string? resolutionnumber { get; set; }
        public long current { get; set; }
    }

    public class FactusRegistrarRangoResponseDto
    {
        public bool success { get; set; }
        public FactusRangoDataDto? data { get; set; }
        public string? message { get; set; }
    }

    public class FactusRangoDataDto
    {
        public int id { get; set; }
        public string? document { get; set; }
        public string? prefix { get; set; }
        public string? from { get; set; }
        public string? to { get; set; }
        public string? current { get; set; }
    }

    public class FactusRangoActivoDto
    {
        public int id { get; set; }
        public string? prefix { get; set; }
        public long from { get; set; }
        public long to { get; set; }
        public long current { get; set; }
        public bool active { get; set; }
    }
}
