using System.ComponentModel.DataAnnotations;

namespace FactCloudAPI.Models.Usuarios
{
    public class ConfiguracionDian
    {
        [Key]
        public int Id { get; set; }

        public string? SoftwareProveedor { get; set; }
        public string? SoftwarePIN { get; set; }

        public string? PrefijoAutorizadoDIAN { get; set; }
        public string? NumeroResolucionDIAN { get; set; }

        public string? RangoNumeracionDesde { get; set; }
        public string? RangoNumeracionHasta { get; set; }

        public string? AmbienteDIAN { get; set; }
        public DateTime? FechaVigenciaInicio { get; set; }
        public DateTime? FechaVigenciaFinal { get; set; }

        // FK
        public int NegocioId { get; set; }
        public Negocio Negocio { get; set; }
    }
}
