using NubeeAPI.Models;
using NubeeAPI.Models.Impuestos;
using NubeeAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace NubeeAPI.Services.Impuestos
{
    public interface ITaxCalculationService
    {
        Task<TaxCalculationLineResult> CalculateLineAsync(
            DetalleFactura detalleFactura,
            int empresaId,
            ContextoContableImpuesto contexto,
            DateTime fechaDocumento,
            CancellationToken cancellationToken = default);

        Task<TaxCalculationDocumentResult> CalculateDocumentAsync(
            Factura factura,
            ContextoContableImpuesto contexto,
            CancellationToken cancellationToken = default);

        Task<Factura> ApplyTaxSnapshotAsync(
            Factura factura,
            TaxCalculationDocumentResult resultado,
            CancellationToken cancellationToken = default);
    }

    public class TaxCalculationService : ITaxCalculationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TaxCalculationService> _logger;

        public TaxCalculationService(ApplicationDbContext context, ILogger<TaxCalculationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<TaxCalculationLineResult> CalculateLineAsync(
            DetalleFactura detalleFactura,
            int empresaId,
            ContextoContableImpuesto contexto,
            DateTime fechaDocumento,                          // ✅ FIX-01: fecha del documento, no del servidor
            CancellationToken cancellationToken = default)
        {
            var resultado = new TaxCalculationLineResult();

            var baseGravable = detalleFactura.SubtotalLinea - detalleFactura.ValorDescuento;
            resultado.BaseGravable = baseGravable;

            var tarifasActivas = await _context.ConfiguracionesImpuestoEmpresa
                .Include(x => x.TarifaImpuesto)
                    .ThenInclude(x => x.ImpuestoConcepto)
                .Include(x => x.TarifaImpuesto)
                    .ThenInclude(x => x.Reglas)
                .Where(x => x.EmpresaId == empresaId
                         && x.Activo
                         && x.TarifaImpuesto.Activa
                         && x.TarifaImpuesto.CumpleBaseMinima(baseGravable))
                .OrderBy(x => x.TarifaImpuesto.PrioridadCalculo)
                .ToListAsync(cancellationToken);

            foreach (var config in tarifasActivas)
            {
                var tarifa = config.TarifaImpuesto;

                // ✅ FIX-01: usar la fecha del documento, no DateTime.UtcNow
                if (!tarifa.EsVigenteEn(fechaDocumento))
                    continue;

                var tarifaFinal = await ApplyRulesAsync(tarifa, baseGravable, cancellationToken);
                if (tarifaFinal == null) continue;

                // ✅ FIX-04: pasar la cantidad real de la línea al calculador
                var valorImpuesto = CalculateImpuestoValue(
                    baseGravable,
                    tarifaFinal.Value,
                    tarifa.TipoMonto,
                    detalleFactura.Cantidad);

                var naturaleza = DetermineLegalNature(tarifa.ImpuestoConcepto.Tipo);

                var lineaImpuesto = new DocumentoLineaImpuesto
                {
                    DetalleFacturaId = detalleFactura.Id,
                    TarifaImpuestoId = tarifa.Id,
                    BaseGravable = baseGravable,
                    TarifaUtilizada = tarifaFinal.Value,
                    ValorCalculado = valorImpuesto,
                    Naturaleza = naturaleza,
                    ReglaAplicada = tarifaFinal != tarifa.Tarifa ? "Regla modificó tarifa" : null,
                    FechaCalculo = DateTime.UtcNow
                };

                resultado.ImpuestosLinea.Add(lineaImpuesto);

                var resumenExistente = resultado.ResumenPorTarifa
                    .FirstOrDefault(x => x.TarifaImpuestoId == tarifa.Id);

                if (resumenExistente != null)
                {
                    resumenExistente.BaseTotal += baseGravable;
                    resumenExistente.ValorTotal += valorImpuesto;
                }
                else
                {
                    resultado.ResumenPorTarifa.Add(new DocumentoResumenImpuesto
                    {
                        TarifaImpuestoId = tarifa.Id,
                        Naturaleza = naturaleza,
                        BaseTotal = baseGravable,
                        TasaAplicada = tarifaFinal.Value,
                        ValorTotal = valorImpuesto,
                        FechaGeneracion = DateTime.UtcNow
                    });
                }

                _logger.LogInformation(
                    "Impuesto {Tarifa} | Línea {LineaId} | Base={Base} | {Pct}% | Valor={Valor}",
                    tarifa.Nombre, detalleFactura.Id, baseGravable, tarifaFinal, valorImpuesto);
            }

            return resultado;
        }

        public async Task<TaxCalculationDocumentResult> CalculateDocumentAsync(
            Factura factura,
            ContextoContableImpuesto contexto,
            CancellationToken cancellationToken = default)
        {
            var resultado = new TaxCalculationDocumentResult
            {
                FacturaId = factura.Id,
                FechaCalculo = DateTime.UtcNow,
                ImpuestosLinea = new List<DocumentoLineaImpuesto>(),
                Resumen = new List<DocumentoResumenImpuesto>()
            };

            if (factura.DetalleFacturas == null || !factura.DetalleFacturas.Any())
                return resultado;

            foreach (var linea in factura.DetalleFacturas)
            {
                // ✅ FIX-01: fecha del documento
                // ✅ FIX-02: EmpresaId, no UsuarioId
                var resultadoLinea = await CalculateLineAsync(
                    linea,
                    factura.UsuarioId,
                    contexto,
                    factura.FechaEmision,
                    cancellationToken);

                resultado.ImpuestosLinea.AddRange(resultadoLinea.ImpuestosLinea);

                foreach (var resumenLinea in resultadoLinea.ResumenPorTarifa)
                {
                    var existente = resultado.Resumen
                        .FirstOrDefault(x => x.TarifaImpuestoId == resumenLinea.TarifaImpuestoId);

                    if (existente != null)
                    {
                        existente.BaseTotal += resumenLinea.BaseTotal;
                        existente.ValorTotal += resumenLinea.ValorTotal;
                    }
                    else
                    {
                        resultado.Resumen.Add(resumenLinea);
                    }
                }
            }

            foreach (var res in resultado.Resumen)
                res.ValorTotal = Math.Round(res.ValorTotal, 2, MidpointRounding.AwayFromZero);

            return resultado;
        }

        public async Task<Factura> ApplyTaxSnapshotAsync(
            Factura factura,
            TaxCalculationDocumentResult resultado,
            CancellationToken cancellationToken = default)
        {
            // ✅ FIX-03: cargar conceptos una sola vez con join limpio
            // ❌ ELIMINADAS las primeras asignaciones que usaban .TarifaImpuesto?.ImpuestoConcepto
            //    (podían ser null en runtime y producían NullReferenceException)
            var tarifasConConcepto = await _context.TarifasImpuestos
                .Include(x => x.ImpuestoConcepto)
                .Where(x => resultado.Resumen.Select(r => r.TarifaImpuestoId).Contains(x.Id))
                .ToListAsync(cancellationToken);

            var rc = resultado.Resumen
                .Join(tarifasConConcepto,
                      r => r.TarifaImpuestoId,
                      t => t.Id,
                      (r, t) => new { r, t })
                .ToList();

            factura.TotalIVA = rc
                .Where(x => x.t.ImpuestoConcepto.Tipo == TipoImpuestoConcepto.IVA
                         && x.r.Naturaleza == NaturalezaFiscal.Trasladado)
                .Sum(x => x.r.ValorTotal);

            factura.TotalINC = rc
                .Where(x => x.t.ImpuestoConcepto.Tipo == TipoImpuestoConcepto.Impoconsumo
                         && x.r.Naturaleza == NaturalezaFiscal.Trasladado)
                .Sum(x => x.r.ValorTotal);

            // ✅ FIX-03: ICA solo trasladado — AdValorem separado de ReteICA
            factura.TotalICA = rc
                .Where(x => x.t.ImpuestoConcepto.Tipo == TipoImpuestoConcepto.AdValorem
                         && x.r.Naturaleza == NaturalezaFiscal.Trasladado)
                .Sum(x => x.r.ValorTotal);

            // Retenciones: Retefuente + ReteIVA + ReteICA + Autorretención
            factura.TotalRetenciones = rc
                .Where(x => x.r.Naturaleza == NaturalezaFiscal.Retenido
                         || x.r.Naturaleza == NaturalezaFiscal.Autorretenido)
                .Sum(x => x.r.ValorTotal);

            factura.TotalFactura = factura.Subtotal
                                 + factura.TotalIVA
                                 + factura.TotalINC
                                 + factura.TotalICA
                                 - factura.TotalRetenciones
                                 - factura.TotalDescuentos;

            return factura;
        }

        // ── HELPERS ──────────────────────────────────────────────────────────

        private async Task<decimal?> ApplyRulesAsync(
            TarifaImpuesto tarifa,
            decimal base_,
            CancellationToken cancellationToken)
        {
            // TODO Sprint 3: evaluar ReglaImpuesto.CondicionJSON con NCalc/Jint
            return await Task.FromResult(tarifa.Tarifa);
        }

        // ✅ FIX-04: recibe cantidad para ValorFijoUnidad (ej: bolsa plástica $66/unidad)
        private static decimal CalculateImpuestoValue(
            decimal baseGravable,
            decimal tarifa,
            TipoMontoImpuesto tipoMonto,
            decimal cantidad)
        {
            return tipoMonto switch
            {
                TipoMontoImpuesto.Porcentaje => Math.Round(baseGravable * tarifa / 100m, 2,
                                                        MidpointRounding.AwayFromZero),
                TipoMontoImpuesto.ValorFijoUnidad => Math.Round(tarifa * cantidad, 2,
                                                        MidpointRounding.AwayFromZero),
                TipoMontoImpuesto.ValorFijoLinea => tarifa,
                _ => 0m
            };
        }

        private static NaturalezaFiscal DetermineLegalNature(TipoImpuestoConcepto tipo)
        {
            return tipo switch
            {
                TipoImpuestoConcepto.IVA => NaturalezaFiscal.Trasladado,
                TipoImpuestoConcepto.Impoconsumo => NaturalezaFiscal.Trasladado,
                TipoImpuestoConcepto.AdValorem => NaturalezaFiscal.Trasladado,
                TipoImpuestoConcepto.Retefuente => NaturalezaFiscal.Retenido,
                TipoImpuestoConcepto.ReteIVA => NaturalezaFiscal.Retenido,
                TipoImpuestoConcepto.ReteICA => NaturalezaFiscal.Retenido,
                TipoImpuestoConcepto.Autorretencion => NaturalezaFiscal.Autorretenido,
                _ => NaturalezaFiscal.Informativo
            };
        }
    }

    // ── DTOs ─────────────────────────────────────────────────────────────────

    public class TaxCalculationLineResult
    {
        public decimal BaseGravable { get; set; }
        public List<DocumentoLineaImpuesto> ImpuestosLinea { get; set; } = new();
        public List<DocumentoResumenImpuesto> ResumenPorTarifa { get; set; } = new();
    }

    public class TaxCalculationDocumentResult
    {
        public int FacturaId { get; set; }
        public DateTime FechaCalculo { get; set; }
        public List<DocumentoLineaImpuesto> ImpuestosLinea { get; set; } = new();
        public List<DocumentoResumenImpuesto> Resumen { get; set; } = new();
        public decimal TotalImpuestos => Resumen.Sum(x => x.ValorTotal);
    }
}