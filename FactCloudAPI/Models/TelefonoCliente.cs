using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactCloudAPI.Models
{
    public class TelefonoCliente
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        [MaxLength(10)]
        public string? Indicativo { get; set; }

        [Required]
        [MaxLength(50)]
        public string Numero { get; set; }

        [MaxLength(20)]
        public string? Extension { get; set; }
    }
}