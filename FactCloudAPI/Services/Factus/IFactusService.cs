using NubeeAPI.Models;
using NubeeAPI.Models.Usuarios;

namespace NubeeAPI.Services.Factus
{
    public interface IFactusService
    {
        /// <summary>Registra el rango de numeración DIAN de la empresa en Factus.</summary>
        Task<int> RegistrarRangoAsync(ResolucionDIAN resolucion, Negocio negocio);

        /// <summary>Envía la factura a Factus para validación ante la DIAN.</summary>
        Task<FactusRespuestaFactura> EnviarFacturaAsync(Factura factura);

        /// <summary>Descarga el PDF oficial de la factura desde Factus.</summary>
        Task<byte[]> DescargarPdfAsync(string numeroFactura);

        /// <summary>Descarga el XML firmado de la factura desde Factus.</summary>
        Task<string> DescargarXmlAsync(string numeroFactura);

        /// <summary>Verifica el estado de una factura en Factus/DIAN.</summary>
        Task<string> ConsultarEstadoAsync(string cufe);
    }
}
