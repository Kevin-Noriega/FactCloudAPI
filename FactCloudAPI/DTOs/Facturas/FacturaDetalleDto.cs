namespace FactCloudAPI.DTOs.Facturas
{
    public class FacturaDetalleDTO
    {
        public int Id { get; set; }
        public string NumeroFactura { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime? FechaVencimiento { get; set; }

        public string Cliente { get; set; }
        public string ClienteCorreo { get; set; }

        public decimal Subtotal { get; set; }
        public decimal TotalIVA { get; set; }
        public decimal? TotalINC { get; set; }
        public decimal TotalFactura { get; set; }

        public string Estado { get; set; }
        public string MedioPago { get; set; }
        public string FormaPago { get; set; }

        public bool EnviadaDIAN { get; set; }
        public bool EnviadaCliente { get; set; }

        public List<FacturaDetalleItemDto> Items { get; set; }
    }

}
