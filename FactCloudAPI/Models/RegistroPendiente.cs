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
        public string WompiReference { get; set; } = "";
        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } = "PENDING";
        // ── Datos del usuario ──
        [MaxLength(200)]
        public string Nombre { get; set; } = "";
        [MaxLength(200)]
        public string Correo { get; set; } = "";
        [MaxLength(20)]
        public string Telefono { get; set; } = "";
        [MaxLength(500)]
        public string PasswordHash { get; set; } = "";
        [MaxLength(50)]
        public string TipoIdentificacion { get; set; } = "";
        [MaxLength(50)]
        public string NumeroIdentificacion { get; set; } = "";
        // ── Datos del negocio ──
        [MaxLength(200)]
        public string NombreNegocio { get; set; } = "";
        [MaxLength(50)]
        public string Nit { get; set; } = "";
        [MaxLength(5)]
        public string? DvNit { get; set; }
        [MaxLength(300)]
        public string Direccion { get; set; } = "";
        [MaxLength(100)]
        public string Ciudad { get; set; } = "";
        [MaxLength(100)]
        public string Departamento { get; set; } = "";
        [MaxLength(20)]
        public string? TelefonoNegocio { get; set; }
        [MaxLength(200)]
        public string? CorreoNegocio { get; set; }
        // ── Datos del plan ──
        public int PlanFacturacionId { get; set; }
        [MaxLength(20)]
        public DateTime? FechaAprobacion { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string DatosRegistro { get; set; } // JSON serializado

        [Column(TypeName = "nvarchar(max)")]
        public string DatosNegocio { get; set; } // JSON serializado

        [Column(TypeName = "nvarchar(max)")]
        public string DatosPlan { get; set; } // JSON serializado

        [MaxLength(50)]
        public string Email { get; set; } // Para búsquedas rápidas

        

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime? FechaActualizacion { get; set; }

        [MaxLength(500)]
        public string NotasError { get; set; } // Para guardar errores si algo falla
    }
}
