using FactCloudAPI.Models.Impuestos;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactCloudAPI.Models
{
    public class DetalleFactura
    {
        [Key] public int Id { get; set; }

        [Required]
        public int FacturaId { get; set; }
        public Factura? Factura { get; set; }

        [Required]
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        [Required, MaxLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string UnidadMedida { get; set; } = "Unidad";

        [Required, Column(TypeName = "decimal(12,6)")]
        public decimal Cantidad { get; set; } = 1;

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        [Column(TypeName = "decimal(6,4)")]
        public decimal PorcentajeDescuento { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorDescuento { get; set; } = 0;

        /// <summary>Subtotal antes de impuestos = (Cantidad × PrecioUnitario) - ValorDescuento</summary>
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal SubtotalLinea { get; set; }

        /// <summary>
        /// Total de la línea incluyendo impuestos de cargo y descontando retenciones.
        /// = SubtotalLinea + SUM(Cargos) - SUM(Retenciones)
        /// </summary>
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal TotalLinea { get; set; }

        /// <summary>Código estándar de producto UNSPSC — requerido en XML DIAN cac:Item</summary>
        [MaxLength(10)]
        public string? CodigoUNSPSC { get; set; }

        [MaxLength(50)]
        public string? CodigoInterno { get; set; }

        // ── Relaciones ──────────────────────────────────────────
        /// <summary>
        /// Impuestos aplicados a esta línea (IVA, INC, ICA, Retefuente, etc.)
        /// Genera múltiples cac:TaxSubtotal en el XML DIAN v1.9
        /// </summary>
        public ICollection<DetalleFacturaImpuesto> Impuestos { get; set; } = new List<DetalleFacturaImpuesto>();

        // ── Propiedades calculadas (no mapeadas) ────────────────
        [NotMapped]
        public decimal TotalCargos =>
            Impuestos.Where(i => i.NaturalezaImpuesto == "Cargo")
                     .Sum(i => i.ValorImpuesto);

        [NotMapped]
        public decimal TotalRetenciones =>
            Impuestos.Where(i => i.NaturalezaImpuesto == "Retencion")
                     .Sum(i => i.ValorImpuesto);

        // ── Métodos ─────────────────────────────────────────────
        /// <summary>
        /// Recalcula SubtotalLinea y TotalLinea a partir de los valores base.
        /// Llamar siempre que cambie Cantidad, PrecioUnitario, ValorDescuento o Impuestos.
        /// </summary>
        public void Recalcular()
        {
            SubtotalLinea = (Cantidad * PrecioUnitario) - ValorDescuento;
            TotalLinea = SubtotalLinea + TotalCargos - TotalRetenciones;
        }
    }
}