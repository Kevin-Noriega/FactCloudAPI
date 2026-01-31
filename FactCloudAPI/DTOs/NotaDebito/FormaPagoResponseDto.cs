namespace FactCloudAPI.DTOs.NotaDebito
{
    public class FormaPagoResponseDto
    {
        public int Id { get; set; }
        public string Metodo { get; set; } = string.Empty;
        public decimal Valor { get; set; }
    }
}
