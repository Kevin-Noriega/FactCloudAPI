using NubeeAPI.Models.Planes;
using System.ComponentModel.DataAnnotations;

namespace NubeeAPI.Models.Suscripciones
{
    public class SuscripcionFacturacion
    {
        public int Id { get; set; }

        // ?? Usuario
        [Required]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        // ?? Plan de facturación
        [Required]
        public int PlanFacturacionId { get; set; }
        public PlanFacturacion PlanFacturacion { get; set; } = null!;

        // ?? Ciclo de la suscripción
        [Required]
        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        // ?? Control de consumo
        public int DocumentosUsados { get; set; } = 0;

        // Estado
        public bool Activa { get; set; } = true;
        public string? TransaccionId { get; set; }
    }
}
