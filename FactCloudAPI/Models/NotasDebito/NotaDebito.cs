using System.Diagnostics.Contracts;

namespace FactCloudAPI.Models.NotasDebito
{
    public class NotaDebito
    {
        public int Id { get; set; }

        // Relación con factura
        public int FacturaId { get; set; }
        public Factura Factura { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        // Datos generales
        public string Tipo { get; set; } = null!;
        public string Numero { get; set; } = null!;
        public DateTime FechaElaboracion { get; set; }

        // Cliente
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

       // public int? ContactoId { get; set; }
       // public Contacto? Contacto { get; set; }

        public int VendedorId { get; set; }
        public Usuario Vendedor { get; set; }

        // Totales
        public decimal TotalBruto { get; set; }
        public decimal TotalDescuentos { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TotalNeto { get; set; }

        // Observaciones
        public string? Observaciones { get; set; }
        public string? ArchivoAdjunto { get; set; }

        // Auditoría
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Navegación
        public ICollection<NotaDebitoDetalle> Detalles { get; set; } = new List<NotaDebitoDetalle>();
        public ICollection<NotaDebitoPago> Pagos { get; set; } = new List<NotaDebitoPago>();
    }

}
