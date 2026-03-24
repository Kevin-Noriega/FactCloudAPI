namespace FactCloudAPI.DTOs.Usuarios
{
    public class UpdateUsuarioDto
    {
        public string Nombre { get; set; }
        public string? Apellido { get; set; }
        public string Correo { get; set; }
        public string? Telefono { get; set; }

        // Datos del negocio
        public string? NombreNegocio { get; set; }
        public string? LogoNegocio { get; set; }
        public string? NitNegocio { get; set; }
        public int? DvNitNegocio { get; set; }
        public string? DireccionNegocio { get; set; }
        public string? CiudadNegocio { get; set; }
        public string? DepartamentoNegocio { get; set; }
        public string? CorreoNegocio { get; set; }
        public string? TelefonoNegocio { get; set; }
        public string? RegimenFiscal { get; set; }
        public string? RegimenTributario { get; set; }

        public bool Estado { get; set; } = true;
    }
}
