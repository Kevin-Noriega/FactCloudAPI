namespace FactCloudAPI.DTOs.Clientes
{
    public class ClienteCreateDto
    {
        // ── Identificación ──────────────────────────────────
        public string TipoPersona { get; set; }
        public string TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public int? DigitoVerificacion { get; set; }
        public string? CodigoSucursal { get; set; }

        // ── Nombre ──────────────────────────────────────────
        public string Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? NombreComercial { get; set; }

        // ── Ubicación ───────────────────────────────────────
        public string Departamento { get; set; }
        public string? DepartamentoCodigo { get; set; }
        public string Ciudad { get; set; }
        public string? CiudadCodigo { get; set; }
        public string Direccion { get; set; }
        public string? CodigoPostal { get; set; }

        // ── Facturación ─────────────────────────────────────
        public string? Correo { get; set; }
        public string? RegimenTributario { get; set; }
        public string? NombreContactoFacturacion { get; set; }
        public string? ApellidoContactoFacturacion { get; set; }
        public string? IndicativoFacturacion { get; set; }
        public string? TelefonoFacturacion { get; set; }

        // ── Responsabilidades fiscales ───────────────────────
        public bool GranContribuyente { get; set; }   // O-13
        public bool AutoretenedorRenta { get; set; }   // O-15
        public bool RetenedorIVA { get; set; }   // O-23
        public bool RegimenSimple { get; set; }   // O-47
        public bool NoAplica { get; set; }   // R-99-PN
        public bool RetenedorICA { get; set; }
        public bool RetenedorRenta { get; set; }

        // ── Teléfonos y contactos ────────────────────────────
        public List<TelefonoDto> Telefonos { get; set; } = new();
        public List<ContactoDto> Contactos { get; set; } = new();

        // Array de códigos (opcional, referencia para facturación)
        public List<string>? Responsabilidades { get; set; }
    }

    public class TelefonoDto
    {
        public string? Indicativo { get; set; }
        public string Numero { get; set; } = null!;
        public string? Extension { get; set; }
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