using FactCloudAPI.Models.Suscripciones;
using System.ComponentModel.DataAnnotations;

namespace FactCloudAPI.Models.Cupones
{
    public class CuponUso
    {
        [Key]
        public int Id { get; set; }
        public int CuponId { get; set; }
        public Cupon Cupon { get; set; }

        public int SuscripcionId { get; set; }
        public SuscripcionFacturacion Suscripcion { get; set; }

        public DateTime UsadoAt { get; set; } = DateTime.UtcNow;
        public decimal DescuentoAplicado { get; set; }

    }
}
