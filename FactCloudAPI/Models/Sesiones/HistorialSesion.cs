using System.ComponentModel.DataAnnotations;

namespace FactCloudAPI.Models.Sesiones
{
    public class HistorialSesion
    {
        [Key]
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        [Required]
        public DateTime FechaHora { get; set; } = DateTime.UtcNow;

        [MaxLength(45)]
        public string IpAddress { get; set; } = "";

        [MaxLength(500)]
        public string UserAgent { get; set; } = "";

        [MaxLength(100)]
        public string Navegador { get; set; } = "";

        [MaxLength(100)]
        public string SistemaOperativo { get; set; } = "";

        [MaxLength(100)]
        public string Dispositivo { get; set; } = "";

        [MaxLength(100)]
        public string Ciudad { get; set; } = "";

        [MaxLength(100)]
        public string Pais { get; set; } = "";

        public bool Exitoso { get; set; } = true;

        public bool SesionActual { get; set; } = false;
    }
}
