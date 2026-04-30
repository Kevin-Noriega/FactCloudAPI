using FactCloudAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactCloudAPI.Models.Impuestos
{
    /// <summary>
    /// Tabla intermedia para manejar múltiples impuestos por línea de factura.
    /// Permite que un DetalleFactura tenga IVA + ReteICA + Retefuente simultáneamente.
    /// Requerido por el Anexo Técnico DIAN v1.9 (múltiples TaxSubtotal por línea).
    /// </summary>
    public class DetalleFacturaImpuesto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DetalleFacturaId { get; set; }
        public DetalleFactura? DetalleFactura { get; set; }

        [Required]
        public int ImpuestoId { get; set; }
        public Impuesto? Impuesto { get; set; }

        /// <summary>Base gravable sobre la que se calculó el impuesto en esta línea</summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BaseGravable { get; set; }

        /// <summary>Tarifa aplicada (puede diferir del Impuesto.Tarifa si se personalizó)</summary>
        [Required]
        [Column(TypeName = "decimal(7,4)")]
        public decimal TarifaAplicada { get; set; }

        /// <summary>Valor calculado del impuesto = BaseGravable * TarifaAplicada / 100</summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorImpuesto { get; set; }

        /// <summary>
        /// Indica si el impuesto suma (+) o resta (-) al total de la línea:
        /// "Cargo"     → IVA, INC, ICA (suma al valor a pagar)
        /// "Retencion" → Retefuente, ReteICA, ReteIVA (resta al valor a pagar)
        /// </summary>
        [Required]
        [MaxLength(15)]
        public string NaturalezaImpuesto { get; set; } = "Cargo";
    }
}
