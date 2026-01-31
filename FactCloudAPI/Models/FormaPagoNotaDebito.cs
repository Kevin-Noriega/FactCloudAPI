using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactCloudAPI.Models
{
    public class FormaPagoNotaDebito
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int NotaDebitoId { get; set; }
        [ForeignKey("NotaDebitoId")]
        public NotaDebito? NotaDebito { get; set; }

        [Required]
        [MaxLength(50)]
        public string Metodo { get; set; } = "Efectivo";

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }
    }
}
