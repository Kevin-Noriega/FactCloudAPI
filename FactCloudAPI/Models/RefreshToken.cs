using System.ComponentModel.DataAnnotations;

namespace FactCloudAPI.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Token { get; set; }  // El refresh token en sí (GUID o string aleatorio)

        [Required]
        public string JwtId { get; set; }  // ID del access token asociado (para validación)

        [Required]
        public int UsuarioId { get; set; }  // FK a Usuario

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaExpiracion { get; set; }

        public bool Usado { get; set; } = false;  // Para evitar reusar el mismo token
        public bool Revocado { get; set; } = false;  // Para invalidar manualmente (logout)

        // Navegación
        public Usuario Usuario { get; set; }
    }
}
