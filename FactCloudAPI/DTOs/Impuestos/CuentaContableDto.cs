using System.ComponentModel.DataAnnotations;

namespace FactCloudAPI.DTOs
{
    // ─── READ ────────────────────────────────────────────────────────────────
    public class CuentaContableDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string CodigoNombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int Nivel { get; set; }
        public string? CodigoPadre { get; set; }
        public int ClasePUC { get; set; }
        public string NombreClase { get; set; } = string.Empty;
        public string Naturaleza { get; set; } = string.Empty;
        public string TipoAjuste { get; set; } = string.Empty;
        public bool PermiteMovimiento { get; set; }
        public bool RequiereTercero { get; set; }
        public bool RequiereCentroCosto { get; set; }
        public bool RequiereDocumento { get; set; }
        public bool Activa { get; set; }
    }

    // ─── CREATE ──────────────────────────────────────────────────────────────
    public class CrearCuentaContableDto
    {
        [Required(ErrorMessage = "El código PUC es obligatorio")]
        [MaxLength(12)]
        [RegularExpression(@"^\d+$", ErrorMessage = "El código PUC solo debe contener dígitos")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de la cuenta es obligatorio")]
        [MaxLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Descripcion { get; set; }

        /// <summary>Si omite, se calcula automáticamente según longitud del Código.</summary>
        public int? Nivel { get; set; }

        [MaxLength(12)]
        public string? CodigoPadre { get; set; }

        /// <summary>Si omite, se infiere del primer dígito del Código.</summary>
        public int? ClasePUC { get; set; }

        [Required]
        [MaxLength(1)]
        [RegularExpression("^[DC]$", ErrorMessage = "Naturaleza debe ser 'D' (Débito) o 'C' (Crédito)")]
        public string Naturaleza { get; set; } = "D";

        [MaxLength(2)]
        public string TipoAjuste { get; set; } = "N";

        public bool PermiteMovimiento { get; set; } = true;
        public bool RequiereTercero { get; set; } = false;
        public bool RequiereCentroCosto { get; set; } = false;
        public bool RequiereDocumento { get; set; } = false;
    }

    // ─── UPDATE ──────────────────────────────────────────────────────────────
    public class ActualizarCuentaContableDto
    {
        [Required]
        [MaxLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Descripcion { get; set; }

        [MaxLength(1)]
        [RegularExpression("^[DC]$")]
        public string? Naturaleza { get; set; }

        [MaxLength(2)]
        public string? TipoAjuste { get; set; }

        public bool? PermiteMovimiento { get; set; }
        public bool? RequiereTercero { get; set; }
        public bool? RequiereCentroCosto { get; set; }
        public bool? RequiereDocumento { get; set; }
        public bool? Activa { get; set; }
    }
}
