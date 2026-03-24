namespace FactCloudAPI.DTOs.Clientes
{
    public class ClienteDetalleDto
    {
        public int Id { get; set; }
        public string TipoPersona { get; set; }
        public string TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public int? DigitoVerificacion { get; set; }
        public string? CodigoSucursal { get; set; }

        public string Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? NombreComercial { get; set; }

        public string Departamento { get; set; }
        public string? DepartamentoCodigo { get; set; }
        public string Ciudad { get; set; }
        public string? CiudadCodigo { get; set; }
        public string Direccion { get; set; }
        public string? CodigoPostal { get; set; }

        public string? Correo { get; set; }
        public string? RegimenTributario { get; set; }
        public string? NombreContactoFacturacion { get; set; }
        public string? ApellidoContactoFacturacion { get; set; }
        public string? IndicativoFacturacion { get; set; }
        public string? TelefonoFacturacion { get; set; }

        public bool GranContribuyente { get; set; }
        public bool AutoretenedorRenta { get; set; }
        public bool RetenedorIVA { get; set; }
        public bool RegimenSimple { get; set; }
        public bool NoAplica { get; set; }
        public bool RetenedorICA { get; set; }
        public bool RetenedorRenta { get; set; }

        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }

        public List<TelefonoDto> Telefonos { get; set; } = new();
        public List<ContactoDto> Contactos { get; set; } = new();
    }
}