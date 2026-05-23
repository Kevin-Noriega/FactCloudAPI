using System.ComponentModel.DataAnnotations;

namespace NubeeAPI.Models
{
    public class AuditoriaAdmin
    {
        [Key]
        public int Id { get; set; }

        public int AdminId { get; set; }
        public Usuario? Admin { get; set; }

        [Required, MaxLength(50)]
        public string Accion { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Detalle { get; set; }

        public DateTime FechaHora { get; set; } = DateTime.UtcNow;
    }
}
