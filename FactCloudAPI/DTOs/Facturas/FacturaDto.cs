namespace FactCloudAPI.DTOs.Facturas
{
    public class FacturaDto
    {
        public int Id { get; set; }
        public string NumeroFactura { get; set; }
        public DateTime FechaEmision { get; set; }
        public string Cliente { get; set; }
        public decimal TotalFactura { get; set; }
        public string Estado { get; set; }
    }
}
