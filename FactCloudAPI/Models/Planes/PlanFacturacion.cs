using System.ComponentModel.DataAnnotations;
using FactCloudAPI.Models.Suscripciones;

namespace FactCloudAPI.Models.Planes
{
    public class PlanFacturacion
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Codigo { get; set; } = null!; // PAY_PER_USE

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = null!;

        public decimal PrecioMensual { get; set; }
        public decimal PrecioAnual { get; set; }

        // null = ilimitado
        public int? LimiteDocumentosMensual { get; set; }
        public int? LimiteUsuarios { get; set; }
        public int DuracionMeses { get; set; } = 12;

        public bool Activo { get; set; } = true;
        public ICollection<SuscripcionFacturacion> Suscripciones { get; set; } = new List<SuscripcionFacturacion>();

    }
}
