namespace FactCloudAPI.DTOs.Clientes
{
    public class ClienteUpdateDto
    {
        public string Nombre { get; set; }
        public string? Apellido { get; set; }
        public string TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string Correo { get; set; }
        public string? Telefono { get; set; }
        public string Departamento { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        public string? CodigoPostal { get; set; }
        public bool Activo { get; set; }
    }
}


