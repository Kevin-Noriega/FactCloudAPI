using FactCloudAPI.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactCloudAPI.Models.Impuestos
{
    /// <summary>
    /// Plan Único de Cuentas (PUC) — Decreto 2650 de 1993 (Colombia)
    /// Estructura jerárquica: Clase > Grupo > Cuenta > Subcuenta > Auxiliar
    /// </summary>
    public class CuentaContable
    {
        [Key]
        public int Id { get; set; }

        // ── Propietario (multi-tenant) ──────────────────────────
        
        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        // ── Código PUC ──────────────────────────────────────────
        /// <summary>
        /// Código numérico del PUC según Decreto 2650/93.
        /// Longitud según nivel:
        ///   1 dígito  → Clase     (1-9)
        ///   2 dígitos → Grupo     (11-99)
        ///   4 dígitos → Cuenta    (1105, 2408...)
        ///   6 dígitos → Subcuenta (110501, 240801...)
        ///   8+ dígitos→ Auxiliar  (11050101, 24080601...)
        /// ⚠️ Obligatorio mínimo hasta subcuenta (6 dígitos) para registros contables.
        /// </summary>
        [Required]
        [MaxLength(12)]
        public string Codigo { get; set; } = string.Empty;

        // ── Descripción ─────────────────────────────────────────
        [Required]
        [MaxLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Descripcion { get; set; }

        // ── Jerarquía ───────────────────────────────────────────
        /// <summary>
        /// Nivel en la jerarquía del PUC:
        /// 1 = Clase (1 dígito)      — Activos, Pasivos, Patrimonio...
        /// 2 = Grupo (2 dígitos)     — Disponible, Inversiones...
        /// 3 = Cuenta (4 dígitos)    — Caja, Bancos...
        /// 4 = Subcuenta (6 dígitos) — Caja general, Moneda extranjera...
        /// 5 = Auxiliar (8+ dígitos) — Detalle operativo
        /// </summary>
        [Required]
        [Range(1, 5)]
        public int Nivel { get; set; }

        /// <summary>Código del padre en la jerarquía. Null = nivel Clase.</summary>
        [MaxLength(12)]
        public string? CodigoPadre { get; set; }

        // ── Clasificación contable ──────────────────────────────
        /// <summary>
        /// Clase del PUC según el primer dígito:
        /// 1 = Activo | 2 = Pasivo | 3 = Patrimonio
        /// 4 = Ingresos | 5 = Gastos | 6 = Costos de ventas
        /// 7 = Costos de producción | 8 = Cuentas de orden deudoras
        /// 9 = Cuentas de orden acreedoras
        /// </summary>
        [Required]
        [Range(1, 9)]
        public int ClasePUC { get; set; }

        /// <summary>
        /// Naturaleza contable normal de la cuenta:
        /// "D" = Débito  → Activos, Gastos, Costos
        /// "C" = Crédito → Pasivos, Patrimonio, Ingresos
        /// </summary>
        [Required]
        [MaxLength(1)]
        public string Naturaleza { get; set; } = "D";

        // ── Tipo de ajuste ──────────────────────────────────────
        /// <summary>
        /// Tipo de ajuste por inflación (PCGA Colombia):
        /// "M" = Monetario | "NM" = No monetario | "N" = No aplica
        /// </summary>
        [MaxLength(2)]
        public string TipoAjuste { get; set; } = "N";

        // ── Configuración operativa ─────────────────────────────
        /// <summary>Permite movimiento directo (false = solo acumula hijos)</summary>
        public bool PermiteMovimiento { get; set; } = true;

        /// <summary>Requiere tercero (NIT/cédula) al registrar movimiento</summary>
        public bool RequiereTercero { get; set; } = false;

        /// <summary>Requiere centro de costo al registrar movimiento</summary>
        public bool RequiereCentroCosto { get; set; } = false;

        /// <summary>Requiere documento base (factura, remisión...) al registrar</summary>
        public bool RequiereDocumento { get; set; } = false;

        public bool Activa { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // ── Navegación ──────────────────────────────────────────
        public ICollection<Impuesto>? ImpuestosDebitoVentas { get; set; }
        public ICollection<Impuesto>? ImpuestosCreditoVentas { get; set; }
        public ICollection<Impuesto>? ImpuestosDebitoCompras { get; set; }
        public ICollection<Impuesto>? ImpuestosCreditoCompras { get; set; }

        // ── Propiedades calculadas (no mapeadas) ────────────────
        [NotMapped]
        public string CodigoNombre => $"{Codigo} - {Nombre}";

        [NotMapped]
        public string NombreClase => ClasePUC switch
        {
            1 => "Activo",
            2 => "Pasivo",
            3 => "Patrimonio",
            4 => "Ingresos",
            5 => "Gastos",
            6 => "Costos de ventas",
            7 => "Costos de producción",
            8 => "Cuentas de orden deudoras",
            9 => "Cuentas de orden acreedoras",
            _ => "Desconocido"
        };
    }
}
