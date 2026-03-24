namespace FactCloudAPI.DTOs.Cupones
{
    public class CuponValidateResponseDto
    {
        public bool IsValid { get; set; }
        public string Code { get; set; } = "";
        public decimal DiscountPercentage { get; set; }
        public string Message { get; set; } ="";
         public decimal PriceAfterDiscount { get; set; }

    }
}
