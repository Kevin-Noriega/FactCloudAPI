using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactCloudAPI.Models
{
    public class NotaCredito
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string NumeroNota { get; set; } = string.Empty;

        // Relaciones
        [Required]
        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }

        [Required]
        public int FacturaId { get; set; }
        [ForeignKey("FacturaId")]
        public Factura? Factura { get; set; }

        // Datos de la factura (desnormalizado para consultas rápidas)
        [MaxLength(50)]
        public string? NumeroFactura { get; set; }

        public int? ClienteId { get; set; }
        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; }

        // Tipo de nota crédito
        [Required]
        [MaxLength(20)]
        public string Tipo { get; set; } = "devolucion"; // anulacion, devolucion, descuento

        // Motivo DIAN
        [Required]
        [MaxLength(50)]
        public string MotivoDIAN { get; set; } = string.Empty; // NC-1, NC-2, NC-3, NC-4, NC-5, NC-6

        // Fechas
        [Required]
        public DateTime FechaElaboracion { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // CUFE y XML
        [MaxLength(500)]
        public string? CUFE { get; set; }

        public string? XMLBase64 { get; set; }

        // Totales
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalBruto { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalDescuentos { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalIVA { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalINC { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ReteICA { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalNeto { get; set; }

        // Estado
        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } = "Pendiente"; // Pendiente, Enviada

        // Observaciones
        [MaxLength(1000)]
        public string? Observaciones { get; set; }

        // Colecciones
        public ICollection<DetalleNotaCredito> DetalleNotaCredito { get; set; } = new List<DetalleNotaCredito>();
        public ICollection<FormaPagoNotaCredito> FormasPago { get; set; } = new List<FormaPagoNotaCredito>();
    }

}
