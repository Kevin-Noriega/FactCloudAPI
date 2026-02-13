using System.ComponentModel.DataAnnotations;

namespace FactCloudAPI.Models
{
    public class FotoPerfil
    {
        [Key]
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }


        [Required, MaxLength(500)]
        public string Url { get; set; } = "";

        [MaxLength(500)]
        public string UrlExterna { get; set; } 

        public DateTime FechaSubida { get; set; } = DateTime.UtcNow;


        public bool EsPrincipal { get; set; } = true;
    }
}
