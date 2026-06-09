using NubeeAPI.Models.Suscripciones;

namespace NubeeAPI.DTOs.Login
{
    public class PlanLoginDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public bool IncluyePOS { get; set; }
    }
    public class UsuarioLoginDto
    {
       
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Apellido { get; set; }
        public string NombreCompleto => $"{Nombre} {Apellido ?? ""}".Trim();
        public string Correo { get; set; }
        public bool Estado { get; set; }
        public string Rol { get; set; } = "usuario";
        public int SuscripcionId { get; set; }
        public string PlanNombre { get; set; } = "Demo";
        public int DocumentosRestantes { get; set; }
        public DateTime? FechaExpiracion { get; set; }
        public DateTime? FechaDesactivacion { get; set; }
        public PlanLoginDto? Plan { get; set; }
        /// <summary>True si el usuario tiene cualquier suscripción activa con acceso a POS.</summary>
        public bool TienePos { get; set; }
    }
 
}
