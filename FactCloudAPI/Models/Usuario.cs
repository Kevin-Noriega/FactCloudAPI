using System.ComponentModel.DataAnnotations;
using FactCloudAPI.Models.Usuarios;
using FactCloudAPI.Models.Suscripciones;

namespace FactCloudAPI.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        public string? TipoIdentificacion { get; set; }

        public string? NumeroIdentificacion { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; }

        [MaxLength(100)]
        public string? Apellido { get; set; }

        [MaxLength(20)]
        public string? Telefono { get; set; }

        [Required, EmailAddress]
        public string Correo { get; set; }

        [Required]
        public string ContrasenaHash { get; set; }

        public bool Estado { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
        public DateTime? FechaDesactivacion { get; set; }

        //  1 usuario → 1 negocio
        public Negocio Negocio { get; set; }

        // Otras relaciones
        public ICollection<Cliente>? Clientes { get; set; }
        public ICollection<Producto>? Productos { get; set; }
        public ICollection<Factura>? Facturas { get; set; }
        public ICollection<NotaDebito>? NotasDebito { get; set; }

        public ICollection<SuscripcionFacturacion> Suscripciones { get; set; }
            = new List<SuscripcionFacturacion>();
    }
}
