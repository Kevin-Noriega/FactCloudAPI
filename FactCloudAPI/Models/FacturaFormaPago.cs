using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NubeeAPI.Models
{
    /// <summary>
    /// Línea de forma de pago de una factura → un elemento de payment_details en Factus V2.
    /// Permite registrar el desglose de pagos (varios medios) que envía el frontend,
    /// en vez de colapsarlo a un único pago con el total.
    /// </summary>
    public class FacturaFormaPago
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int FacturaId { get; set; }
        [ForeignKey("FacturaId")]
        public Factura? Factura { get; set; }

        /// <summary>Código DIAN del medio de pago (payment_method_code): "10","42","48",…</summary>
        [Required]
        [MaxLength(10)]
        public string MetodoPagoCodigo { get; set; } = "10";

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }
    }
}
