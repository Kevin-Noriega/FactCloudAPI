namespace FactCloudAPI.Models.NotasDebito
{
    public class NotaDebitoPago
    {
        public int Id { get; set; }

        public int NotaDebitoId { get; set; }
        public NotaDebito NotaDebito { get; set; }

        public string FormaPago { get; set; } = null!;
        public decimal Valor { get; set; }
    }

}
