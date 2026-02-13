using FactCloudAPI.Models.Suscripciones;

namespace FactCloudAPI.DTOs.Login
{
    public class UsuarioLoginDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Apellido { get; set; }
        public string NombreCompleto => $"{Nombre} {Apellido ?? ""}".Trim();
        public string Correo { get; set; }
        public bool Estado { get; set; }
        public int SuscripcionId { get; set; }
        public string PlanNombre { get; set; } = "Demo";
        public int DocumentosRestantes { get; set; }
        public DateTime? FechaExpiracion { get; set; }
        public DateTime? FechaDesactivacion { get; set; }
    }
}
