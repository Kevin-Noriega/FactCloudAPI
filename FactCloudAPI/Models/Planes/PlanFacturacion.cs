using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FactCloudAPI.Models.Suscripciones;

namespace FactCloudAPI.Models.Planes
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

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        [Required]
        public decimal PrecioAnual { get; set; }

        public bool Destacado { get; set; } = false;

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
