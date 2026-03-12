using System.ComponentModel.DataAnnotations;

namespace FactCloudAPI.DTOs.FotoPerfil
{
    public class SubirFotoPerfilDto
    {
        [Required, MaxLength(500)]
        public string Url { get; set; } = "";

        [MaxLength(100)]
        public string? Descripcion { get; set; }
    }
}
