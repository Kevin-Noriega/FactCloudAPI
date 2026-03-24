using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactCloudAPI.Models
{
    public class DetalleNotaDebito
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int NotaDebitoId { get; set; }
        [ForeignKey("NotaDebitoId")]
        public NotaDebito? NotaDebito { get; set; }

        [Required]
        public int ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public Producto? Producto { get; set; }

        [Required]
        [MaxLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Cantidad { get; set; }

        [Required]
        [MaxLength(50)]
        public string UnidadMedida { get; set; } = "Unidad";

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PorcentajeDescuento { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorDescuento { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SubtotalLinea { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TarifaIVA { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorIVA { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TarifaINC { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorINC { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalLinea { get; set; }
    }

}
