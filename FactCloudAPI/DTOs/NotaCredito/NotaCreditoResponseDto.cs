using FactCloudAPI.DTOs.NotaDebito;

namespace FactCloudAPI.DTOs.NotaCredito
{
    public class NotaCreditoResponseDto
    {
        public int Id { get; set; }
        public string NumeroNota { get; set; } = string.Empty;
        public int FacturaId { get; set; }
        public string? NumeroFactura { get; set; }
        public ClienteSimpleDto? Cliente { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string MotivoDIAN { get; set; } = string.Empty;
        public DateTime FechaElaboracion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string? CUFE { get; set; }
        public string? XMLBase64 { get; set; }
        public decimal TotalBruto { get; set; }
        public decimal TotalDescuentos { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TotalIVA { get; set; }
        public decimal TotalINC { get; set; }
        public decimal ReteICA { get; set; }
        public decimal TotalNeto { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
        public List<DetalleNotaCreditoResponseDto> DetalleNotaCredito { get; set; } = new();
        public List<FormaPagoNotaCreditoResponseDto> FormasPago { get; set; } = new();
    }
}
