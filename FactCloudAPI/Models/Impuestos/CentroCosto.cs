using System.ComponentModel.DataAnnotations;

namespace NubeeAPI.Models.Impuestos
{
    public class CentroCosto
    {
        [Key] public int Id { get; set; }
        public int? UsuarioId { get; set; }

        [Required, MaxLength(20)] public string Codigo { get; set; }
        [Required, MaxLength(200)] public string Nombre { get; set; }

        public int? PadreId { get; set; }
        public CentroCosto? Padre { get; set; }

        public int Nivel { get; set; } = 1;
        public bool PermiteMovimiento { get; set; } = true;
        public bool Activo { get; set; } = true;

        public ICollection<CentroCosto> Hijos { get; set; } = new System.Collections.Generic.List<CentroCosto>();
    }
}