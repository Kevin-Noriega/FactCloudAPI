using System.ComponentModel.DataAnnotations;

namespace NubeeAPI.DTOs.Admin
{
    // ── Crear usuario (admin) ─────────────────────────────────────────
    public class CrearUsuarioAdminDto
    {
        [Required, MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Apellido { get; set; }

        [Required, EmailAddress]
        public string Correo { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Contrasena { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Telefono { get; set; }

        [MaxLength(30)]
        public string Rol { get; set; } = "usuario";

        public bool Estado { get; set; } = true;
    }

    // ── Editar usuario (admin) ────────────────────────────────────────
    public class EditarUsuarioAdminDto
    {
        [MaxLength(100)]
        public string? Nombre { get; set; }

        [MaxLength(100)]
        public string? Apellido { get; set; }

        [EmailAddress]
        public string? Correo { get; set; }

        [MaxLength(20)]
        public string? Telefono { get; set; }

        public bool? Estado { get; set; }

        [MaxLength(30)]
        public string? Rol { get; set; }

        [MinLength(6)]
        public string? Contrasena { get; set; }
    }

    // ── Cambiar rol ───────────────────────────────────────────────────
    public class CambiarRolDto
    {
        [Required, MaxLength(30)]
        public string Rol { get; set; } = string.Empty;
    }

    // ── Reset contraseña (admin) ──────────────────────────────────────
    public class ResetPasswordAdminDto
    {
        [Required, MinLength(6)]
        public string NuevaContrasena { get; set; } = string.Empty;
    }

    // ── Respuesta: listado de usuarios (admin) ────────────────────────
    public class UsuarioAdminDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Apellido { get; set; }
        public string Correo { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public bool Estado { get; set; }
        public string Rol { get; set; } = "usuario";
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaDesactivacion { get; set; }
        public string? PlanNombre { get; set; }
        public string? NombreNegocio { get; set; }
        public bool TieneSuscripcionActiva { get; set; }
    }
}
