using System.ComponentModel.DataAnnotations;

namespace FactCloudAPI.Models.Usuarios
{
    public class PerfilTributario
    {
        [Key]
        public int Id { get; set; }

        public int NegocioId { get; set; }
        public Negocio Negocio { get; set; }

        [MaxLength(20)]
        public string? RegimenIvaCodigo { get; set; }

        [MaxLength(10)]
        public string? ActividadEconomicaCIIU { get; set; }

        public string? TributosJson { get; set; }

        public string? ResponsabilidadesFiscalesJson { get; set; }
    }
}
