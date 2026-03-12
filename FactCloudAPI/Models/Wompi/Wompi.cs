namespace FactCloudAPI.Models.Wompi
{
    public class WompiTransactionRequest
    {
        public int AmountInCents { get; set; }
        public string Currency { get; set; } = "COP";
        public string Reference { get; set; }
        public string CustomerEmail { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string AcceptanceToken { get; set; }
        public CustomerData CustomerData { get; set; }
    }

    public class PaymentMethod
    {
        public string Type { get; set; } // CARD, NEQUI, PSE
        public string Token { get; set; } // Token de tarjeta
        public int Installments { get; set; } = 1;
    }

    public class CustomerData
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public LegalIdInfo LegalId { get; set; }
    }

    public class LegalIdInfo
    {
        public string Type { get; set; } // CC, CE, NIT
        public string Number { get; set; }
    }

    public class WompiTransactionResponse
    {
        public Data Data { get; set; }
    }

    public class Data
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string Reference { get; set; }
        public int AmountInCents { get; set; }
    }

    public class AcceptanceTokenResponse
    {
        public AcceptanceData Data { get; set; }
    }

    public class AcceptanceData
    {
        public PresignedAcceptance PresignedAcceptance { get; set; }
    }

    public class PresignedAcceptance
    {
        public string AcceptanceToken { get; set; }
        public string Permalink { get; set; }
    }
}
