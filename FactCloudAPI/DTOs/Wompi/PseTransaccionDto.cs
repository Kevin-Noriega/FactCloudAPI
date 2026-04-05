namespace FactCloudAPI.DTOs.Wompi
{
    public class PseTransaccionDto
    {
        public long PrecioEnCentavos { get; set; }
        public string Email { get; set; } = "";
        public string AcceptanceToken { get; set; } = "";
        public string Currency { get; set; } = "COP";
        public string TipoPersona { get; set; } = "Natural"; // "Natural" o "Juridica"
        public string TipoDocumento { get; set; } = "";
        public string Reference { get; set; } = "";
        public string NumeroDocumento { get; set; } = "";
        public string CodigoBanco { get; set; } = "";
        public string NombrePlan { get; set; } = "";
        public int PlanId { get; set; }

        public PSEPaymentMethodDto PaymentMethod { get; set; } = new();
        public PSECustomerDataDto CustomerData { get; set; } = new();

        public DatosRegistroDto DatosRegistro { get; set; } = new();
        public DatosNegocioDto DatosNegocio { get; set; } = new();
        public DatosPlanDto DatosPlan { get; set; } = new();
    }
    public class PSEPaymentMethodDto
    {
        public string Type { get; set; } = "PSE";
        public int User_type { get; set; }                    // 0 = Natural, 1 = Jurídica
        public string User_legal_id_type { get; set; } = "";  // CC, NIT, CE
        public string User_legal_id { get; set; } = "";       // Número documento
        public string Financial_institution_code { get; set; } = ""; // Código banco
        public string Payment_description { get; set; } = "";
    }
    public class PSECustomerDataDto
    {
        public string FullName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public PSELegalIdDto LegalId { get; set; } = new();
    }
    public class PSELegalIdDto
    {
        public string Type { get; set; } = "";
        public string Number { get; set; } = "";
    }

}
