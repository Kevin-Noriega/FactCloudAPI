namespace FactCloudAPI.DTOs.Wompi
{
    public class PseTransaccionDto
    {
        public long PrecioEnCentavos { get; set; }
        public string Email { get; set; } = "";
        public string AcceptanceToken { get; set; } = "";
        public string TipoPersona { get; set; } = "Natural"; // "Natural" o "Juridica"
        public string TipoDocumento { get; set; } = "";
        public string NumeroDocumento { get; set; } = "";
        public string CodigoBanco { get; set; } = "";
        public string NombrePlan { get; set; } = "";
        public int PlanId { get; set; }
        public DatosRegistroDto DatosRegistro { get; set; } = new();
        public DatosNegocioDto DatosNegocio { get; set; } = new();
    }

}
