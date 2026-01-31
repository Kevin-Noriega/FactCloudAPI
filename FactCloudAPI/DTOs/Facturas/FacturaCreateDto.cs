namespace FactCloudAPI.DTOs.Facturas
{
    public class FacturaCreateDto
    {
        public int ClienteId { get; set; }
        public string FormaPago { get; set; }
        public string MedioPago { get; set; }
        public int? DiasCredito { get; set; }
        public string? Observaciones { get; set; }

        public List<FacturaCreateItemDto> Items { get; set; }
    }

}
