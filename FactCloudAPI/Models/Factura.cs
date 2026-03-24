using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactCloudAPI.Models
{
    public class Factura
    {
        // ==================== IDENTIFICADORES ====================

        [Key]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        [Required]
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        // ==================== RESOLUCIÓN DIAN ====================

        /// <summary>Número resolución DIAN (14 dígitos exactos) → sts:InvoiceAuthorization</summary>
        
        [StringLength(14, MinimumLength = 14, ErrorMessage = "La autorización DIAN debe tener exactamente 14 dígitos")]
        public string? NumeroAutorizacion { get; set; } 

        /// <summary>Fecha inicio vigencia resolución → sts:StartDate</summary>
      
        public DateTime? FechaInicioAutorizacion { get; set; }

        /// <summary>Fecha fin vigencia resolución → sts:EndDate</summary>
      
        public DateTime? FechaFinAutorizacion { get; set; }

        /// <summary>Número inicial del rango autorizado → sts:From</summary>
        [Required]
        public long RangoNumeracionDesde { get; set; }

        /// <summary>Número final del rango autorizado → sts:To</summary>
        [Required]
        public long RangoNumeracionHasta { get; set; }

        /// <summary>
        /// Clave técnica asignada por la DIAN para calcular el CUFE.
        /// NUNCA va en el XML. Guardar cifrada en producción.
        /// </summary>
        [MaxLength(200)]
        public string? ClaveTecnica { get; set; }

        // ==================== TIPO Y AMBIENTE ====================

        /// <summary>
        /// cbc:ProfileExecutionID
        /// 1 = Producción | 2 = Habilitación/Pruebas
        /// ⚠️ Iniciar siempre en 2 hasta tener habilitación DIAN aprobada
        /// </summary>
        [Required]
        [Range(1, 2)]
        public int TipoAmbiente { get; set; } = 2;

        /// <summary>
        /// cbc:InvoiceTypeCode
        /// "01" = FEV estándar | "02" = Exportación | "03" = Contingencia | "04" = Talonario
        /// </summary>
        [Required]
        [MaxLength(2)]
        public string TipoFactura { get; set; } = "01";

        /// <summary>
        /// cbc:CustomizationID
        /// "10" = Operación estándar | "09" = Mandatos
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string TipoOperacion { get; set; } = "10";

        // ==================== NUMERACIÓN Y FECHAS ====================

        /// <summary>Número de la factura sin prefijo → cbc:ID</summary>
        [Required]
        [MaxLength(20)]
        public string NumeroFactura { get; set; } = string.Empty;

        /// <summary>Prefijo del rango autorizado → sts:Prefix (máx 4 chars según DIAN)</summary>
        [MaxLength(4)]
        public string? Prefijo { get; set; }

        /// <summary>Fecha de emisión → cbc:IssueDate (YYYY-MM-DD)</summary>
        [Required]
        public DateTime FechaEmision { get; set; } = DateTime.Now;

        /// <summary>
        /// Fecha de vencimiento → cbc:DueDate
        /// Obligatoria para RADIAN (título valor electrónico)
        /// </summary>
        public DateTime? FechaVencimiento { get; set; }

        /// <summary>
        /// Hora de emisión → cbc:IssueTime
        /// ⚠️ Formato DIAN obligatorio: HH:mm:ss-05:00 (UTC-0500 Colombia)
        /// Ejemplo correcto: 14:30:00-05:00
        /// </summary>
        [MaxLength(14)]
        public string? HoraEmision { get; set; }

        // ==================== CUFE Y QR ====================

        /// <summary>
        /// Código Único Factura Electrónica → cbc:UUID
        /// SHA-384 en hexadecimal = siempre 96 caracteres exactos
        /// Fórmula: SHA384(NumFac+FecFac+HorFac+ValFac+"01"+ValIVA+"04"+ValINC+"03"+ValICA+ValTot+NitOFE+NumAdq+ClTec+TipoAmb)
        /// </summary>
        [MaxLength(96)]
        public string? Cufe { get; set; }

        /// <summary>
        /// URL QR para verificación DIAN
        /// Formato: https://catalogo-vpfe.dian.gov.co/document/searchqr?documentkey={CUFE}
        /// </summary>
        [MaxLength(200)]
        public string? QRCode { get; set; }

        // ==================== VALORES MONETARIOS ====================

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        /// <summary>Total IVA (código tributo 01) — requerido en fórmula CUFE</summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalIVA { get; set; }

        /// <summary>Total INC - Impuesto Nacional al Consumo (código 04) — requerido en fórmula CUFE</summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalINC { get; set; } = 0;

        /// <summary>Total ICA - Industria y Comercio (código 03) — requerido en fórmula CUFE</summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalICA { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalDescuentos { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalRetenciones { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalFactura { get; set; }

        // ==================== PAGO ====================

        /// <summary>
        /// Forma de pago → cbc:PaymentMeansCode
        /// "1" = Contado (pago inmediato) | "2" = Crédito
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string FormaPago { get; set; } = "1";

        /// <summary>
        /// Medio de pago — códigos DIAN:
        /// "10" = Efectivo | "42" = Transferencia bancaria |
        /// "47" = Débito bancario | "48" = Tarjeta crédito | "ZZZ" = Otro
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string MedioPago { get; set; } = "10";

        public int? DiasCredito { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MontoPagado { get; set; }

        public DateTime? FechaPago { get; set; }

        // ==================== ESTADO Y OBSERVACIONES ====================

        /// <summary>Estados: "Emitida" | "Enviada" | "Validada" | "Pagada" | "Anulada" | "Vencida"</summary>
        [Required]
        [MaxLength(50)]
        public string Estado { get; set; } = "Emitida";

        [MaxLength(2000)]
        public string? Observaciones { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // ==================== ENVÍO DIAN ====================

        public bool EnviadaDIAN { get; set; } = false;
        public DateTime? FechaLimiteEnvioDIAN { get; set; }
        public DateTime? FechaEnvioDIAN { get; set; }

        [MaxLength(1000)]
        public string? RespuestaDIAN { get; set; }

        // ==================== ENVÍO CLIENTE ====================

        public bool EnviadaCliente { get; set; } = false;
        public DateTime? FechaEnvioCliente { get; set; }

        // ==================== XML FIRMADO ====================

        /// <summary>XML UBL 2.1 firmado (XAdES-EPES) en Base64 para transmisión a DIAN</summary>
        public string? XmlBase64 { get; set; }

        // ==================== RELACIONES ====================

        public ICollection<DetalleFactura>? DetalleFacturas { get; set; }
        public ICollection<NotaDebito> NotasDebito { get; set; } = new List<NotaDebito>();

        // ==================== MÉTODOS ====================

        public void CalcularFechas()
        {
            // ⚠️ Formato DIAN obligatorio: HH:mm:ss-05:00 (Colombia UTC-0500)
            if (string.IsNullOrEmpty(HoraEmision))
                HoraEmision = FechaEmision.ToString("HH:mm:ss") + "-05:00";

            // Plazo máximo de transmisión a DIAN = 48 horas desde emisión
            FechaLimiteEnvioDIAN = FechaEmision.AddHours(48);

            FechaVencimiento = (FormaPago == "2" && DiasCredito.HasValue && DiasCredito > 0)
                ? FechaEmision.AddDays(DiasCredito.Value)
                : FechaEmision;
        }

        /// <summary>
        /// Construye la URL del QR de verificación DIAN.
        /// Llamar siempre después de calcular el CUFE.
        /// </summary>
        public void GenerarQRCode()
        {
            if (!string.IsNullOrEmpty(Cufe))
                QRCode = $"https://catalogo-vpfe.dian.gov.co/document/searchqr?documentkey={Cufe}";
        }

        // ==================== PROPIEDADES CALCULADAS (NO MAPEADAS) ====================

        /// <summary>Número completo de factura con prefijo → cbc:ID en el XML</summary>
        [NotMapped]
        public string NumeroFacturaCompleto =>
            string.IsNullOrEmpty(Prefijo) ? NumeroFactura : $"{Prefijo}{NumeroFactura}";

        [NotMapped]
        public bool DentroPlazoEnvioDIAN =>
            EnviadaDIAN || (FechaLimiteEnvioDIAN.HasValue && DateTime.Now <= FechaLimiteEnvioDIAN);

        [NotMapped]
        public bool EstaVencida =>
            FechaVencimiento.HasValue && DateTime.Now > FechaVencimiento && Estado != "Pagada";

        [NotMapped]
        public int? HorasRestantesEnvioDIAN
        {
            get
            {
                if (EnviadaDIAN || !FechaLimiteEnvioDIAN.HasValue) return null;
                var horas = (FechaLimiteEnvioDIAN.Value - DateTime.Now).TotalHours;
                return horas > 0 ? (int)Math.Ceiling(horas) : 0;
            }
        }
    }
}
