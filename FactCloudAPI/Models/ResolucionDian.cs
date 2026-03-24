using FactCloudAPI.Models.Usuarios;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactCloudAPI.Models
{
    public class ResolucionDIAN
    {
        [Key]
        public int Id { get; set; }

        // FK al negocio (el NIT autorizado por la DIAN)
        [Required]
        public int NegocioId { get; set; }
        public Negocio? Negocio { get; set; }

        /// <summary>stsInvoiceAuthorization — 14 dígitos exactos</summary>
        [Required, StringLength(14, MinimumLength = 14)]
        public string NumeroAutorizacion { get; set; } = string.Empty;

        /// <summary>stsPrefix — máx 4 chars según DIAN</summary>
        [MaxLength(4)]
        public string? Prefijo { get; set; }

        /// <summary>stsStartDate</summary>
        [Required]
        public DateTime FechaInicio { get; set; }

        /// <summary>stsEndDate</summary>
        [Required]
        public DateTime FechaFin { get; set; }

        /// <summary>stsFrom — número inicial del rango</summary>
        [Required]
        public long RangoDesde { get; set; }

        /// <summary>stTo — número final del rango</summary>
        [Required]
        public long RangoHasta { get; set; }

        /// <summary>
        /// Clave técnica asignada por la DIAN para calcular el CUFE.
        /// NUNCA va en el XML. Guardar cifrada en producción.
        /// </summary>
        [MaxLength(200)]
        public string? ClaveTecnica { get; set; }

        /// <summary>1 = Producción, 2 = Habilitación/Pruebas</summary>
        [Required, Range(1, 2)]
        public int TipoAmbiente { get; set; } = 2;

        public bool Activa { get; set; } = true;

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // ── Propiedades calculadas ────────────────────────────────────────
        [NotMapped]
        public bool EstaVigente =>
            Activa && DateTime.Now >= FechaInicio && DateTime.Now <= FechaFin;

        [NotMapped]
        public int DiasRestantes =>
            (int)(FechaFin - DateTime.Now).TotalDays;
    }
}
