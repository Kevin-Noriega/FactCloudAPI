namespace FactCloudAPI.DTOs.Facturas
{
    public class FacturaPagoDto
    {
        public string Estado { get; set; }
        public string MedioPago { get; set; }
        public string FormaPago { get; set; }
        public decimal? MontoPagado { get; set; }
        public DateTime? FechaPago { get; set; }
        public string? Observaciones { get; set; }
    }

}
