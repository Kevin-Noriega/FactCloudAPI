namespace FactCloudAPI.DTOs.Productos
{
    public class ProductoCreateDto
    {
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }

        public string? CodigoInterno { get; set; }
        public string? CodigoUNSPSC { get; set; }

        public string UnidadMedida { get; set; }

        public string? Marca { get; set; }
        public string? Modelo { get; set; }

        public decimal PrecioUnitario { get; set; }
        public decimal? Costo { get; set; }

        public string TipoImpuesto { get; set; }
        public decimal TarifaIVA { get; set; }

        public bool ProductoExcluido { get; set; }
        public bool ProductoExento { get; set; }

        public bool GravaINC { get; set; }
        public decimal? TarifaINC { get; set; }

        public int CantidadDisponible { get; set; }
        public int CantidadMinima { get; set; }

        public string? Categoria { get; set; }
        public string? CodigoBarras { get; set; }
    }

}
