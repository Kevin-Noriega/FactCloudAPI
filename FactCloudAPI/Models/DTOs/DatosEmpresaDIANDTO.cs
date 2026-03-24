namespace FactCloudAPI.Models.DTOs
{
    public class DatosEmpresaDIANDTO
    {
        public string? NitNegocio { get; set; }
        public int? DvNitNegocio { get; set; }
        public string NitCompleto => $"{NitNegocio}-{DvNitNegocio}";

        public string? NombreNegocio { get; set; }
        public string? DireccionNegocio { get; set; }
        public string? CiudadNegocio { get; set; }
        public string? DepartamentoNegocio { get; set; }
        public string? CorreoNegocio { get; set; }
        public string? TelefonoNegocio { get; set; }
        public string? SoftwarePIN { get; set; }
        public string? PrefijoAutorizadoDIAN { get; set; }
        public string? NumeroResolucionDIAN { get; set; }
        public DateTime? FechaVigenciaFinal { get; set; }
    }
}
