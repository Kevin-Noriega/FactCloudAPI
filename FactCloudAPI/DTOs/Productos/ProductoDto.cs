namespace FactCloudAPI.DTOs.Productos
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string UnidadMedida { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int CantidadDisponible { get; set; }
        public bool Activo { get; set; }
    }

}
