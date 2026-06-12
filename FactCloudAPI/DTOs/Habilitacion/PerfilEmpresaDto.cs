namespace NubeeAPI.DTOs.Habilitacion
{
    public class PerfilEmpresaDto
    {
        public string TipoPersona { get; set; } = "empresa"; // "empresa" | "persona_natural"

        public string? TipoIdentificacion { get; set; }  // ej. "NIT", "CC"
        public string? NumeroIdentificacion { get; set; }
        public int? Dv { get; set; }

        public string? Nombres { get; set; }          // para persona natural
        public string? Apellidos { get; set; }        // para persona natural

        public string? NombreComercial { get; set; }
        public string? RazonSocial { get; set; }
        public string? Correo { get; set; }
        public string? Direccion { get; set; }
        public string? Ciudad { get; set; }
        public string? Departamento { get; set; }
        public string? Telefono { get; set; }

        // Datos tributarios
        public string? RegimenIvaCodigo { get; set; }
        public string? ActividadEconomicaCIIU { get; set; }
        public List<string>? Tributos { get; set; }
        public List<string>? ResponsabilidadesFiscales { get; set; }

        // Representante legal
        public string? RepresentanteNombre { get; set; }
        public string? RepresentanteApellidos { get; set; }
        public string? RepresentanteTipoId { get; set; }    // mapearemos a enum
        public string? RepresentanteNumeroId { get; set; }
        public string? CiudadExpedicion { get; set; }
        public string? CiudadResidencia { get; set; }

        // Acceso proveedor tecnológico
        public string? CorreoAcceso { get; set; }
    }
}
