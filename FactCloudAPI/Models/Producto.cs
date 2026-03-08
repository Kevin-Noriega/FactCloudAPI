using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactCloudAPI.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        // Producto o servicio
        public bool EsServicio { get; set; }

        // Información general
        [Required]
        [MaxLength(500)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Descripcion { get; set; }

        // Codificación estándar
        [MaxLength(100)]
        public string? CodigoInterno { get; set; }

        [MaxLength(100)]
        public string? CodigoUNSPSC { get; set; }  // ← Código arancelario DIAN

        // Unidad de medida
        [Required]
        [MaxLength(50)]
        public string UnidadMedida { get; set; } = "Unidad";

        // Marca, modelo, categoría (producto)
        [MaxLength(200)]
        public string? Marca { get; set; }

        [MaxLength(200)]
        public string? Modelo { get; set; }

        [MaxLength(200)]
        public string? Categoria { get; set; }

        // Código de barras (solo producto)
        [MaxLength(200)]
        public string? CodigoBarras { get; set; }

        // Valores monetarios
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Costo { get; set; }

        public bool IncluyeIVA { get; set; }

        // Impuestos simplificados tipo Siigo
        [MaxLength(50)]
        public string? ImpuestoCargo { get; set; }

        [MaxLength(50)]
        public string? Retencion { get; set; }

        // Inventario (solo producto)
        [Column(TypeName = "int")]
        public int? CantidadDisponible { get; set; }  // ← ✅ Nullable

        [Column(TypeName = "int")]
        public int CantidadMinima { get; set; } = 0;  // ← ✅ Stock mínimo

        // Tipo producto/servicio DIAN
        [MaxLength(50)]
        public string? TipoProducto { get; set; }

        // Estado / tracking
        public bool Activo { get; set; } = true;  // ← `estado` del front
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Relaciones
        public ICollection<DetalleFactura>? DetalleFacturas { get; set; }
    }


}