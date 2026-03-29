namespace FactCloudAPI.DTOs.Habilitacion
{
    public class ConfiguracionSoftwareDto
    {
        public string NitFabricante { get; set; } = null!;
        public string NombreSoftware { get; set; } = null!;
        public string CodigoSoftware { get; set; } = null!;
    }

    public class TestSetDto
    {
        public string TestSetId { get; set; } = null!;
    }

    public class ResolucionDianDto
    {
        public string NumeroAutorizacion { get; set; } = null!;
        public string? Prefijo { get; set; }
        public int RangoDesde { get; set; }
        public int RangoHasta { get; set; }
        public string FechaInicio { get; set; } = null!;  // "yyyy-MM-dd"
        public string FechaFin { get; set; } = null!;  // "yyyy-MM-dd"
        public string ClaveTecnica { get; set; } = null!;
        public string TipoAmbiente { get; set; } = null!;  // "1" prod / "2" pruebas
    }
}