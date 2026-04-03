using System.ComponentModel.DataAnnotations;

namespace FactCloudAPI.Models.Usuarios
{
    public class RepresentanteLegal
    {
        [Key]
        public int Id { get; set; }

        public int NegocioId { get; set; }
        public Negocio Negocio { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; }

        [Required, MaxLength(100)]
        public string Apellidos { get; set; }

        [Required]
        public TipoDocumento TipoDocumento { get; set; }

        [Required, MaxLength(20)]
        public string NumeroIdentificacion { get; set; }

        [MaxLength(100)]
        public string? CiudadExpedicion { get; set; }

        [MaxLength(100)]
        public string? CiudadResidencia { get; set; }
    }
}
