using System.ComponentModel.DataAnnotations;

namespace NubeeAPI.Models.Pos
{
    /// <summary>
    /// Venta registrada desde el POS. Enlaza con el turno abierto (si lo hay)
    /// y guarda el desglose por medio de pago para el arqueo de caja.
    /// </summary>
    public class PosVenta
    {
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        /// <summary>Turno al que pertenece la venta (si había uno abierto).</summary>
        public int? TurnoId { get; set; }
        public TurnoPos? Turno { get; set; }

        public int NumeroVenta { get; set; }
        public DateTime Fecha { get; set; }

        [MaxLength(200)]
        public string ClienteNombre { get; set; } = "Consumidor Final";

        public decimal Subtotal { get; set; }
        public decimal Impuestos { get; set; }
        public decimal Total { get; set; }

        // Desglose por medio de pago
        public decimal Efectivo { get; set; }
        public decimal Tarjeta { get; set; }
        public decimal PagosLinea { get; set; }
        public decimal Otros { get; set; }
        public decimal Credito { get; set; }

        [MaxLength(20)]
        public string Estado { get; set; } = "Registrada";

        public ICollection<PosVentaDetalle> Detalles { get; set; } = new List<PosVentaDetalle>();
    }

    public class PosVentaDetalle
    {
        public int Id { get; set; }

        public int PosVentaId { get; set; }
        public PosVenta PosVenta { get; set; } = null!;

        /// <summary>Producto del catálogo (null si fue un ítem manual).</summary>
        public int? ProductoId { get; set; }

        [MaxLength(500)]
        public string Nombre { get; set; } = "";

        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Descuento { get; set; }
        public decimal TotalLinea { get; set; }
    }
}
