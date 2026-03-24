using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactCloudAPI.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        // ── Identificación ──────────────────────────────────────
        [Required]
        [MaxLength(50)]
        public string TipoPersona { get; set; }            // Natural / Juridica

        [Required]
        [MaxLength(50)]
        public string TipoIdentificacion { get; set; }     // CC, NIT, CE...

        [Required]
        [MaxLength(50)]
        public string NumeroIdentificacion { get; set; }

        public int? DigitoVerificacion { get; set; }

        [MaxLength(20)]
        public string CodigoSucursal { get; set; } = "0";  // ← NUEVO

        // ── Nombre ──────────────────────────────────────────────
        [Required]
        [MaxLength(200)]
        public string Nombre { get; set; }

        [MaxLength(200)]
        public string? Apellido { get; set; }

        [MaxLength(200)]
        public string? NombreComercial { get; set; }

        // ── Ubicación ───────────────────────────────────────────
        [Required]
        [MaxLength(100)]
        public string Departamento { get; set; }

        [MaxLength(20)]
        public string? DepartamentoCodigo { get; set; }

        [Required]
        [MaxLength(100)]
        public string Ciudad { get; set; }

        [MaxLength(20)]
        public string? CiudadCodigo { get; set; }

        [Required]
        [MaxLength(500)]
        public string Direccion { get; set; }

        [MaxLength(20)]
        public string? CodigoPostal { get; set; }

        [MaxLength(5)]
        public string Pais { get; set; } = "CO";

        [MaxLength(20)]
        public string? ActividadEconomicaCIIU { get; set; }

        // ── Facturación y envío ──────────────────────────────────
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Correo { get; set; }

        [MaxLength(100)]
        public string? RegimenTributario { get; set; }

        [MaxLength(100)]
        public string? RegimenFiscal { get; set; }

        [MaxLength(100)]
        public string? NombreContactoFacturacion { get; set; }   // ← NUEVO

        [MaxLength(100)]
        public string? ApellidoContactoFacturacion { get; set; } // ← NUEVO

        [MaxLength(20)]
        public string? IndicativoFacturacion { get; set; }       // ← NUEVO

        [MaxLength(50)]
        public string? TelefonoFacturacion { get; set; }         // ← NUEVO

        // ── Responsabilidades fiscales ────────────────────────────
        public bool GranContribuyente { get; set; } = false;    // ← NUEVO O-13
        public bool AutoretenedorRenta { get; set; } = false;    // O-15
        public bool RetenedorIVA { get; set; } = false;    // O-23
        public bool RegimenSimple { get; set; } = false;    // ← NUEVO O-47
        public bool NoAplica { get; set; } = false;    // ← NUEVO R-99-PN
        public bool RetenedorICA { get; set; } = false;
        public bool RetenedorRenta { get; set; } = false;

        // ── Estado y registro ────────────────────────────────────
        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // ── Relaciones ───────────────────────────────────────────
        public ICollection<TelefonoCliente> Telefonos { get; set; } = new List<TelefonoCliente>(); // ← NUEVO
        public ICollection<ContactoCliente> Contactos { get; set; } = new List<ContactoCliente>();
        public ICollection<Factura>? Facturas { get; set; }
        public ICollection<NotaDebito>? NotasDebito { get; set; }
    }
}