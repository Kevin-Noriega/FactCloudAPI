using NubeeAPI.Models;
using NubeeAPI.Models.Pos;

namespace NubeeAPI.Services.Facturas
{
    /// <summary>
    /// Emite una factura electrónica DIAN a partir de una venta del POS,
    /// reutilizando el mismo pipeline de numeración + CUFE + XML que la
    /// facturación normal. Lanza <see cref="Utils.Exceptions.BusinessException"/>
    /// con un mensaje claro cuando la venta no es facturable (ítems manuales,
    /// sin resolución vigente, etc.).
    /// </summary>
    public interface IPosFacturacionService
    {
        Task<Factura> EmitirDesdeVentaAsync(PosVenta venta, int usuarioId, CancellationToken ct = default);
    }
}
