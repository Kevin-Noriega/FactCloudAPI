using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FactCloudAPI.DTOs.NotaDebito
{
        public class NotaDebitoDto
        {
            [Required]
            public int UsuarioId { get; set; }

            [Required]
            public int FacturaId { get; set; }

            [Required]
            [MaxLength(50)]
            public string NumeroNota { get; set; } = string.Empty;

            [Required]
            [MaxLength(20)]
            public string Tipo { get; set; } = "DF-1";

            [Required]
            [MaxLength(50)]
            public string MotivoDIAN { get; set; } = string.Empty;

            [Required]
            public DateTime FechaElaboracion { get; set; }

            [MaxLength(500)]
            public string? CUFE { get; set; }

            public decimal TotalBruto { get; set; }
            public decimal TotalDescuentos { get; set; }
            public decimal Subtotal { get; set; }
            public decimal TotalIVA { get; set; }
            public decimal TotalINC { get; set; }
            public decimal ReteICA { get; set; }
            public decimal TotalNeto { get; set; }

            [MaxLength(1000)]
            public string? Observaciones { get; set; }

            [Required]
            [MaxLength(20)]
            public string Estado { get; set; } = "Pendiente";

            [Required]
            public List<DetalleNotaDebitoDto> DetalleNotaDebito { get; set; } = new();

            [Required]
            public List<FormaPagoDto> FormasPago { get; set; } = new();
        }

    
}
