using System.ComponentModel.DataAnnotations;
namespace FactCloudAPI.DTOs.Usuarios
{
    public class ActivacionCuentaDto
    {
        [Required]
        public int PlanFacturacionId { get; set; }

        [Required]
        public string TransaccionId { get; set; } // ID de Wompi

        // Datos del negocio (opcionales en la activación)
        public string? NombreNegocio { get; set; }
        public string? Nit { get; set; }
        public int? DvNit { get; set; }
        public string? Direccion { get; set; }
        public string? Ciudad { get; set; }
        public string? Departamento { get; set; }
        public string? TelefonoNegocio { get; set; }
        public string? CorreoNegocio { get; set; }
    }

}
