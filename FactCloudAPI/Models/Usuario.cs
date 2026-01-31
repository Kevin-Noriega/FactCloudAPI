using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FactCloudAPI.Models.NotasDebito;
namespace FactCloudAPI.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [MaxLength(100)]
        public string? Apellido { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [MaxLength(20)]
        public string? Telefono { get; set; }

        [Required]
        public string ContrasenaHash { get; set; }  // Almacenar hash de contraseña

        [MaxLength(20)]
        public string? NitNegocio { get; set; }

        public int? DvNitNegocio { get; set; }

        [MaxLength(150)]
        public string? NombreNegocio { get; set; }

        [MaxLength(200)]
        public string? DireccionNegocio { get; set; }

        [MaxLength(100)]
        public string? CiudadNegocio { get; set; }

        [MaxLength(100)]
        public string? DepartamentoNegocio { get; set; }

        [EmailAddress]
        [MaxLength(150)]
        public string? CorreoNegocio { get; set; }

        [MaxLength(255)]
        public string? LogoNegocio { get; set; }

        [MaxLength(20)]
        public string? TipoIdentificacion { get; set; }

        public string? TipoPersona { get; set; }

        public string? NumeroIdentificacion { get; set; }
        [MaxLength(20)]
        public string? CodigoPostal { get; set; }

        [MaxLength(20)]
        public string? DepartamentoCodigo { get; set; }
        [MaxLength(20)]
        public string? CiudadCodigo { get; set; }

        [MaxLength(20)]
        public string? TelefonoNegocio { get; set; }

        [MaxLength(100)]
        public string? ActividadEconomicaCIIU { get; set; }

        [MaxLength(100)]
        public string? RegimenFiscal { get; set; }

        public string? RegimenTributario { get; set; }


        [MaxLength(5)]
        public string Pais { get; set; } = "CO";

        // Datos autorizados DIAN para facturación electrónica
        public string? SoftwareProveedor { get; set; }
        public string? SoftwarePIN { get; set; }
        public string? PrefijoAutorizadoDIAN { get; set; }
        public string? NumeroResolucionDIAN { get; set; }
        public string? FechaResolucionDIAN { get; set; }
        public string? RangoNumeracionDesde { get; set; }
        public string? RangoNumeracionHasta { get; set; }
        public string? AmbienteDIAN { get; set; }
        public string? FechaVigenciaInicio { get; set; }
        public string? FechaVigenciaFinal { get; set; }

        public bool Estado { get; set; } = true;
        public DateTime? FechaDesactivacion { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // Relaciones
        public ICollection<string>? ResponsabilidadesRut { get; set; } = new List<string>();

        public ICollection<Cliente>? Clientes { get; set; }
        public ICollection<Producto>? Productos { get; set; }
        public ICollection<Factura>? Facturas { get; set; }
        public ICollection<FactCloudAPI.Models.NotasDebito.NotaDebito> NotasDebito { get; set; }


    }
}
