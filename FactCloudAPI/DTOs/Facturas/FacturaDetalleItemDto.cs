namespace FactCloudAPI.DTOs.Facturas
{
    public class FacturaDetalleItemDto
    {
        public int ProductoId { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get; set; }
    }

}
