namespace FactCloudAPI.DTOs.Clientes
{
    public class ClienteDetalleDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? NombreComercial { get; set; }
        public string Correo { get; set; }
        public bool Activo { get; set; }
    }
}
