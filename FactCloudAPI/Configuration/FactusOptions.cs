using System.ComponentModel.DataAnnotations;

namespace NubeeAPI.Configuration
{
    /// <summary>
    /// Configuración de la integración con Factus (sección "Factus" de appsettings).
    /// Se enlaza vía Options pattern y se valida al arrancar.
    /// </summary>
    public class FactusOptions
    {
        public const string SectionName = "Factus";

        [Required] public string BaseUrl { get; set; } = "https://api-sandbox.factus.com.co";
        [Required] public string ClientId { get; set; } = "";
        [Required] public string ClientSecret { get; set; } = "";
        [Required] public string Username { get; set; } = "";
        [Required] public string Password { get; set; } = "";
    }
}
