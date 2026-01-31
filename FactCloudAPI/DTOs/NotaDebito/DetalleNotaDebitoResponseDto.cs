namespace FactCloudAPI.DTOs.NotaDebito
{
    public class DetalleNotaDebitoResponseDto
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal Cantidad { get; set; }
        public string UnidadMedida { get; set; } = string.Empty;
        public decimal PrecioUnitario { get; set; }
        public decimal PorcentajeDescuento { get; set; }
        public decimal ValorDescuento { get; set; }
        public decimal SubtotalLinea { get; set; }
        public decimal TarifaIVA { get; set; }
        public decimal ValorIVA { get; set; }
        public decimal TarifaINC { get; set; }
        public decimal ValorINC { get; set; }
        public decimal TotalLinea { get; set; }
    }
}
