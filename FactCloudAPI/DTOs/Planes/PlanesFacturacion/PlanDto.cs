namespace FactCloudAPI.DTOs.Planes.PlanesFacturacion
{
    public class PlanDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; }

        public decimal PrecioMensual { get; set; }
        public decimal PrecioAnual { get; set; }

        public int? LimiteDocumentosMensual { get; set; }
        public int? LimiteUsuarios { get; set; }

        public bool FacturacionIlimitada { get; set; }
        public decimal? PrecioPorDocumento { get; set; }
    }
}
