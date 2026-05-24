using NubeeAPI.Models.Impuestos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NubeeAPI.Models
{
    public class DetalleFactura
    {
        [Key]
        public int Id { get; set; }

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

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal SubtotalLinea { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal TotalLinea { get; set; }

        [MaxLength(10)]
        public string? CodigoUNSPSC { get; set; }

        [MaxLength(50)]
        public string? CodigoInterno { get; set; }

        public ICollection<DocumentoLineaImpuesto> Impuestos { get; set; }
            = new List<DocumentoLineaImpuesto>();

        [NotMapped]
        public decimal TotalImpuestosTrasladados =>
            Impuestos
                .Where(i => i.Naturaleza == NaturalezaFiscal.Trasladado)
                .Sum(i => i.ValorCalculado);

        [NotMapped]
        public decimal TotalImpuestosDescontables =>
            Impuestos
                .Where(i => i.Naturaleza == NaturalezaFiscal.Descontable)
                .Sum(i => i.ValorCalculado);

        [NotMapped]
        public decimal TotalRetenciones =>
            Impuestos
                .Where(i => i.Naturaleza == NaturalezaFiscal.Retenido
                         || i.Naturaleza == NaturalezaFiscal.Autorretenido)
                .Sum(i => i.ValorCalculado);

        public void Recalcular()
        {
            SubtotalLinea = (Cantidad * PrecioUnitario) - ValorDescuento;

            TotalLinea = SubtotalLinea
                       + TotalImpuestosTrasladados
                       - TotalRetenciones;
        }
    }
}