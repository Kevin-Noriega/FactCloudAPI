namespace FactCloudAPI.DTOs.Clientes
{
    public class ClienteCreateDto
    {
        public string Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? NombreComercial { get; set; }

        public string TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public int? DigitoVerificacion { get; set; }

        public string TipoPersona { get; set; }
        public string RegimenTributario { get; set; }

        public string Correo { get; set; }
        public string? Telefono { get; set; }

        public string Departamento { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }

        public string? CodigoPostal { get; set; }
    }

}
