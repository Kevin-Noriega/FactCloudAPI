using NubeeAPI.Models.Planes;

namespace NubeeAPI.DTOs.Planes.PlanesFacturacion
{
    public class PlanFeatureDto
    {
        public int Id { get; set; }

        public int PlanFacturacionId { get; set; }
        public PlanFacturacion PlanFacturacion { get; set; } = null!;

        public string Text { get; set; } = null!;
        public string? Tooltip { get; set; }
    }
}
