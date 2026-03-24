namespace FactCloudAPI.DTOs.Productos
{
    public class ProductoDetalleDto
    {
        public int Id { get; set; }
        public bool EsServicio { get; set; }          // ← NUEVO
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? CodigoInterno { get; set; }
        public string? CodigoUNSPSC { get; set; }
        public string UnidadMedida { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal? Costo { get; set; }
        public string? ImpuestoCargo { get; set; }     // ← NUEVO
        public string? Retencion { get; set; }         // ← NUEVO
        public int? CantidadDisponible { get; set; }   // ← Nullable
        public int CantidadMinima { get; set; }
        public string? Categoria { get; set; }
        public bool IncluyeIVA { get; set; }           // ← NUEVO
        public bool Activo { get; set; }
    }


}
