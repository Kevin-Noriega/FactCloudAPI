using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NubeeAPI.Models
{
    /// <summary>
    /// Notificación in-app de un usuario. Se persiste en BD (historial / centro de
    /// notificaciones) y, al crearse, se emite en tiempo real por SignalR.
    /// </summary>
    public class Notificacion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        /// <summary>success | warning | info | error</summary>
        [Required]
        [MaxLength(20)]
        public string Tipo { get; set; } = "info";

        /// <summary>Dominio que originó la notificación: factura, producto, cliente, pos, usuario, plan, documento…</summary>
        [MaxLength(40)]
        public string? Categoria { get; set; }

        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Mensaje { get; set; } = string.Empty;

        /// <summary>Id de la entidad relacionada (ej. FacturaId), para navegar desde el front.</summary>
        public int? ReferenciaId { get; set; }

        /// <summary>Ruta relativa del front a la que enlaza la notificación.</summary>
        [MaxLength(300)]
        public string? Enlace { get; set; }

        public bool Leida { get; set; } = false;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Relación con Usuario
        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }
    }
}
