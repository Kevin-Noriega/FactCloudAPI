using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactCloudAPI.Models
{
    [Table("RegistrosPendientes")]
    public class RegistroPendiente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string TransaccionId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string DatosRegistro { get; set; } // JSON serializado

        [Column(TypeName = "nvarchar(max)")]
        public string DatosNegocio { get; set; } // JSON serializado

        [Column(TypeName = "nvarchar(max)")]
        public string DatosPlan { get; set; } // JSON serializado

        [MaxLength(50)]
        public string Email { get; set; } // Para búsquedas rápidas

        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } = "PENDING"; // PENDING, APPROVED, DECLINED, COMPLETED

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime? FechaActualizacion { get; set; }

        [MaxLength(500)]
        public string NotasError { get; set; } // Para guardar errores si algo falla
    }
}
