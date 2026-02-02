namespace FactCloudAPI.Models.DTOs
{
    public class CuentaDesactivadaResponse
    {
        public bool EstaDesactivada { get; set; } = true;
        public int DiasRestantes { get; set; }
        public string Mensaje { get; set; }
    }
}
