namespace FactCloudAPI.Models.NotasDebito
{
    public class NotaDebitoDetalle
    {
        public int Id { get; set; }

        public int NotaDebitoId { get; set; }
        public NotaDebito NotaDebito { get; set; }

        public int ProductoId { get; set; }
        public Producto Producto { get; set; }

        public string Descripcion { get; set; } = null!;
        public decimal Cantidad { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal PorcentajeDescuento { get; set; }

        // Impuestos
        public decimal ImpuestoCargo { get; set; }
        public decimal ImpuestoRetencion { get; set; }

        public decimal ValorTotal { get; set; }
    }

}
