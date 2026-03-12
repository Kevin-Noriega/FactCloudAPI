using System.ComponentModel.DataAnnotations;

namespace FactCloudAPI.DTOs.DocumentoSoporte
{

    public class DocumentoSoporteCreateDto
    {
        [Required(ErrorMessage = "El nombre del proveedor es requerido")]
        [StringLength(200)]
        public string ProveedorNombre { get; set; }

        [Required(ErrorMessage = "El NIT del proveedor es requerido")]
        [StringLength(20)]
        public string ProveedorNit { get; set; }

        [Required(ErrorMessage = "El tipo de identificación es requerido")]
        [StringLength(5)]
        public string ProveedorTipoIdentificacion { get; set; }

        [StringLength(200)]
        public string ProveedorDireccion { get; set; }

        [StringLength(100)]
        public string ProveedorCiudad { get; set; }

        [StringLength(100)]
        public string ProveedorDepartamento { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string ProveedorEmail { get; set; }

        [StringLength(20)]
        public string ProveedorTelefono { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El valor total es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El valor debe ser mayor a 0")]
        public decimal ValorTotal { get; set; }

        [Range(0, double.MaxValue)]
        public decimal IVA { get; set; } = 0;

        [Range(0, double.MaxValue)]
        public decimal Descuento { get; set; } = 0;

        [StringLength(500)]
        public string Observaciones { get; set; }
    }

    public class DocumentoSoporteResponseDto
    {
        public int Id { get; set; }
        public string NumeroDocumento { get; set; }
        public string CUDS { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public string ProveedorNombre { get; set; }
        public string ProveedorNit { get; set; }
        public string ProveedorTipoIdentificacion { get; set; }
        public string Descripcion { get; set; }
        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Descuento { get; set; }
        public decimal ValorTotal { get; set; }
        public string Observaciones { get; set; }
        public string EstadoDian { get; set; }
        public string MensajeDian { get; set; }
        public DateTime? FechaRespuestaDian { get; set; }
    }
}
