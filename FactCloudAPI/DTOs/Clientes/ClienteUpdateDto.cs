namespace FactCloudAPI.DTOs.Clientes
{
    public class ClienteUpdateDto
    {
        public string Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? NombreComercial { get; set; }

        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
    }
}

