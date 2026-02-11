using FactCloudAPI.Models.Planes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactCloudAPI.Models.Cupones
{
    public class Cupon
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(20)]
        public string Codigo { get; set; }
        [Required]
        public decimal DescuentoPorcentaje { get; set; }
        [Required, MaxLength(20)]
        public int? MaxUsos { get; set; }
        public int UsosCodigo {get; set; } = 0;
        public DateTime? Expiracion { get; set; } // null: ilimitado

        public int? PlanId { get; set; }
        [ForeignKey("PlanId")]
        public PlanFacturacion? PlanFacturacion { get; set; }

        public bool IsActive { get; set; }
        public ICollection<CuponUso> usos { get; set; } = new List<CuponUso>();
           

    }
}
