namespace FactCloudAPI.Models.Planes
{
    public class PlanFeature
    {
        public int Id { get; set; }

        public int PlanFacturacionId { get; set; }
        public PlanFacturacion PlanFacturacion { get; set; } = null!;

        public string Texto { get; set; } = null!;
        public string? Tooltip { get; set; }
    }

}
