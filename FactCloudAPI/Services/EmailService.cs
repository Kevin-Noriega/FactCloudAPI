using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using FactCloudAPI.Data;
using FactCloudAPI.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mime;


namespace FactCloudAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public EmailService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<bool> EnviarFacturaCliente(int facturaId)
        {
            var factura = await _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.Usuario)
                .Include(f => f.DetalleFacturas)
                .FirstOrDefaultAsync(f => f.Id == facturaId);

            if (factura == null)
                throw new Exception("Factura no encontrada");

            if (string.IsNullOrEmpty(factura.Cliente.Correo))
                throw new Exception("El cliente no tiene correo registrado");

            // Cuerpo HTML
            var builder = new BodyBuilder();
            builder.HtmlBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <title>Factura Electrónica #{factura.NumeroFactura}</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #00a2ff, #025b8f); color: white; padding: 20px; border-radius: 8px 8px 0 0; }}
        .content {{ background: #f9f9f9; padding: 20px; border: 1px solid #ddd; }}
        .table {{ width: 100%; border-collapse: collapse; margin: 20px 0; }}
        .table td {{ padding: 10px; border-bottom: 1px solid #ddd; }}
        .table td:first-child {{ font-weight: bold; width: 40%; }}
        .footer {{ background: #f0f0f0; padding: 15px; text-align: center; font-size: 12px; color: #666; border-radius: 0 0 8px 8px; }}
        .total {{ font-size: 24px; color: #00a2ff; font-weight: bold; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1 style='margin: 0;'>Factura Electrónica</h1>
            <p style='margin: 5px 0 0 0;'>{_config["Email:NombreEmpresa"]}</p>
        </div>
        <div class='content'>
            <h2>Estimado/a {factura.Cliente.Nombre}</h2>
            <p>Adjuntamos su factura electrónica con los siguientes detalles:</p>
            
            <table class='table'>
                <tr>
                    <td>Número de Factura:</td>
                    <td>{factura.NumeroFactura}</td>
                </tr>
                <tr>
                    <td>Fecha de Emisión:</td>
                    <td>{factura.FechaEmision:dd/MM/yyyy}</td>
                </tr>
                <tr>
                    <td>Fecha de Vencimiento:</td>
                    <td>{factura.FechaVencimiento:dd/MM/yyyy}</td>
                </tr>
                <tr>
                    <td>Forma de Pago:</td>
                    <td>{factura.FormaPago}</td>
                </tr>
                <tr>
                    <td>Medio de Pago:</td>
                    <td>{factura.MedioPago}</td>
                </tr>
                <tr>
                    <td>Subtotal:</td>
                    <td>${factura.Subtotal:N2}</td>
                </tr>
                <tr>
                    <td>IVA:</td>
                    <td>${factura.TotalIVA:N2}</td>
                </tr>
                <tr style='background: #e8f5ff;'>
                    <td><strong>TOTAL:</strong></td>
                    <td class='total'>${factura.TotalFactura:N2}</td>
                </tr>
            </table>

            {(string.IsNullOrEmpty(factura.Observaciones) ? "" :
                            $"<p><strong>Observaciones:</strong><br>{factura.Observaciones}</p>")}

            {(string.IsNullOrEmpty(factura.Cufe) ? "" :
                            $"<p style='font-size: 11px; color: #666;'><strong>CUFE:</strong> {factura.Cufe}</p>")}

            <p>Gracias por su confianza.</p>
        </div>
        <div class='footer'>
            <p>Este es un mensaje automático, por favor no responda a este correo.</p>
            <p>© {DateTime.Now.Year} {_config["Email:NombreEmpresa"]} - Todos los derechos reservados</p>
        </div>
    </div>
</body>
</html>";


            // 1. Cliente SendGrid
            var client = new SendGridClient(_config["SendGrid:ApiKey"]);
            Console.WriteLine($"FROM => '{_config["SendGrid:FromEmail"]}'");
            Console.WriteLine($"TO => '{factura.Cliente.Correo}'");

            var from = new EmailAddress(
                _config["SendGrid:FromEmail"],
                _config["SendGrid:FromName"]
            );

            var to = new EmailAddress(
                factura.Cliente.Correo,
                factura.Cliente.Nombre
            );

            var msg = MailHelper.CreateSingleEmail(
                from,
                to,
                $"Factura Electrónica #{factura.NumeroFactura}",
                plainTextContent: null,
                htmlContent: builder.HtmlBody
            );

            // 2. Adjuntar XML si existe
            if (!string.IsNullOrEmpty(factura.XmlBase64))
            {
                msg.AddAttachment(
                    $"Factura_{factura.NumeroFactura}.xml",
                    factura.XmlBase64,
                    "application/xml"
                );
            }

            // 3. Enviar
            var response = await client.SendEmailAsync(msg);
            if ((int)response.StatusCode >= 400)
            {
                var body = await response.Body.ReadAsStringAsync();
                throw new Exception($"Error SendGrid: {response.StatusCode} - {body}");
            }

            // 4. Actualizar factura
            factura.EnviadaCliente = true;
            factura.FechaEnvioCliente = DateTime.Now;
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
