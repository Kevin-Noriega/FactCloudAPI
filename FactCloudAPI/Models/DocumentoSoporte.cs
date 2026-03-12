using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactCloudAPI.Models
{
    [Table("DocumentosSoporte")]
    public class DocumentoSoporte
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string NumeroDocumento { get; set; } // DS-001, DS-002, etc.

        [Required]
        [StringLength(100)]
        public string Prefijo { get; set; } = "DS";

        [Required]
        public int Consecutivo { get; set; }

        [Required]
        [StringLength(96)]
        public string CUDS { get; set; } // Código Único de Documento Soporte (SHA-384)

        [Required]
        public DateTime FechaGeneracion { get; set; } = DateTime.Now;

        // Información del Proveedor (No obligado a facturar)
        [Required]
        [StringLength(200)]
        public string ProveedorNombre { get; set; }

        [Required]
        [StringLength(20)]
        public string ProveedorNit { get; set; }

        [Required]
        [StringLength(5)]
        public string ProveedorTipoIdentificacion { get; set; } // CC, CE, TI, PP, NIT

        [StringLength(200)]
        public string ProveedorDireccion { get; set; }

        [StringLength(100)]
        public string ProveedorCiudad { get; set; }

        [StringLength(100)]
        public string ProveedorDepartamento { get; set; }

        [StringLength(100)]
        public string ProveedorEmail { get; set; }

        [StringLength(20)]
        public string ProveedorTelefono { get; set; }

        // Detalles del Documento
        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal IVA { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Descuento { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorTotal { get; set; }

        [StringLength(500)]
        public string Observaciones { get; set; }

        // Información DIAN
        [StringLength(50)]
        public string EstadoDian { get; set; } = "Pendiente"; // Pendiente, Aceptado, Rechazado

        [StringLength(500)]
        public string MensajeDian { get; set; }

        public DateTime? FechaRespuestaDian { get; set; }

        [StringLength(100)]
        public string NumeroRespuestaDian { get; set; }

        // Archivos generados
        public string RutaXML { get; set; }

        public string RutaPDF { get; set; }

        // Relación con Usuario
        [Required]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }

        // Auditoría
        public bool Estado { get; set; } = true;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public DateTime? FechaActualizacion { get; set; }
    }
}
