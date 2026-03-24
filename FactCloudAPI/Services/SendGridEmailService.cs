using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

public class SendGridEmailService
{
    private readonly string _apiKey;
    private readonly string _fromEmail;
    private readonly string _fromName;

    public SendGridEmailService(IConfiguration config)
    {
        _apiKey = config["SendGrid:ApiKey"];
        _fromEmail = config["SendGrid:FromEmail"];
        _fromName = config["SendGrid:FromName"];
    }

    public async Task EnviarFacturaAsync(string toEmail, string toName, string asunto, string html)
    {
        var client = new SendGridClient(_apiKey);
        var from = new EmailAddress(_fromEmail, _fromName);
        var to = new EmailAddress(toEmail, toName);
        var msg = MailHelper.CreateSingleEmail(from, to, asunto, plainTextContent: null, htmlContent: html);

        var response = await client.SendEmailAsync(msg);
        // response.StatusCode debe ser 202 si todo salió bien
    }
}
