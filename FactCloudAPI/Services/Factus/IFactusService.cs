using NubeeAPI.Models;
using NubeeAPI.Models.Usuarios;

namespace NubeeAPI.Services.Factus
{
    public interface IFactusService
    {
        /// <summary>Registra el rango de numeración DIAN de la empresa en Factus.</summary>
        Task<int> RegistrarRangoAsync(ResolucionDIAN resolucion, Negocio negocio, CancellationToken ct = default);

        /// <summary>Envía la factura a Factus para validación ante la DIAN.</summary>
        Task<FactusRespuestaFactura> EnviarFacturaAsync(Factura factura, CancellationToken ct = default);

        /// <summary>Descarga el PDF oficial de la factura desde Factus. Recibe el número OFICIAL devuelto por Factus (ej. SETP990001103).</summary>
        Task<byte[]> DescargarPdfAsync(string numeroFactus, CancellationToken ct = default);

        /// <summary>Descarga el XML firmado de la factura desde Factus. Recibe el número OFICIAL devuelto por Factus.</summary>
        Task<string> DescargarXmlAsync(string numeroFactus, CancellationToken ct = default);

        /// <summary>Consulta una factura en Factus por su número oficial (GET /v2/bills/{number}). Devuelve el JSON crudo.</summary>
        Task<string> ConsultarFacturaAsync(string numeroFactus, CancellationToken ct = default);

        /// <summary>Verifica el estado de una factura en Factus/DIAN por su número oficial. Devuelve el campo "status" o "" si no se pudo leer.</summary>
        Task<string> ConsultarEstadoAsync(string numeroFactus, CancellationToken ct = default);

        /// <summary>Lista/filtra facturas en Factus (GET /v2/bills). Devuelve el JSON crudo.</summary>
        Task<string> ListarFacturasAsync(string? queryString = null, CancellationToken ct = default);

        /// <summary>Elimina una factura NO validada en Factus por su reference_code.</summary>
        Task<bool> EliminarFacturaNoValidadaAsync(string referenceCode, CancellationToken ct = default);
    }
}
