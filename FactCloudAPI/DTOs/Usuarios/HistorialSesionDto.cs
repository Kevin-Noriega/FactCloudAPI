namespace FactCloudAPI.DTOs.Usuarios
{
    public class HistorialSesionDto
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public string IpAddress { get; set; } = "";
        public string Navegador { get; set; } = "";
        public string SistemaOperativo { get; set; } = "";
        public string Dispositivo { get; set; } = "";
        public string Ciudad { get; set; } = "";
        public string Pais { get; set; } = "";
        public bool Exitoso { get; set; }
        public bool SesionActual { get; set; }
    }
}
