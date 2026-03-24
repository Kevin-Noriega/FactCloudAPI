using System.ComponentModel.DataAnnotations;

namespace FactCloudAPI.Models.Planes
{
    public class UsuarioAddon
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int AddonId { get; set; }

        public DateTime FechaContratacion { get; set; } = DateTime.Now;

        public DateTime? FechaVencimiento { get; set; }

        public bool Activo { get; set; } = true;

        // Navegación
        public Usuario Usuario { get; set; } = null!;
        public Addon Addon { get; set; } = null!;
    }
}