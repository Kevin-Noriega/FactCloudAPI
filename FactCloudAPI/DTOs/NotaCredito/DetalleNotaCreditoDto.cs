using System.ComponentModel.DataAnnotations;

namespace FactCloudAPI.DTOs.NotaCredito
{
    public class DetalleNotaCreditoDto
    {
        [Required]
        public int ProductoId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public decimal Cantidad { get; set; }

        [Required]
        [MaxLength(50)]
        public string UnidadMedida { get; set; } = "Unidad";

        [Required]
        public decimal PrecioUnitario { get; set; }

        public decimal PorcentajeDescuento { get; set; }
        public decimal ValorDescuento { get; set; }
        public decimal SubtotalLinea { get; set; }
        public decimal TarifaIVA { get; set; }
        public decimal ValorIVA { get; set; }
        public decimal TarifaINC { get; set; }
        public decimal ValorINC { get; set; }
        public decimal TotalLinea { get; set; }
    }
}
