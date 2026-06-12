using NubeeAPI.DTOs.Habilitacion;
using NubeeAPI.Models;
using NubeeAPI.Models.Usuarios;

namespace NubeeAPI.Services.Factus
{
    /// <summary>
    /// Contrato del servicio de integración con la API de Factus.
    /// Maneja autenticación OAuth2, refresh de token y llamadas REST.
    /// </summary>
    public interface IFactusService
    {
        /// <summary>
        /// Registra el rango de numeración DIAN del negocio en Factus
        /// (POST /v2/numbering-ranges) y devuelve el id del rango creado.
        /// </summary>
        Task<int> RegistrarRangoAsync(
            ResolucionDIAN resolucion,
            Negocio negocio,
            CancellationToken ct = default);

        /// <summary>
        /// Consulta el rango en Factus por su id (GET /v2/numbering-ranges/{id}).
        /// Devuelve un DTO simplificado con datos del rango o null si no existe.
        /// </summary>
        Task<FactusRangoActivoDto?> ObtenerRangoActivoAsync(
            int factusRangoId,
            CancellationToken ct = default);

        /// <summary>
        /// Verifica que la empresa exista y esté activa en Factus.
        /// Normalmente llamando a GET /v2/companies y comparando el NIT.
        /// </summary>
        Task<bool> VerificarEmpresaAsync(
            string nit,
            CancellationToken ct = default);

        /// <summary>
        /// Envía la factura a Factus para validación ante la DIAN
        /// (POST /v2/bills/validate) y devuelve datos clave (número, CUFE, etc.).
        /// </summary>
        Task<FactusRespuestaFactura> EnviarFacturaAsync(
            Factura factura,
            CancellationToken ct = default);

        /// <summary>
        /// Descarga el PDF oficial de la factura desde Factus.
        /// Recibe el número OFICIAL devuelto por Factus (ej. SETP990001103).
        /// </summary>
        Task<byte[]> DescargarPdfAsync(
            string numeroFactus,
            CancellationToken ct = default);

        /// <summary>
        /// Descarga el XML firmado de la factura desde Factus.
        /// Recibe el número OFICIAL devuelto por Factus.
        /// </summary>
        Task<string> DescargarXmlAsync(
            string numeroFactus,
            CancellationToken ct = default);

        /// <summary>
        /// Consulta una factura en Factus por su número oficial
        /// (GET /v2/bills/{number}). Devuelve el JSON crudo.
        /// </summary>
        Task<string> ConsultarFacturaAsync(
            string numeroFactus,
            CancellationToken ct = default);

        /// <summary>
        /// Verifica el estado de una factura en Factus/DIAN por su número oficial.
        /// Devuelve el campo "status" o "" si no se pudo leer.
        /// </summary>
        Task<string> ConsultarEstadoAsync(
            string numeroFactus,
            CancellationToken ct = default);

        /// <summary>
        /// Lista/filtra facturas en Factus (GET /v2/bills?{query}).
        /// Devuelve el JSON crudo.
        /// </summary>
        Task<string> ListarFacturasAsync(
            string? queryString = null,
            CancellationToken ct = default);

        /// <summary>
        /// Elimina una factura NO validada en Factus por su reference_code
        /// (DELETE /v2/bills/destroy/reference/{referenceCode}).
        /// </summary>
        Task<bool> EliminarFacturaNoValidadaAsync(
            string referenceCode,
            CancellationToken ct = default);
    }
}