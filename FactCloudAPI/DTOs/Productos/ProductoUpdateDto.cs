namespace FactCloudAPI.DTOs.Productos
{
    public class ProductoUpdateDto
    {
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }

        public decimal PrecioUnitario { get; set; }
        public decimal? Costo { get; set; }

        public int CantidadDisponible { get; set; }

        public string? Categoria { get; set; }
    }

}
