using System.ComponentModel.DataAnnotations;

namespace FactCloudAPI.DTOs.Impuestos
{
    // ─── READ ────────────────────────────────────────────────────────────────
    public class AutoretencionDto
    {
        public int Id { get; set; }
        public int Codigo { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string TipoAutoretencion { get; set; } = string.Empty;
        public decimal Tarifa { get; set; }
        public string TarifaDisplay { get; set; } = string.Empty;
        public decimal? BaseMinimaAplicacion { get; set; }
        public string TipoBase { get; set; } = string.Empty;
        public bool EnUso { get; set; }

        public CuentaContableResumenDto? CuentaDebito { get; set; }
        public CuentaContableResumenDto? CuentaCredito { get; set; }
    }

    // ─── CREATE ──────────────────────────────────────────────────────────────
    public class CrearAutoretencionDto
    {
        [Required]
        public int Codigo { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(Autoretención 2201|Autoretención ICA)$",
            ErrorMessage = "TipoAutoretencion debe ser 'Autoretención 2201' o 'Autoretención ICA'")]
        public string TipoAutoretencion { get; set; } = "Autoretención 2201";

        [Required]
        [Range(0, 100, ErrorMessage = "La tarifa debe estar entre 0 y 100")]
        public decimal Tarifa { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? BaseMinimaAplicacion { get; set; }

        [MaxLength(10)]
        public string TipoBase { get; set; } = "Pesos";

        // Cuentas PUC
        public int? CuentaDebitoId { get; set; }
        public int? CuentaCreditoId { get; set; }
    }

    // ─── UPDATE ──────────────────────────────────────────────────────────────
    public class ActualizarAutoretencionDto
    {
        [MaxLength(100)]
        public string? Nombre { get; set; }

        [Range(0, 100)]
        public decimal? Tarifa { get; set; }

        public decimal? BaseMinimaAplicacion { get; set; }
        public bool? EnUso { get; set; }

        public int? CuentaDebitoId { get; set; }
        public int? CuentaCreditoId { get; set; }
    }
}
