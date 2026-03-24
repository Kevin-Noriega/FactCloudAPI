namespace FactCloudAPI.Models.Wompi
{
    public class Transaccion
    {
        public int Id { get; set; }
        public string WompiId { get; set; } = "";
        public string Reference { get; set; } = "";
        public string Estado { get; set; } = "PENDING";
        public int PlanId { get; set; }
        public string DatosRegistro { get; set; } = "";
        public string DatosNegocio { get; set; } = "";
        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    }
}
