using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactCloudAPI.Models.Usuarios
{
    public enum TipoSujeto
    {
        PersonaNatural = 1,
        PersonaJuridica = 2
    }

    public enum TipoDocumento
    {
        NIT = 1,
        CC = 2,
        CE = 3,
        Pasaporte = 4,
        Otro = 99
    }

    public class Negocio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public TipoSujeto TipoSujeto { get; set; }

        [Required]
        public TipoDocumento TipoDocumento { get; set; } = TipoDocumento.NIT;

        [MaxLength(200)]
        public string? NombreComercial { get; set; }

        [MaxLength(200)]
        public string? RazonSocial { get; set; }

        [MaxLength(100)]
        public string? PrimerNombre { get; set; }

        [MaxLength(100)]
        public string? SegundoNombre { get; set; }

        [MaxLength(100)]
        public string? PrimerApellido { get; set; }

        [MaxLength(100)]
        public string? SegundoApellido { get; set; }

        [Required, MaxLength(20)]
        public string NumeroIdentificacionE { get; set; }

        public int? DvNit { get; set; }

        [Required, MaxLength(200)]
        public string Direccion { get; set; }

        [Required, MaxLength(100)]
        public string Ciudad { get; set; }

        [MaxLength(100)]
        public string? Departamento { get; set; }

        [Required, MaxLength(2)]
        public string Pais { get; set; } = "CO";

        [Phone, MaxLength(30)]
        public string? Telefono { get; set; }

        [EmailAddress]
        public string CorreoElectronico { get; set; }

        [EmailAddress]
        public string? CorreoRecepcionDian { get; set; }

        public bool DatosFacturacionCompletos { get; set; } = false;

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public RepresentanteLegal? RepresentanteLegal { get; set; }
        public PerfilTributario? PerfilTributario { get; set; }
        public ConfiguracionDian? ConfiguracionDIAN { get; set; }

        public ICollection<ResolucionDIAN> Resoluciones { get; set; } = new List<ResolucionDIAN>();
    }

}
