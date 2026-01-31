namespace FactCloudAPI.DTOs.NotaCredito
{
    public class FormaPagoNotaCreditoResponseDto
    {
        public int Id { get; set; }
        public string Metodo { get; set; } = string.Empty;
        public decimal Valor { get; set; }
    }
}
