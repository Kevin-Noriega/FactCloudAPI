namespace FactCloudAPI.DTOs.Addons
{
    // ── Response: addon disponible en la tienda ──
    public class AddonResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Unidad { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public string? Color { get; set; }
        public bool Contratado { get; set; } // true si el usuario ya lo tiene
    }

    // ── Response: addon contratado por el usuario ──
    public class UsuarioAddonResponseDto
    {
        public int Id { get; set; }
        public int AddonId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Unidad { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public string? Color { get; set; }
        public DateTime FechaContratacion { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public bool Activo { get; set; }
    }

    // ── Request: agregar addons ──
    public class AgregarAddonsRequest
    {
        public List<int> Addons { get; set; } = new();
    }

    // ── Request: cancelar addon ──
    public class CancelarAddonRequest
    {
        public int AddonId { get; set; }
    }
}