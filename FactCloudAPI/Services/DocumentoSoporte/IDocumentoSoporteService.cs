using FactCloudAPI.Models;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace FactCloudAPI.Services
{
    public interface IDocumentoSoporteService
    {
        string GenerarCUDS(string prefijo, int consecutivo, DateTime fecha, string nitAdquiriente,
            string nitProveedor, decimal valorTotal, string nitSoftwareProvider, string pinSoftware);
        string GenerarXML(DocumentoSoporte documento, Usuario usuario);
        byte[] GenerarPDF(DocumentoSoporte documento, Usuario usuario);
        Task<bool> EnviarADIAN(DocumentoSoporte documento, string xmlContent);
    }

    public class DocumentoSoporteService : IDocumentoSoporteService
    {
        private readonly IConfiguration _configuration;

        public DocumentoSoporteService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Genera el CUDS según especificaciones DIAN
        /// Algoritmo: SHA-384 del string concatenado
        /// </summary>
        public string GenerarCUDS(string prefijo, int consecutivo, DateTime fecha,
            string nitAdquiriente, string nitProveedor, decimal valorTotal,
            string nitSoftwareProvider, string pinSoftware)
        {
            // Formato: Prefijo + Consecutivo + Fecha(yyyyMMdd) + Hora(HHmmss) + 
            //          ValorTotal + CodigoImpuesto1 + ValorImpuesto1 + 
            //          CodigoImpuesto2 + ValorImpuesto2 + ... +
            //          ValorPagar + NITAdquiriente + NITProveedor +
            //          ClaveT + TipoAmbiente

            string fechaFormato = fecha.ToString("yyyyMMdd");
            string horaFormato = fecha.ToString("HHmmss");
            string valorFormato = valorTotal.ToString("0.00").Replace(",", "");

            // Construcción del string base para CUDS
            string cadenaBase = $"{prefijo}{consecutivo:D10}{fechaFormato}{horaFormato}" +
                               $"{valorFormato}01{valorFormato}04{valorFormato}" +
                               $"{valorFormato}{nitAdquiriente}{nitProveedor}" +
                               $"{nitSoftwareProvider}{pinSoftware}";

            // Generar hash SHA-384
            using (SHA384 sha384 = SHA384.Create())
            {
                byte[] hashBytes = sha384.ComputeHash(Encoding.UTF8.GetBytes(cadenaBase));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// Genera el XML del documento soporte según anexo técnico DIAN
        /// </summary>
        public string GenerarXML(DocumentoSoporte documento, Usuario usuario)
        {
            XNamespace cac = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";
            XNamespace cbc = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
            XNamespace ext = "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2";
            XNamespace sts = "dian:gov:co:facturaelectronica:Structures-2-1";
            XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
            XNamespace ds = "http://www.w3.org/2000/09/xmldsig#";

            XDocument xml = new XDocument(
                new XDeclaration("1.0", "UTF-8", "no"),
                new XElement(sts + "Invoice",
                    new XAttribute(XNamespace.Xmlns + "cac", cac),
                    new XAttribute(XNamespace.Xmlns + "cbc", cbc),
                    new XAttribute(XNamespace.Xmlns + "ext", ext),
                    new XAttribute(XNamespace.Xmlns + "sts", sts),
                    new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                    new XAttribute(XNamespace.Xmlns + "ds", ds),

                    // UBL Versión
                    new XElement(cbc + "UBLVersionID", "UBL 2.1"),
                    new XElement(cbc + "CustomizationID", "10"),
                    new XElement(cbc + "ProfileID", "DIAN 2.1: Documento Soporte en Adquisiciones efectuadas a Sujetos No Obligados a Facturar"),
                    new XElement(cbc + "ProfileExecutionID", "1"), // 1=Producción, 2=Pruebas

                    // ID del documento (CUDS)
                    new XElement(cbc + "ID", documento.CUDS),
                    new XElement(cbc + "UUID",
                        new XAttribute("schemeName", "CUDS-SHA384"),
                        documento.CUDS),

                    // Fecha y hora de emisión
                    new XElement(cbc + "IssueDate", documento.FechaGeneracion.ToString("yyyy-MM-dd")),
                    new XElement(cbc + "IssueTime", documento.FechaGeneracion.ToString("HH:mm:ss") + "-05:00"),

                    // Tipo de documento (05 = Documento Soporte)
                    new XElement(cbc + "InvoiceTypeCode", "05"),

                    // Numeración
                    new XElement(cbc + "Note", $"{documento.Prefijo}{documento.Consecutivo}"),

                    // Moneda
                    new XElement(cbc + "DocumentCurrencyCode", "COP"),

                    // Información del Adquiriente (quien emite el documento soporte)
                    new XElement(cac + "AccountingSupplierParty",
                        new XElement(cac + "Party",
                            new XElement(cac + "PartyIdentification",
                                new XElement(cbc + "ID",
                                    new XAttribute("schemeName", usuario.TipoIdentificacion),
                                    new XAttribute("schemeID", usuario.TipoIdentificacion),
                                    usuario.NumeroIdentificacion)),
                            new XElement(cac + "PartyName",
                                new XElement(cbc + "Name", usuario.NombreNegocio)),
                            new XElement(cac + "PhysicalLocation",
                                new XElement(cac + "Address",
                                    new XElement(cbc + "ID", "11001"), // Código ciudad
                                    new XElement(cbc + "CityName", usuario.CiudadNegocio),
                                    new XElement(cbc + "CountrySubentity", usuario.DepartamentoNegocio),
                                    new XElement(cbc + "CountrySubentityCode", "11"),
                                    new XElement(cac + "AddressLine",
                                        new XElement(cbc + "Line", usuario.DireccionNegocio)),
                                    new XElement(cac + "Country",
                                        new XElement(cbc + "IdentificationCode", "CO")))),
                            new XElement(cac + "PartyTaxScheme",
                                new XElement(cbc + "RegistrationName", usuario.NombreNegocio),
                                new XElement(cbc + "CompanyID",
                                    new XAttribute("schemeName", usuario.TipoIdentificacion),
                                    usuario.NumeroIdentificacion),
                                new XElement(cac + "TaxScheme",
                                    new XElement(cbc + "ID", "01"),
                                    new XElement(cbc + "Name", "IVA"))))),

                    // Información del Proveedor (No obligado a facturar)
                    new XElement(cac + "AccountingCustomerParty",
                        new XElement(cac + "Party",
                            new XElement(cac + "PartyIdentification",
                                new XElement(cbc + "ID",
                                    new XAttribute("schemeName", documento.ProveedorTipoIdentificacion),
                                    new XAttribute("schemeID", documento.ProveedorTipoIdentificacion),
                                    documento.ProveedorNit)),
                            new XElement(cac + "PartyName",
                                new XElement(cbc + "Name", documento.ProveedorNombre)))),

                    // Totales
                    new XElement(cac + "LegalMonetaryTotal",
                        new XElement(cbc + "LineExtensionAmount",
                            new XAttribute("currencyID", "COP"),
                            documento.Subtotal.ToString("0.00")),
                        new XElement(cbc + "TaxExclusiveAmount",
                            new XAttribute("currencyID", "COP"),
                            documento.Subtotal.ToString("0.00")),
                        new XElement(cbc + "TaxInclusiveAmount",
                            new XAttribute("currencyID", "COP"),
                            documento.ValorTotal.ToString("0.00")),
                        new XElement(cbc + "AllowanceTotalAmount",
                            new XAttribute("currencyID", "COP"),
                            documento.Descuento.ToString("0.00")),
                        new XElement(cbc + "PayableAmount",
                            new XAttribute("currencyID", "COP"),
                            documento.ValorTotal.ToString("0.00"))),

                    // Líneas del documento
                    new XElement(cac + "InvoiceLine",
                        new XElement(cbc + "ID", "1"),
                        new XElement(cbc + "InvoicedQuantity", "1"),
                        new XElement(cbc + "LineExtensionAmount",
                            new XAttribute("currencyID", "COP"),
                            documento.Subtotal.ToString("0.00")),
                        new XElement(cac + "Item",
                            new XElement(cbc + "Description", documento.Descripcion)),
                        new XElement(cac + "Price",
                            new XElement(cbc + "PriceAmount",
                                new XAttribute("currencyID", "COP"),
                                documento.Subtotal.ToString("0.00"))))
                )
            );

            return xml.ToString();
        }

        /// <summary>
        /// Genera el PDF del documento soporte
        /// </summary>
        public byte[] GenerarPDF(DocumentoSoporte documento, Usuario usuario)
        {
            // Aquí puedes usar librerías como iTextSharp, QuestPDF o similar
            // Por ahora retorno un placeholder
            // TODO: Implementar generación de PDF con representación gráfica DIAN

            return Encoding.UTF8.GetBytes("PDF Placeholder - Implementar con iTextSharp/QuestPDF");
        }

        /// <summary>
        /// Envía el documento a DIAN para validación
        /// </summary>
        public async Task<bool> EnviarADIAN(DocumentoSoporte documento, string xmlContent)
        {
            // TODO: Implementar integración con Web Services DIAN
            // - Firmar XML con certificado digital
            // - Enviar a DIAN mediante SOAP/REST
            // - Procesar respuesta (CUDS válido, aceptado/rechazado)

            // Por ahora simulo respuesta exitosa
            await Task.Delay(100);
            return true;
        }
    }
}
