using System.ComponentModel.DataAnnotations;

namespace FactCloudAPI.Models.Usuarios
{
    public class Negocio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NombreNegocio { get; set; }

        public string? RazonSocial { get; set; }
        public string? Nit { get; set; }
        public int? DvNit { get; set; }

        public string? Direccion { get; set; }
        public string? Ciudad { get; set; }
        public string? Departamento { get; set; }

        public string Pais { get; set; } = "CO";
        public string? Telefono { get; set; }
        public string? Correo { get; set; }

        public string? TipoPersona { get; set; }
        public string? ActividadEconomicaCIIU { get; set; }

        //  FK → Usuario
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        // 1 negocio → 1 configuración DIAN
        public ConfiguracionDian ConfiguracionDIAN { get; set; }
    }
}
