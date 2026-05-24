using NubeeAPI.Models.Impuestos;

namespace NubeeAPI.Services.Impuestos
{
    public static class AsientoContableValidator
    {
        private const decimal ToleranciaRedondeo = 0.01m;

        /// <summary>
        /// Valida que el comprobante cuadre: SUM(Debito) == SUM(Credito).
        /// Lanza InvalidOperationException si no cuadra — nunca persistir un asiento descuadrado.
        /// </summary>
        public static void ValidarCuadre(IEnumerable<AsientoContable> lineas)
        {
            var lista = lineas.ToList();

            if (!lista.Any())
                throw new InvalidOperationException("El comprobante no tiene líneas.");

            var totalDebito = lista.Sum(l => l.Debito);
            var totalCredito = lista.Sum(l => l.Credito);
            var diferencia = Math.Abs(totalDebito - totalCredito);

            if (diferencia > ToleranciaRedondeo)
                throw new InvalidOperationException(
                    $"El asiento no cuadra. Débitos: {totalDebito:N2} | " +
                    $"Créditos: {totalCredito:N2} | Diferencia: {diferencia:N2}");
        }

        /// <summary>
        /// Valida que cada línea individualmente no tenga débito Y crédito simultáneos.
        /// </summary>
        public static void ValidarLineas(IEnumerable<AsientoContable> lineas)
        {
            foreach (var linea in lineas)
            {
                if (linea.Debito > 0 && linea.Credito > 0)
                    throw new InvalidOperationException(
                        $"La línea CuentaId={linea.CuentaId} tiene débito y crédito simultáneos.");

                if (linea.Debito < 0 || linea.Credito < 0)
                    throw new InvalidOperationException(
                        $"La línea CuentaId={linea.CuentaId} tiene valor negativo.");
            }
        }
    }
}