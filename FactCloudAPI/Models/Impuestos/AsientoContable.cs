using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NubeeAPI.Models.Impuestos
{
    public enum EstadoAsiento
    {
        Borrador = 1,
        Confirmado = 2,
        Anulado = 3
    }

    public class AsientoContable
    {
        [Key]
        public long Id { get; set; }

        // ── Referencia polimórfica al documento origen ──────────────
        /// <summary>
        /// Tipo del documento que generó este asiento.
        /// Ej: "Factura", "NotaCredito", "NotaDebito", "ReciboC", "CompEgreso"
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string TipoDocumentoOrigen { get; set; } = null!;

        /// <summary>
        /// Id del documento origen (FacturaId, NotaCreditoId, etc.)
        /// </summary>
        [Required]
        public long DocumentoOrigenId { get; set; }

        /// <summary>
        /// Número legible del comprobante contable. Ej: CB-2025-001
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string NumeroComprobante { get; set; } = null!;

        /// <summary>
        /// Período contable en formato YYYYMM. Ej: 202506 = Junio 2025.
        /// Permite contabilizar en el período correcto aunque se registre después.
        /// </summary>
        [Required]
        public int PeriodoContable { get; set; }

        // ── Línea del asiento ────────────────────────────────────────
        [Required]
        public int CuentaId { get; set; }
        public CuentaContable? Cuenta { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Debito { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Credito { get; set; } = 0;

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        // ── Fechas ───────────────────────────────────────────────────
        /// <summary>Fecha contable del documento (no la fecha del servidor).</summary>
        [Required]
        public DateTime FechaDocumento { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // ── Estado ───────────────────────────────────────────────────
        [Required]
        public EstadoAsiento Estado { get; set; } = EstadoAsiento.Borrador;

        // ── Auditoría ────────────────────────────────────────────────
        public int? UsuarioRegistroId { get; set; }
        public int? UsuarioConfirmacionId { get; set; }
        public DateTime? FechaConfirmacion { get; set; }

        // ── Validación de cuadre — se hace en el SERVICIO, no aquí ──
        // La validación SUM(Débitos) == SUM(Créditos) aplica sobre
        // el conjunto de líneas del comprobante, no sobre una línea.
        // Ver: AsientoContableService.ValidarCuadre()
    }
}