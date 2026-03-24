using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FactCloudAPI.DTOs.Usuarios
{
    public class CreateUsuarioDto
    {
        // Datos básicos del usuario

        [Required, MaxLength(100)]
        public string Nombre { get; set; }

        [MaxLength(100)]
        public string? Apellido { get; set; }

        [MaxLength(20)]
        public string? Telefono { get; set; }

        [Required, EmailAddress]
        public string Correo { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

        // Identificación 

        [Required, MaxLength(20)]
        public string? TipoIdentificacion { get; set; }
        [Required]
        public string? NumeroIdentificacion { get; set; }
    }
}
