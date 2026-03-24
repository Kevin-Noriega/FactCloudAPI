using FactCloudAPI.Models;
using System.Xml.Linq;
namespace FactCloudAPI.Services
{
    public static class XmlFacturaGenerator
    {
        public static string GenerarXml(Factura factura)
        {
            var xml = new XDocument(
                new XElement("Invoice",
                    new XElement("NumeroFactura", factura.NumeroFactura),
                    new XElement("Fecha", factura.FechaEmision),
                    new XElement("Cliente",
                        new XElement("Nombre", factura.Cliente.Nombre),
                        new XElement("NIT", factura.Cliente.NumeroIdentificacion)
                    ),
                    new XElement("Total", factura.TotalFactura),
                    new XElement("IVA", factura.TotalIVA),
                    new XElement("CUFE", factura.Cufe),
                    new XElement("Items",
                        factura.DetalleFacturas.Select(d =>
                            new XElement("Item",
                                new XElement("Producto", d.Producto.Nombre),
                                new XElement("Cantidad", d.Cantidad),
                                new XElement("PrecioUnitario", d.PrecioUnitario),
                                new XElement("Subtotal", d.SubtotalLinea)
                            )
                        )
                    )
                )
            );

            return xml.ToString();
        }
    }
}
