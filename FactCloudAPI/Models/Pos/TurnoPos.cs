using System.ComponentModel.DataAnnotations;

namespace NubeeAPI.Models.Pos
{
    /// <summary>
    /// Turno (apertura/cierre de caja) del POS. Cada turno pertenece a un
    /// usuario (cajero) y guarda la base inicial, el arqueo de cierre y los
    /// totales calculados al cerrar.
    /// </summary>
    public class TurnoPos
    {
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        /// <summary>Número de turno secuencial por usuario.</summary>
        public int NumeroTurno { get; set; }

        /// <summary>Nombre del vendedor que abrió el turno (snapshot).</summary>
        [MaxLength(200)]
        public string VendedorNombre { get; set; } = "";

        public DateTime FechaApertura { get; set; }
        public DateTime? FechaCierre { get; set; }

        /// <summary>Base de efectivo declarada al abrir la caja.</summary>
        public decimal BaseInicial { get; set; }

        // ── Arqueo declarado en el cierre ──────────────────────────────────
        public decimal? TotalEfectivoReal { get; set; }
        public decimal? TotalTarjeta { get; set; }
        public decimal? TotalPagosLinea { get; set; }
        public decimal? TotalOtros { get; set; }

        // ── Cálculos guardados al cerrar ───────────────────────────────────
        public decimal? TotalEsperado { get; set; }
        public decimal? Diferencia { get; set; }

        [MaxLength(500)]
        public string? Observaciones { get; set; }

        [MaxLength(200)]
        public string? CerradoPorNombre { get; set; }

        /// <summary>"Abierto" | "Cerrado".</summary>
        [MaxLength(20)]
        public string Estado { get; set; } = "Abierto";
    }
}
