using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NubeeAPI.Models.Suscripciones;

namespace NubeeAPI.Models.Planes
{
    public class PlanFacturacion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Codigo { get; set; } = null!; // BASIC, PRO, PAY_PER_USE

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = null!;

        /// <summary>
        /// Categoría del plan: "FACTURACION" (suscripción base) | "POS" (módulo POS
        /// que se vende por separado y se contrata sobre cualquier plan de facturación).
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Tipo { get; set; } = "FACTURACION";

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        [Required]
        public decimal PrecioAnual { get; set; }

        public bool Destacado { get; set; } = false;

        // ── Módulos/capacidades habilitados por el plan (esquema tipo SaaS) ──
        // Cada plan de facturación define qué funcionalidades incluye. El POS NO
        // se incluye en los planes de facturación: es un producto aparte (Tipo="POS")
        // que la empresa contrata adicionalmente. IncluyePOS marca los planes POS.
        public bool IncluyePOS { get; set; } = false;
        public bool IncluyeInventario { get; set; } = false;
        public bool IncluyeNomina { get; set; } = false;
        public bool IncluyeContabilidad { get; set; } = false;
        public bool IncluyeSucursales { get; set; } = false;

        //Descuento en porcentaje
        public int? DescuentoPorcentaje { get; set; }
        public bool DescuentoActivo { get; set; } = false;

         //(null = ilimitado)
        public int? LimiteDocumentosAnuales { get; set; }
        public int? LimiteUsuarios { get; set; }

        public int DuracionMeses { get; set; } = 12;

        public bool Activo { get; set; } = true;
        public ICollection<PlanFeature> Features { get; set; } = new List<PlanFeature>();

        public ICollection<SuscripcionFacturacion> Suscripciones { get; set; }
            = new List<SuscripcionFacturacion>();

        [NotMapped]
        public decimal PrecioAnualFinal
        {
            get
            {
                if (DescuentoActivo && DescuentoPorcentaje.HasValue)
                {
                    var descuento = PrecioAnual * DescuentoPorcentaje.Value / 100m;
                    return decimal.Round(PrecioAnual - descuento, 2);
                }

                return PrecioAnual;
            }
        }

        [NotMapped]
        public decimal PrecioMensualFinal
            => decimal.Round(PrecioAnualFinal / 12, 2);
    }
}
