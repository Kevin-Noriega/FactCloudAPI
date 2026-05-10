using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FactCloudAPI.Models.Impuestos
{
    public class AsientoContable
    {
        [Key] public int Id { get; set; }

        public int? FacturaId { get; set; }
        public Factura? Factura { get; set; }

        [Required] public int CuentaId { get; set; }
        public CuentaContable? Cuenta { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Debito { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Credito { get; set; } = 0;

        public string? Descripcion { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        // Validación de cuadre
        [NotMapped]
        public bool EsValido => Debito >= 0 && Credito >= 0
                             && !(Debito > 0 && Credito > 0); // no ambos a la vez
    }
}
