using System.ComponentModel.DataAnnotations;

namespace NubeeAPI.Models.Pos
{
    /// <summary>
    /// Etiqueta para clasificar/identificar ventas en el POS.
    /// </summary>
    public class EtiquetaPos
    {
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Nombre { get; set; } = "";

        public bool Activa { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
