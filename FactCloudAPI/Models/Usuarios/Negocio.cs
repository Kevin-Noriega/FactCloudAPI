using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NubeeAPI.Models.Usuarios
{
    public class Negocio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NombreNegocio { get; set; } = null!;

        public string? RazonSocial { get; set; }
        public string? Nit { get; set; }
        public int? DvNit { get; set; }

        public string? Direccion { get; set; }
        public string? Ciudad { get; set; }
        public string? Departamento { get; set; }

        public string Pais { get; set; } = "CO";
        public string? Telefono { get; set; }
        public string? Correo { get; set; }

        /// <summary>
        /// true cuando el perfil de facturación (empresa, perfil tributario, representante)
        /// está completo.
        /// </summary>
        public bool DatosFacturacionCompletos { get; set; } = false;

        /// <summary>
        /// true cuando se completó todo el flujo de habilitación electrónica
        /// (certificado, test set, resolución y rango en Factus).
        /// </summary>
        public bool HabilitacionCompleta { get; set; } = false;

        /// <summary>
        /// Texto libre para tipo de persona actual (ej. "PersonaNatural", "PersonaJuridica").
        /// </summary>
        public string? TipoPersona { get; set; }

        public string? ActividadEconomicaCIIU { get; set; }

        //  FK → Usuario
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        // 1 negocio → 1 configuración DIAN
        public ConfiguracionDian ConfiguracionDIAN { get; set; } = null!;

        public PerfilTributario? PerfilTributario { get; set; }

        public RepresentanteLegal? RepresentanteLegal { get; set; }

        /// <summary>
        /// Certificado digital asociado (propio o Nubee).
        /// </summary>
        public CertificadoDigital? CertificadoDigital { get; set; }

        /// <summary>
        /// Todas las resoluciones DIAN del negocio.
        /// </summary>
        public ICollection<ResolucionDIAN> Resoluciones { get; set; } = new List<ResolucionDIAN>();

        /// <summary>
        /// Resolución marcada como activa (o la primera vigente, según tu lógica).
        /// </summary>
        [NotMapped]
        public ResolucionDIAN? ResolucionActiva =>
            Resoluciones.FirstOrDefault(r => r.Activa);
    }
}