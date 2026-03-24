using System.ComponentModel.DataAnnotations;

namespace FactCloudAPI.DTOs.NotaDebito
{
    public class FormaPagoDto
    {
        [Required]
        [MaxLength(50)]
        public string Metodo { get; set; } = "Efectivo";

        [Required]
        public decimal Valor { get; set; }
    }
}
