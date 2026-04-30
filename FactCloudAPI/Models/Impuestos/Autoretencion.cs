using FactCloudAPI.Models.Impuestos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactCloudAPI.Models.Impuestos
{
    /// <summary>
    /// Autorretenciones — pestaña "Autoretenciones" de Siigo.
    /// Aplica a empresas designadas autorretenedoras por la DIAN
    /// (Grandes Contribuyentes o con resolución especial).
    /// La empresa se practica la retención a sí misma sobre sus propios ingresos.
    /// Decreto 1512 de 1985 y Resolución DIAN 15 de 2016 (CREE → Renta).
    /// </summary>
    public class Autoretencion
    {
        [Key]
        public int Id { get; set; }

        // ── Multi-tenant ────────────────────────────────────────
        
        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        // ── Identificación ──────────────────────────────────────
        /// <summary>Código interno. Ej: 26, 27, 28 (como en Siigo)</summary>
        [Required]
        public int Codigo { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        // ── Tipo de autorretención ──────────────────────────────
        /// <summary>
        /// Tipo de autorretención:
        /// "Autoretención 2201" → Autorretención especial en renta (Dec. 2201/2016)
        ///   Tarifas: 0.40% (comercio), 0.80% (servicios), 1.60% (otras actividades)
        /// "Autoretención ICA"  → Autorretención de ICA (según municipio)
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string TipoAutoretencion { get; set; } = "Autoretención 2201";

        // ── Tarifa ──────────────────────────────────────────────
        /// <summary>
        /// Tarifa porcentual. Valores típicos Dec. 2201/2016:
        /// 0.40% → Actividades comerciales y de manufactura
        /// 0.80% → Prestación de servicios
        /// 1.60% → Otras actividades / actividades especiales
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(7,4)")]
        public decimal Tarifa { get; set; }

        // ── Cuentas PUC ─────────────────────────────────────────
        /// <summary>
        /// Cuenta DÉBITO de la autorretención.
        /// PUC estándar: 13551519 — Autorretención especial en renta
        /// (Activo — Anticipo de impuestos y contribuciones)
        /// Se debita porque representa un activo (pago anticipado de impuesto propio).
        /// </summary>
        public int? CuentaDebitoId { get; set; }
        [ForeignKey("CuentaDebitoId")]
        public CuentaContable? CuentaDebito { get; set; }

        /// <summary>
        /// Cuenta CRÉDITO de la autorretención.
        /// PUC estándar: 23657501 — Autorretenciones por pagar
        /// (Pasivo — Retenciones y aportes de nómina)
        /// Se acredita porque es una obligación tributaria pendiente de declarar y pagar.
        /// </summary>
        public int? CuentaCreditoId { get; set; }
        [ForeignKey("CuentaCreditoId")]
        public CuentaContable? CuentaCredito { get; set; }

        // ── Base gravable ───────────────────────────────────────
        /// <summary>
        /// Base mínima para aplicar la autorretención (en UVT o pesos).
        /// Null = sin base mínima.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? BaseMinimaAplicacion { get; set; }

        /// <summary>
        /// Tipo de base:
        /// "UVT"   → Unidades de Valor Tributario
        /// "Pesos" → Valor absoluto en COP
        /// </summary>
        [MaxLength(10)]
        public string TipoBase { get; set; } = "Pesos";

        // ── Estado ──────────────────────────────────────────────
        public bool EnUso { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // ── Propiedades calculadas ──────────────────────────────
        [NotMapped]
        public string TarifaDisplay => $"{Tarifa}%";
    }
}
