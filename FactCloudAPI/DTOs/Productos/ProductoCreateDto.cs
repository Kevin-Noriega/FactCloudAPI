namespace FactCloudAPI.DTOs.Productos
{
    public class ProductoCreateDto
    {
        public bool EsServicio { get; set; }          // ← NUEVO
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? CodigoInterno { get; set; }
        public string? CodigoUNSPSC { get; set; }
        public string UnidadMedida { get; set; } = "Unidad";
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? Categoria { get; set; }
        public string? CodigoBarras { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal? Costo { get; set; }
        public bool IncluyeIVA { get; set; }
        public string? ImpuestoCargo { get; set; }     // ← NUEVO
        public string? Retencion { get; set; }         // ← NUEVO
        public int? CantidadDisponible { get; set; }   // ← Nullable
        public int CantidadMinima { get; set; } = 0;
        public string? TipoProducto { get; set; }
    }

}
