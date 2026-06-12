using System.ComponentModel.DataAnnotations;

namespace NubeeAPI.Models.Usuarios
{
    public class ConfiguracionDian
    {
        [Key]
        public int Id { get; set; }

        // FK
        public int NegocioId { get; set; }
        public Negocio Negocio { get; set; }

        // Datos de software
        public string? SoftwareProveedor { get; set; }
        public string? SoftwarePIN { get; set; }

        // Resolución / numeración actuales (lo que ya tenías)
        public string? PrefijoAutorizadoDIAN { get; set; }
        public string? NumeroResolucionDIAN { get; set; }
        public string? RangoNumeracionDesde { get; set; }
        public string? RangoNumeracionHasta { get; set; }
        public DateTime? FechaVigenciaInicio { get; set; }
        public DateTime? FechaVigenciaFinal { get; set; }

        // Nuevo: TestSet y Ambiente tipado
        public string? TestSetId { get; set; }

        /// <summary>
        /// 1 = Producción, 2 = Habilitación/Sandbox.
        /// </summary>
        public int AmbienteDIAN { get; set; } = 2;

        // Auditoría opcional
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaActualizacion { get; set; }
    }
    // ══════════════════════════════════════════════════════════════════
    //  ENTIDAD: CertificadoDigital
    //  Almacena la configuración del certificado digital para firma
    //  de documentos electrónicos. Nunca exponer RutaCifrada/PasswordHash
    //  en DTOs de respuesta.
    // ══════════════════════════════════════════════════════════════════
    public class CertificadoDigital
    {
        public int Id { get; set; }

        /// <summary>FK al negocio dueño del certificado.</summary>
        public int NegocioId { get; set; }
        public Negocio? Negocio { get; set; }

        // ── Opción A: certificado propio ──────────────────────────────
        /// <summary>true cuando el usuario sube su propio .p12/.pfx</summary>
        public bool UsaCertificadoPropio { get; set; }

        /// <summary>Ruta cifrada (AES-256) del archivo almacenado en servidor/blob.</summary>
        public string? RutaCifrada { get; set; }

        /// <summary>Hash PBKDF2 de la contraseña; NUNCA la contraseña en texto plano.</summary>
        public string? PasswordHash { get; set; }

        /// <summary>Nombre original del archivo (.p12 / .pfx) para mostrar en UI.</summary>
        public string? NombreArchivo { get; set; }

        // ── Opción B: certificado de Nubee ────────────────────────────
        /// <summary>true cuando el usuario acepta usar el cert de Nubee.</summary>
        public bool UsaCertificadoNubee { get; set; }

        /// <summary>Fecha en que se aceptó la carta de exoneración (UTC).</summary>
        public DateTime? FechaAceptacionCarta { get; set; }

        /// <summary>Versión del texto de la carta que se aceptó (ej. "v1.0").</summary>
        public string? VersionCartaAceptada { get; set; }

        // ── Auditoría ─────────────────────────────────────────────────
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaActualizacion { get; set; }
    }
}
