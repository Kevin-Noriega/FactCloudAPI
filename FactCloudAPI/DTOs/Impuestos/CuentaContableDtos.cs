using System.ComponentModel.DataAnnotations;

namespace NubeeAPI.DTOs.Impuestos
{
    // ── READ: lista plana ────────────────────────────────────────────────
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

    // ── READ: árbol jerárquico ────────────────────────────────────────────
    public class CuentaContableArbolDto : CuentaContableDto
    {
        public List<CuentaContableArbolDto> Hijos { get; set; } = new();
    }

   

    // ── CREATE ────────────────────────────────────────────────────────────
    public class CrearCuentaContableDto
    {
        /// <summary>
        /// Código PUC:
        /// 1 dígito  → Clase     (ej: "1")
        /// 2 dígitos → Grupo     (ej: "11")
        /// 4 dígitos → Cuenta    (ej: "1105")
        /// 6 dígitos → Subcuenta (ej: "110505")
        /// 8+ dígitos → Auxiliar (ej: "11050501")
        /// </summary>
        [Required(ErrorMessage = "El código PUC es obligatorio")]
        [MaxLength(12)]
        [RegularExpression(@"^\d+$", ErrorMessage = "El código PUC solo debe contener dígitos")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de la cuenta es obligatorio")]
        [MaxLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Descripcion { get; set; }

        /// <summary>Si se omite, se calcula automáticamente según la longitud del código.</summary>
        public int? Nivel { get; set; }

        [MaxLength(12)]
        public string? CodigoPadre { get; set; }

        /// <summary>Si se omite, se infiere del primer dígito del código.</summary>
        public int? ClasePUC { get; set; }

        [MaxLength(1)]
        [RegularExpression("^[DC]$", ErrorMessage = "Naturaleza debe ser 'D' (Débito) o 'C' (Crédito)")]
        public string? Naturaleza { get; set; }

        [MaxLength(2)]
        [RegularExpression("^(M|NM|N)$", ErrorMessage = "TipoAjuste debe ser 'M', 'NM' o 'N'")]
        public string? TipoAjuste { get; set; }

        public bool PermiteMovimiento { get; set; } = true;
        public bool RequiereTercero { get; set; } = false;
        public bool RequiereCentroCosto { get; set; } = false;
        public bool RequiereDocumento { get; set; } = false;
    }

    // ── UPDATE: parcial ───────────────────────────────────────────────────
    public class ActualizarCuentaContableDto
    {
        [MaxLength(200)]
        public string? Nombre { get; set; }

        [MaxLength(1000)]
        public string? Descripcion { get; set; }

        [MaxLength(1)]
        [RegularExpression("^[DC]$", ErrorMessage = "Naturaleza debe ser 'D' (Débito) o 'C' (Crédito)")]
        public string? Naturaleza { get; set; }

        [MaxLength(2)]
        [RegularExpression("^(M|NM|N)$", ErrorMessage = "TipoAjuste debe ser 'M', 'NM' o 'N'")]
        public string? TipoAjuste { get; set; }

        public bool? PermiteMovimiento { get; set; }
        public bool? RequiereTercero { get; set; }
        public bool? RequiereCentroCosto { get; set; }
        public bool? RequiereDocumento { get; set; }
        public bool? Activa { get; set; }
    }
}