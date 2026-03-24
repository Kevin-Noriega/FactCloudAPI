namespace FactCloudAPI.DTOs.Clientes
{
    public class ClienteCreateDto
    {
        public string Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? NombreComercial { get; set; }
        // ✅ Agrega esto
        public List<ContactoDto> Contactos { get; set; } = new();

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
        public bool EsProveedor { get; set; } = false;
        public bool RetenedorIVA { get; set; } = false;
        public bool RetenedorICA { get; set; } = false;
        public bool RetenedorRenta { get; set; } = false;
        public bool AutoretenedorRenta { get; set; } = false;
    }
    public class ContactoDto
    {
        public string Nombre { get; set; } = null!;
        public string? Apellido { get; set; }
        public string? Correo { get; set; }
        public string? Cargo { get; set; }
        public string? Indicativo { get; set; }
        public string? Telefono { get; set; }
    }

}
