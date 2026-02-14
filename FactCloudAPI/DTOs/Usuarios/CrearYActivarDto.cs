using System.ComponentModel.DataAnnotations;

namespace FactCloudAPI.DTOs.Usuarios
{
    public class CrearYActivarDto
    {  // Usuario
        [Required]
        public string Nombre { get; set; } = null!;

        public string? Telefono { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;

        public string? TipoIdentificacion { get; set; }
        public string? NumeroIdentificacion { get; set; }
        public string? Pais { get; set; }

        // Negocio
        [Required]
        public string NombreNegocio { get; set; } = null!;

        [Required]
        public string Nit { get; set; } = null!;

        public int? DvNit { get; set; }
        public string? Direccion { get; set; }
        public string? Ciudad { get; set; }
        public string? Departamento { get; set; }
        public string? TelefonoNegocio { get; set; }
        public string? CorreoNegocio { get; set; }

        // Suscripción
        [Required]
        public int PlanFacturacionId { get; set; }

        [Required]
        public string TransaccionId { get; set; } = null!;

        public string TipoPago { get; set; } = "anual"; // "mensual" o "anual"
        public decimal PrecioPagado { get; set; }
    }
    }
