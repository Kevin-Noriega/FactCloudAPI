using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NubeeAPI.Models.Impuestos
{
    // ════════════════════════════════════════════════════════════════
    // ENUMS
    // ════════════════════════════════════════════════════════════════

    public enum TipoImpuestoConcepto
    {
        IVA = 1,
        Impoconsumo = 2,
        Retefuente = 3,
        ReteIVA = 4,
        ReteICA = 5,
        Autorretencion = 6,
        AdValorem = 7,
        BolsaPlastica = 8,
        Ultraprocesados = 9,
        BebidasAzucaradas = 10,
        Otro = 11
    }

    public enum NaturalezaFiscal
    {
        Trasladado = 1,
        Descontable = 2,
        Retenido = 3,
        Autorretenido = 4,
        Informativo = 5
    }

    public enum TipoMontoImpuesto
    {
        Porcentaje = 1,
        ValorFijoUnidad = 2,
        ValorFijoLinea = 3,
        UVT = 4
    }

    public enum ContextoContableImpuesto
    {
        Venta = 1,
        Compra = 2,
        NotaCreditoVenta = 3,
        NotaCreditoCompra = 4,
        NotaDebitoVenta = 5,
        NotaDebitoCompra = 6,
        DevolucionVenta = 7,
        DevolucionCompra = 8
    }

    public enum RolCuentaImpuesto
    {
        ImpuestoGenerado = 1,
        ImpuestoDescontable = 2,
        RetencionPorPagar = 3,
        RetencionAFavor = 4,
        AutorretencionPorPagar = 5,
        AutorretencionAFavor = 6,
        Anticipo = 7,
        Devolucion = 8,
        AjusteRedondeo = 9
    }

    // ════════════════════════════════════════════════════════════════
    // ENTIDADES
    // ════════════════════════════════════════════════════════════════

    public class ImpuestoConcepto
    {
        [Key]
        public long Id { get; set; }

        public int? EmpresaId { get; set; }

        [Required, MaxLength(50)]
        public string CodigoInterno { get; set; } = null!;

        [Required, MaxLength(200)]
        public string Nombre { get; set; } = null!;

        [Required]
        public TipoImpuestoConcepto Tipo { get; set; }

        [MaxLength(2)]
        public string? CodigoTributoDIAN { get; set; }

        public bool EsRetencion { get; set; } = false;
        public bool EsAutorretencion { get; set; } = false;
        public bool RequiereBaseMinima { get; set; } = false;
        public bool PermiteTarifaCero { get; set; } = true;
        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }

        public ICollection<TarifaImpuesto> Tarifas { get; set; } = new List<TarifaImpuesto>();
    }

    public class TarifaImpuesto
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long ImpuestoConceptoId { get; set; }
        public ImpuestoConcepto ImpuestoConcepto { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Nombre { get; set; } = null!;

        [Required]
        public TipoMontoImpuesto TipoMonto { get; set; } = TipoMontoImpuesto.Porcentaje;

        [Required, Column(TypeName = "decimal(18,4)")]
        public decimal Tarifa { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? BaseMinima { get; set; }

        [MaxLength(10)]
        public string? UnidadBaseMinima { get; set; }

        public bool PrecioIncluyeImpuesto { get; set; } = false;
        public bool PermiteAcumulacionConOtros { get; set; } = true;
        public short PrioridadCalculo { get; set; } = 100;

        [Required]
        public DateOnly VigenteDesde { get; set; }
        public DateOnly? VigenteHasta { get; set; }

        public bool Activa { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public ICollection<MapeoContableTarifa> MapeosContables { get; set; } = new List<MapeoContableTarifa>();
        public ICollection<ReglaImpuesto> Reglas { get; set; } = new List<ReglaImpuesto>();
        public ICollection<ConfiguracionImpuestoEmpresa> Configuraciones { get; set; } = new List<ConfiguracionImpuestoEmpresa>();

        public bool EsVigenteEn(DateTime fecha)
        {
            var d = DateOnly.FromDateTime(fecha);
            if (!Activa) return false;
            return VigenteHasta.HasValue
                ? d >= VigenteDesde && d <= VigenteHasta
                : d >= VigenteDesde;
        }

        public bool CumpleBaseMinima(decimal base_) =>
            !BaseMinima.HasValue || base_ >= BaseMinima.Value;
    }

    // ✅ FIX: índice a nivel de clase con [Index], no en método virtual
    [Index(nameof(EmpresaId), nameof(TarifaImpuestoId), IsUnique = true)]
    public class ConfiguracionImpuestoEmpresa
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public int EmpresaId { get; set; }

        [Required]
        public long TarifaImpuestoId { get; set; }
        public TarifaImpuesto TarifaImpuesto { get; set; } = null!;

        public bool Activo { get; set; } = true;
        public bool AplicacionAutomatica { get; set; } = true;
        public bool PermiteEdicionManual { get; set; } = false;
        public bool GeneraContabilidad { get; set; } = true;
        public bool ReportaDIAN { get; set; } = true;

        [MaxLength(500)]
        public string? Observaciones { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
    }

    // ✅ FIX: índice a nivel de clase — (TarifaImpuestoId, Contexto, RolCuenta) único
    [Index(nameof(TarifaImpuestoId), nameof(Contexto), nameof(RolCuenta), IsUnique = true)]
    public class MapeoContableTarifa
    {
        [Key]
        public long Id { get; set; }

        public int? EmpresaId { get; set; }

        [Required]
        public long TarifaImpuestoId { get; set; }
        public TarifaImpuesto TarifaImpuesto { get; set; } = null!;

        [Required]
        public ContextoContableImpuesto Contexto { get; set; }

        [Required]
        public RolCuentaImpuesto RolCuenta { get; set; }

        [Required]
        public int CuentaContableId { get; set; }
        public CuentaContable CuentaContable { get; set; } = null!;

        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }

    public class ReglaImpuesto
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long TarifaImpuestoId { get; set; }
        public TarifaImpuesto TarifaImpuesto { get; set; } = null!;

        [Required, MaxLength(200)]
        public string Descripcion { get; set; } = null!;

        [Required]
        public string CondicionJSON { get; set; } = null!;

        // ✅ FIX: string → enum (auditoria GRAVE-03)
        [Required]
        public AccionReglaImpuesto Accion { get; set; } = AccionReglaImpuesto.Aplicar;

        /// <summary>FK a TarifaImpuesto alternativa (en lugar de decimal suelto)</summary>
       
        public long? TarifaAlternativaId { get; set; }
        public TarifaImpuesto? TarifaAlternativa { get; set; }

        public bool Activa { get; set; } = true;
        public int Prioridad { get; set; } = 100;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }

    public enum AccionReglaImpuesto
    {
        Aplicar = 1,
        Excluir = 2,
        ModificarTarifa = 3,
        RedirigirCuenta = 4
    }

    public class DocumentoLineaImpuesto
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public int DetalleFacturaId { get; set; }
        public DetalleFactura DetalleFactura { get; set; } = null!;

        [Required]
        public long TarifaImpuestoId { get; set; }
        public TarifaImpuesto TarifaImpuesto { get; set; } = null!;

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal BaseGravable { get; set; }

        [Required, Column(TypeName = "decimal(18,4)")]
        public decimal TarifaUtilizada { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal ValorCalculado { get; set; }

        [Required]
        public NaturalezaFiscal Naturaleza { get; set; }

        // ── Snapshot inmutable al confirmar (auditoria ARQU-01) ──────
        /// <summary>Nombre de la tarifa en el momento de cálculo. Nunca se actualiza.</summary>
        [MaxLength(100)]
        public string? SnapshotNombreTarifa { get; set; }

        /// <summary>Código DIAN en el momento de cálculo. Nunca se actualiza.</summary>
        [MaxLength(2)]
        public string? SnapshotCodigoDIAN { get; set; }

        /// <summary>Tarifa exacta en el momento de cálculo. Nunca se actualiza.</summary>
        [Column(TypeName = "decimal(18,4)")]
        public decimal? SnapshotTarifa { get; set; }

        [MaxLength(200)]
        public string? ReglaAplicada { get; set; }

        public DateTime FechaCalculo { get; set; } = DateTime.UtcNow;
    }

    // ✅ FIX: índice a nivel de clase — (FacturaId, TarifaImpuestoId) único
    [Index(nameof(FacturaId), nameof(TarifaImpuestoId), IsUnique = true)]
    public class DocumentoResumenImpuesto
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public int FacturaId { get; set; }

        [Required]
        public long TarifaImpuestoId { get; set; }
        public TarifaImpuesto TarifaImpuesto { get; set; } = null!;

        [Required]
        public NaturalezaFiscal Naturaleza { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal BaseTotal { get; set; }

        [Required, Column(TypeName = "decimal(18,4)")]
        public decimal TasaAplicada { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal ValorTotal { get; set; }

        public DateTime FechaGeneracion { get; set; } = DateTime.UtcNow;
    }
}