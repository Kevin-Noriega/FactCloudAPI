namespace FactCloudAPI.DTOs.Usuarios
{
    
    
        // ─── DATOS GENERALES ──────────────────────────────────────────────────
        public class NegocioDatosGeneralesDto
        {
            public int TipoSujeto { get; set; }   // 1=PersonaNatural, 2=PersonaJuridica
            public int TipoDocumento { get; set; }   // 1=NIT, 2=CC, 3=CE, 4=Pasaporte, 99=Otro
            public string? NombreComercial { get; set; }
            public string? RazonSocial { get; set; }
            public string? PrimerNombre { get; set; }
            public string? SegundoNombre { get; set; }
            public string? PrimerApellido { get; set; }
            public string? SegundoApellido { get; set; }
            public string NumeroIdentificacionE { get; set; } = "";
            public int? DvNit { get; set; }
            public string Direccion { get; set; } = "";
            public string Ciudad { get; set; } = "";
            public string? Departamento { get; set; }
            public string Pais { get; set; } = "CO";
            public string? Telefono { get; set; }
            public string CorreoElectronico { get; set; } = "";
            public string? CorreoRecepcionDian { get; set; }
        }

        // ─── PERFIL TRIBUTARIO ────────────────────────────────────────────────
        public class PerfilTributarioDto
        {
            public string? RegimenIvaCodigo { get; set; }
            public string? ActividadEconomicaCIIU { get; set; }
            public string? TributosJson { get; set; }
            public string? ResponsabilidadesFiscalesJson { get; set; }
        }

        // ─── REPRESENTANTE LEGAL ──────────────────────────────────────────────
        public class RepresentanteLegalDto
        {
            public string Nombre { get; set; } = "";
            public string Apellidos { get; set; } = "";
            public int TipoDocumento { get; set; }
            public string NumeroIdentificacion { get; set; } = "";
            public string? CiudadExpedicion { get; set; }
            public string? CiudadResidencia { get; set; }
        }

        // ─── CONFIGURACIÓN DIAN ───────────────────────────────────────────────
        public class ConfiguracionDianDto
        {
            public string? SoftwareProveedor { get; set; }
            public string? SoftwarePIN { get; set; }
            public string? PrefijoAutorizadoDIAN { get; set; }
            public string? NumeroResolucionDIAN { get; set; }
            public string? RangoNumeracionDesde { get; set; }
            public string? RangoNumeracionHasta { get; set; }
            public string? AmbienteDIAN { get; set; }
            public DateTime? FechaVigenciaInicio { get; set; }
            public DateTime? FechaVigenciaFinal { get; set; }
        }

        // ─── RESPUESTA COMPLETA ───────────────────────────────────────────────
        public class NegocioCompletoResponse
        {
            public int Id { get; set; }
            public bool DatosFacturacionCompletos { get; set; }
            public NegocioDatosGeneralesDto? DatosGenerales { get; set; }
            public PerfilTributarioDto? PerfilTributario { get; set; }
            public RepresentanteLegalDto? RepresentanteLegal { get; set; }
            public ConfiguracionDianDto? ConfiguracionDian { get; set; }
            public List<string> PasosCompletados { get; set; } = new();
            public List<string> PasosPendientes { get; set; } = new();
        }
    

}
