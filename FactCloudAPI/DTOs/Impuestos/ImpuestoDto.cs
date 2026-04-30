using System.ComponentModel.DataAnnotations;

namespace NubeeAPI.DTOs.Impuestos
{
    // --- READ ----------------------------------------------------------------
    public class ImpuestoDto
    {
        public int Id { get; set; }
        public int Codigo { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string TipoImpuesto { get; set; } = string.Empty;
        public decimal Tarifa { get; set; }
        public bool PorValor { get; set; }
        public string TarifaDisplay { get; set; } = string.Empty;
        public string? CodigoTributoDIAN { get; set; }
        public bool EnUso { get; set; }

        // Cuentas PUC resumidas
        public CuentaContableResumenDto? CuentaDebitoVentas { get; set; }
        public CuentaContableResumenDto? CuentaCreditoVentas { get; set; }
        public CuentaContableResumenDto? CuentaDebitoCompras { get; set; }
        public CuentaContableResumenDto? CuentaCreditoCompras { get; set; }
        public CuentaContableResumenDto? CuentaDevolucionVentas { get; set; }
        public CuentaContableResumenDto? CuentaDevolucionCompras { get; set; }
    }

    // --- CREATE --------------------------------------------------------------
    public class CrearImpuestoDto
    {
        [Required]
        public int Codigo { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(IVA|INC|ICA|Retefuente|ReteICA|ReteIVA)$",
            ErrorMessage = "TipoImpuesto debe ser: IVA, INC, ICA, Retefuente, ReteICA o ReteIVA")]
        public string TipoImpuesto { get; set; } = string.Empty;

        [Required]
        [Range(0, 100, ErrorMessage = "La tarifa debe estar entre 0 y 100")]
        public decimal Tarifa { get; set; }

        public bool PorValor { get; set; } = false;

        [MaxLength(2)]
        public string? CodigoTributoDIAN { get; set; }

        // IDs de cuentas PUC (opcionales al crear, recomendados)
        public int? CuentaDebitoVentasId { get; set; }
        public int? CuentaCreditoVentasId { get; set; }
        public int? CuentaDebitoComprasId { get; set; }
        public int? CuentaCreditoComprasId { get; set; }
        public int? CuentaDevolucionVentasId { get; set; }
        public int? CuentaDevolucionComprasId { get; set; }
    }

    // --- UPDATE --------------------------------------------------------------
    public class ActualizarImpuestoDto
    {
        [MaxLength(100)]
        public string? Nombre { get; set; }

        [Range(0, 100)]
        public decimal? Tarifa { get; set; }

        public bool? PorValor { get; set; }
        public bool? EnUso { get; set; }

        public int? CuentaDebitoVentasId { get; set; }
        public int? CuentaCreditoVentasId { get; set; }
        public int? CuentaDebitoComprasId { get; set; }
        public int? CuentaCreditoComprasId { get; set; }
        public int? CuentaDevolucionVentasId { get; set; }
        public int? CuentaDevolucionComprasId { get; set; }
    }

    // --- SHARED RESUMEN ------------------------------------------------------
    public class CuentaContableResumenDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string CodigoNombre { get; set; } = string.Empty;
    }
}
