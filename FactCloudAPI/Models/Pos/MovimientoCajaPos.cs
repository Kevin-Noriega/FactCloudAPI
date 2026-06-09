using System.ComponentModel.DataAnnotations;

namespace NubeeAPI.Models.Pos
{
    /// <summary>
    /// Ingreso o retiro de efectivo de la caja del POS no asociado a una venta.
    /// </summary>
    public class MovimientoCajaPos
    {
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        /// <summary>"Ingreso" | "Retiro".</summary>
        [MaxLength(20)]
        public string Tipo { get; set; } = "Ingreso";

        public decimal Monto { get; set; }

        [MaxLength(300)]
        public string? Descripcion { get; set; }

        [MaxLength(30)]
        public string NumeroComprobante { get; set; } = "";

        public DateTime Fecha { get; set; }
    }
}
