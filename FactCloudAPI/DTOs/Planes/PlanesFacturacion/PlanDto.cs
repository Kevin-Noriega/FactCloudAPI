namespace FactCloudAPI.DTOs.Planes.PlanesFacturacion
{
    public class PlanDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal MonthlyPrice { get; set; }
        public decimal AnnualPrice { get; set; }

        public bool HasDiscount { get; set; }
        public int? DiscountPercentage { get; set; }
        public decimal? OriginalAnnualPrice { get; set; }

        public bool Featured { get; set; }
        public string? Tag { get; set; }

        public List<PlanFeatureDto> Features { get; set; }
    }
}
