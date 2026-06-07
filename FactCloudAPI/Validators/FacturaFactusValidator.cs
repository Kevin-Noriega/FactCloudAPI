using FluentValidation;
using NubeeAPI.Models;

namespace NubeeAPI.Validators
{
    /// <summary>
    /// Valida que una Factura esté lista para enviarse a Factus/DIAN.
    /// Se usa de forma EXPLÍCITA en el endpoint de envío (no auto-cableado al
    /// model binding) para no alterar el flujo de creación de facturas.
    /// </summary>
    public class FacturaFactusValidator : AbstractValidator<Factura>
    {
        // Medios de pago DIAN aceptados (payment_method_code)
        private static readonly HashSet<string> MediosPagoValidos =
            new() { "10", "20", "42", "47", "48", "49", "71", "72", "ZZZ" };

        public FacturaFactusValidator()
        {
            RuleFor(f => f.FactusRangoId)
                .GreaterThan(0)
                .WithMessage("La factura no tiene numbering_range_id (FactusRangoId) asignado.");

            RuleFor(f => f.TotalFactura)
                .GreaterThan(0).WithMessage("El total de la factura debe ser mayor que cero.");

            RuleFor(f => f.FormaPago)
                .Must(fp => fp == "1" || fp == "2")
                .WithMessage("FormaPago debe ser '1' (contado) o '2' (crédito).");

            // ── Cliente ──────────────────────────────────────────────────
            RuleFor(f => f.Cliente)
                .NotNull().WithMessage("La factura no tiene cliente.");

            When(f => f.Cliente != null, () =>
            {
                RuleFor(f => f.Cliente!.NumeroIdentificacion)
                    .NotEmpty().WithMessage("El cliente no tiene número de identificación.");
                RuleFor(f => f.Cliente!.Nombre)
                    .NotEmpty().WithMessage("El cliente no tiene nombre.");
            });

            // ── Ítems ────────────────────────────────────────────────────
            RuleFor(f => f.DetalleFacturas)
                .NotNull().Must(d => d!.Count > 0)
                .WithMessage("La factura no tiene ítems.");

            RuleForEach(f => f.DetalleFacturas).ChildRules(item =>
            {
                item.RuleFor(d => d.Cantidad).GreaterThan(0)
                    .WithMessage("Cada ítem debe tener cantidad mayor que cero.");
                item.RuleFor(d => d.PrecioUnitario).GreaterThanOrEqualTo(0)
                    .WithMessage("El precio unitario no puede ser negativo.");
            });

            // ── Formas de pago (si hay desglose) ─────────────────────────
            RuleForEach(f => f.FormasPago).ChildRules(fp =>
            {
                fp.RuleFor(p => p.MetodoPagoCodigo)
                    .Must(c => MediosPagoValidos.Contains(c))
                    .WithMessage(p => $"Medio de pago '{p.MetodoPagoCodigo}' no es un código DIAN válido.");
                fp.RuleFor(p => p.Valor).GreaterThan(0)
                    .WithMessage("Cada forma de pago debe tener un valor mayor que cero.");
            });

            // La suma del desglose debe coincidir con el total (tolerancia de redondeo)
            RuleFor(f => f)
                .Must(f => f.FormasPago == null || f.FormasPago.Count == 0
                        || Math.Abs(f.FormasPago.Sum(p => p.Valor) - f.TotalFactura) <= 0.01m)
                .WithMessage("La suma de las formas de pago no coincide con el total de la factura.");
        }
    }
}
