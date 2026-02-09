using System.ComponentModel.DataAnnotations;

namespace FactCloudAPI.DTOs.Login
{
    public class LoginDto
    {
        [Required, EmailAddress]
        public string Correo { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
