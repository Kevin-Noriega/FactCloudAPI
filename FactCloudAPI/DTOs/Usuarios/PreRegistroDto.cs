using System.ComponentModel.DataAnnotations;

namespace FactCloudAPI.DTOs.Usuarios
{
    public class PreRegistroDto
    {
        [Required]
        public string Nombre { get; set; }

        public string? Apellido { get; set; }

        [Required, EmailAddress]
        public string Correo { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

        public string? TipoIdentificacion { get; set; }
        public string? NumeroIdentificacion { get; set; }
        public string? Telefono { get; set; }
    }

    // DTOs/ActivacionCuentaDto.cs
   
    // DTOs/LoginDto.cs
    

}
