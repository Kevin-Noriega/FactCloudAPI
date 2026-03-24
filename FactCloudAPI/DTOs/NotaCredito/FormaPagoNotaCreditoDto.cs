using System.ComponentModel.DataAnnotations;

namespace FactCloudAPI.DTOs.NotaCredito
{
    public class FormaPagoNotaCreditoDto
    {
        [Required]
        [MaxLength(50)]
        public string Metodo { get; set; } = "Efectivo";

        [Required]
        public decimal Valor { get; set; }
    }
}
