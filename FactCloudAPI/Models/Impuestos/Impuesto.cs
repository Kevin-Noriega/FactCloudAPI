using NubeeAPI.Models.Impuestos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NubeeAPI.Models.Impuestos
{
    /// <summary>
    /// Impuestos configurables por empresa (como en Siigo: pestaña "Impuestos").
    /// Cubre IVA, INC (Impoconsumo), ICA, Retefuente y ReteICA.
    /// Cada impuesto queda vinculado a las cuentas PUC de ventas, compras y devoluciones.
    /// </summary>
    public class Impuesto
    {
        [Key]
        public int Id { get; set; }

        // -- Multi-tenant ----------------------------------------
      
        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        // -- Identificación --------------------------------------
        /// <summary>Código interno de la empresa. Ej: 1, 2, 3...</summary>
        [Required]
        public int Codigo { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de impuesto:
        /// "IVA"         ? Impuesto al Valor Agregado (código DIAN: 01)
        /// "INC"         ? Impuesto Nacional al Consumo (código DIAN: 04)
        /// "ICA"         ? Industria y Comercio (código DIAN: 03)
        /// "Retefuente"  ? Retención en la fuente
        /// "ReteICA"     ? Retención de ICA
        /// "ReteIVA"     ? Retención de IVA
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string TipoImpuesto { get; set; } = string.Empty;

        // -- Tarifa ----------------------------------------------
        /// <summary>
        /// Porcentaje de la tarifa. Ej: 19.00, 5.00, 2.50, 11.04
        /// Para "Por valor fijo" usar PorValor = true y Tarifa = 0.
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(7,4)")]
        public decimal Tarifa { get; set; }

        /// <summary>
        /// Si es true, el impuesto se calcula sobre un valor fijo (no porcentaje).
        /// Ejemplo: algunas tasas de timbre o contribuciones fijas.
        /// </summary>
        public bool PorValor { get; set; } = false;

        // -- Código DIAN (para XML UBL 2.1) ---------------------
        /// <summary>
        /// Código del tributo según tablas DIAN para factura electrónica:
        /// "01" = IVA | "02" = IC Porcentual | "03" = ICA
        /// "04" = INC | "05" = ReteRenta | "06" = ReteICA | "07" = ReteIVA
        /// </summary>
        [MaxLength(2)]
        public string? CodigoTributoDIAN { get; set; }

        // -- Cuentas PUC — Ventas --------------------------------
        /// <summary>
        /// Cuenta débito al vender con este impuesto.
        /// IVA: 13551501 (Anticipo de impuestos IVA)
        /// Retefuente: 13551501 (Anticipo retención en la fuente)
        /// </summary>
        public int? CuentaDebitoVentasId { get; set; }
        [ForeignKey("CuentaDebitoVentasId")]
        public CuentaContable? CuentaDebitoVentas { get; set; }

        /// <summary>
        /// Cuenta crédito al vender con este impuesto.
        /// IVA: 24080601 (IVA generado 19%)
        /// Retefuente: 23651501 (Honorarios - retención a cargo)
        /// </summary>
        public int? CuentaCreditoVentasId { get; set; }
        [ForeignKey("CuentaCreditoVentasId")]
        public CuentaContable? CuentaCreditoVentas { get; set; }

        // -- Cuentas PUC — Compras -------------------------------
        /// <summary>
        /// Cuenta débito al comprar con este impuesto.
        /// IVA: 24081001 (IVA descontable por compras 19%)
        /// Retefuente: 23651501 (Honorarios - retención por pagar)
        /// </summary>
        public int? CuentaDebitoComprasId { get; set; }
        [ForeignKey("CuentaDebitoComprasId")]
        public CuentaContable? CuentaDebitoCompras { get; set; }

        /// <summary>
        /// Cuenta crédito al comprar con este impuesto.
        /// Retefuente: 23651501 (Retención en la fuente por pagar)
        /// ReteICA: 23680501 (Reteica por pagar)
        /// </summary>
        public int? CuentaCreditoComprasId { get; set; }
        [ForeignKey("CuentaCreditoComprasId")]
        public CuentaContable? CuentaCreditoCompras { get; set; }

        // -- Cuentas PUC — Devoluciones --------------------------
        /// <summary>Cuenta para devoluciones en ventas (nota crédito)</summary>
        public int? CuentaDevolucionVentasId { get; set; }
        [ForeignKey("CuentaDevolucionVentasId")]
        public CuentaContable? CuentaDevolucionVentas { get; set; }

        /// <summary>Cuenta para devoluciones en compras (nota débito)</summary>
        public int? CuentaDevolucionComprasId { get; set; }
        [ForeignKey("CuentaDevolucionComprasId")]
        public CuentaContable? CuentaDevolucionCompras { get; set; }

        // -- Estado ----------------------------------------------
        public bool EnUso { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // -- Propiedades calculadas ------------------------------
        [NotMapped]
        public string TarifaDisplay => PorValor
            ? $"${Tarifa:N2} (valor fijo)"
            : $"{Tarifa}%";
    }
}
