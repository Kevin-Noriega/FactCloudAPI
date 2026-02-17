using System.ComponentModel.DataAnnotations;

namespace FactCloudAPI.Models.Planes
{
    public class Addon
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = null!;

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        [Required]
        public decimal Precio { get; set; }

        [Required]
        [MaxLength(50)]
        public string Unidad { get; set; } = "mes"; // "mes", "año", "unidad"

        [Required]
        [MaxLength(50)]
        public string Tipo { get; set; } = "Capacidad"; // "Capacidad", "Usuarios", "Reportes"

        [MaxLength(20)]
        public string? Color { get; set; }

        public bool Activo { get; set; } = true;
    }

}
