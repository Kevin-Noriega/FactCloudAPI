using System.ComponentModel.DataAnnotations;

namespace NubeeAPI.Models.Pos
{
    /// <summary>
    /// Preferencias de impresión del POS por usuario.
    /// </summary>
    public class ConfiguracionImpresionPos
    {
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        /// <summary>"navegador" | "directa".</summary>
        [MaxLength(20)]
        public string MetodoImpresion { get; set; } = "navegador";

        [MaxLength(150)]
        public string? ImpresoraDefecto { get; set; }

        [MaxLength(30)]
        public string? TamanoPapel { get; set; }

        public int Copias { get; set; } = 1;

        /// <summary>Impresión simple (sin logo).</summary>
        public bool ImpresionSimple { get; set; } = false;

        // Márgenes en milímetros
        public int MargenSuperior { get; set; } = 2;
        public int MargenInferior { get; set; } = 2;
        public int MargenIzquierdo { get; set; } = 2;
        public int MargenDerecho { get; set; } = 2;
    }
}
