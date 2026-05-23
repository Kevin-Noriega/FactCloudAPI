using System.Net;
using System.Net.Mail;

namespace NubeeAPI.Services
{
    public class ConfiguracionEmpresa
    {
        public string NombreEmpresa { get; set; } = "";
        public string Nit { get; set; } = "";
        public string Direccion { get; set; } = "";
        public string Ciudad { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string Correo { get; set; } = "";
        public string SitioWeb { get; set; } = "";
        public string RegimenFiscal { get; set; } = "Simplificado";
        public string? LogoUrl { get; set; }
    }

    public class ConfiguracionSmtp
    {
        public string Host { get; set; } = "";
        public int Puerto { get; set; } = 587;
        public string Usuario { get; set; } = "";
        public string Contrasena { get; set; } = "";
        public string Remitente { get; set; } = "";
        public string NombreRemitente { get; set; } = "FactCloud";
        public bool UsarTls { get; set; } = true;
    }

    public class AdminConfigDian
    {
        public string Ambiente { get; set; } = "Habilitacion";
        public string NumeroResolucion { get; set; } = "";
        public string Prefijo { get; set; } = "FE";
        public long RangoDesde { get; set; } = 1;
        public long RangoHasta { get; set; } = 1000000;
        public DateTime? FechaResolucion { get; set; }
        public string NitEmisor { get; set; } = "";
        public string? ClaveCertificado { get; set; }
    }

    public class ConfiguracionApariencia
    {
        public string NombrePlataforma { get; set; } = "FactCloud";
        public string ColorPrimario { get; set; } = "#2563eb";
        public string ColorSecundario { get; set; } = "#1e40af";
        public string? LogoUrl { get; set; }
        public string? FaviconUrl { get; set; }
        public string? Slogan { get; set; }
    }

    public class ConfiguracionMantenimiento
    {
        public bool ModoMantenimiento { get; set; } = false;
        public string MensajeMantenimiento { get; set; } = "El sistema se encuentra en mantenimiento. Por favor, intente más tarde.";
        public DateTime? InicioMantenimiento { get; set; }
        public DateTime? FinEstimado { get; set; }
    }

    public class ConfiguracionService
    {
        public ConfiguracionEmpresa Empresa { get; private set; } = new();
        public ConfiguracionSmtp Smtp { get; private set; } = new();
        public AdminConfigDian Dian { get; private set; } = new();
        public ConfiguracionApariencia Apariencia { get; private set; } = new();
        public ConfiguracionMantenimiento Mantenimiento { get; private set; } = new();
        public DateTime FechaInicio { get; } = DateTime.UtcNow;

        public void ActualizarEmpresa(ConfiguracionEmpresa datos) => Empresa = datos;
        public void ActualizarSmtp(ConfiguracionSmtp datos) => Smtp = datos;
        public void ActualizarDian(AdminConfigDian datos) => Dian = datos;
        public void ActualizarApariencia(ConfiguracionApariencia datos) => Apariencia = datos;
        public void ActualizarMantenimiento(ConfiguracionMantenimiento datos) => Mantenimiento = datos;

        public async Task<(bool ok, string mensaje)> ProbarSmtp(string correoDestino)
        {
            if (string.IsNullOrWhiteSpace(Smtp.Host))
                return (false, "Configure el servidor SMTP primero.");

            try
            {
                using var client = new SmtpClient(Smtp.Host, Smtp.Puerto)
                {
                    Credentials = new NetworkCredential(Smtp.Usuario, Smtp.Contrasena),
                    EnableSsl = Smtp.UsarTls,
                    Timeout = 10_000
                };
                var msg = new MailMessage
                {
                    From = new MailAddress(Smtp.Remitente, Smtp.NombreRemitente),
                    Subject = "Prueba de conexión SMTP — FactCloud",
                    Body = "Este es un correo de prueba generado desde el panel de administración de FactCloud.",
                    IsBodyHtml = false,
                };
                msg.To.Add(correoDestino);
                await client.SendMailAsync(msg);
                return (true, "Correo de prueba enviado correctamente.");
            }
            catch (Exception ex)
            {
                return (false, $"Error al conectar con SMTP: {ex.Message}");
            }
        }
    }
}
