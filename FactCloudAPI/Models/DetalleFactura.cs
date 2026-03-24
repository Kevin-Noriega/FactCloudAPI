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

        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        [Required, MaxLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        [Required, MaxLength(50), DefaultValue("Unidad")]
        public string UnidadMedida { get; set; } = "Unidad";

        [Required, Column(TypeName = "decimal(12,6)")]
        public decimal Cantidad { get; set; } = 1;

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        [Column(TypeName = "decimal(6,4)")]
        public decimal PorcentajeDescuento { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorDescuento { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal SubtotalLinea { get; set; }

        [Column(TypeName = "decimal(6,4)")]
        public decimal TarifaIVA { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorIVA { get; set; }

        [Column(TypeName = "decimal(6,4)")]
        public decimal TarifaINC { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorINC { get; set; }

        [Column(TypeName = "decimal(6,4)")]
        public decimal TarifaICA { get; set; } = 0;  // ✅ Nuevo

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorICA { get; set; } = 0;   // ✅ Nuevo

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalLinea { get; set; }

        [MaxLength(10)]
        public string? CodigoUNSPSC { get; set; }

        [MaxLength(50)]
        public string? CodigoInterno { get; set; }
    }
}
