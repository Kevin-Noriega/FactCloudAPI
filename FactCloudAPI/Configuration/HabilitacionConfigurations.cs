using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NubeeAPI.Models;
using NubeeAPI.Models.Usuarios;

namespace NubeeAPI.Configuration
{
    // ══════════════════════════════════════════════════════════════════
    //  CertificadoDigital — Configuración EF Core
    // ══════════════════════════════════════════════════════════════════
    public class CertificadoDigitalConfiguration : IEntityTypeConfiguration<CertificadoDigital>
    {
        public void Configure(EntityTypeBuilder<CertificadoDigital> b)
        {
            b.ToTable("CertificadosDigitales");
            b.HasKey(c => c.Id);

            b.Property(c => c.RutaCifrada).HasMaxLength(1000);
            b.Property(c => c.PasswordHash).HasMaxLength(500);
            b.Property(c => c.NombreArchivo).HasMaxLength(255);
            b.Property(c => c.VersionCartaAceptada).HasMaxLength(20);

            // Índice único: un negocio tiene un único certificado activo
            b.HasIndex(c => c.NegocioId).IsUnique();

            b.HasOne(c => c.Negocio)
             .WithOne(n => n.CertificadoDigital)
             .HasForeignKey<CertificadoDigital>(c => c.NegocioId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }

    // ══════════════════════════════════════════════════════════════════
    //  ConfiguracionDian — Configuración EF Core
    // ══════════════════════════════════════════════════════════════════
    public class ConfiguracionDianConfiguration : IEntityTypeConfiguration<ConfiguracionDian>
    {
        public void Configure(EntityTypeBuilder<ConfiguracionDian> b)
        {
            b.ToTable("ConfiguracionesDian");
            b.HasKey(c => c.Id);

            b.Property(c => c.SoftwareProveedor).HasMaxLength(20);
            b.Property(c => c.SoftwarePIN).HasMaxLength(100);
            // TestSetId separado de AmbienteDIAN (que es int 1 o 2)
            b.Property(c => c.TestSetId).HasMaxLength(100);
            b.Property(c => c.AmbienteDIAN).HasDefaultValue(2);

            b.HasIndex(c => c.NegocioId).IsUnique();

            b.HasOne(c => c.Negocio)
             .WithOne(n => n.ConfiguracionDIAN)
             .HasForeignKey<ConfiguracionDian>(c => c.NegocioId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }

    // ══════════════════════════════════════════════════════════════════
    //  ResolucionDIAN — Configuración EF Core
    // ══════════════════════════════════════════════════════════════════
    public class ResolucionDIANConfiguration : IEntityTypeConfiguration<ResolucionDIAN>
    {
        public void Configure(EntityTypeBuilder<ResolucionDIAN> b)
        {
            b.ToTable("ResolucionesDIAN");
            b.HasKey(r => r.Id);

            b.Property(r => r.NumeroAutorizacion)
             .IsRequired()
             .HasMaxLength(14);

            b.Property(r => r.Prefijo).HasMaxLength(4);
            b.Property(r => r.ClaveTecnica).HasMaxLength(200);

            // Índice para búsquedas frecuentes
            b.HasIndex(r => new { r.NegocioId, r.Activa });
            b.HasIndex(r => r.NumeroAutorizacion);

            // EstaVigente y DiasRestantes son calculados → no se mapean
            b.Ignore(r => r.EstaVigente);
            b.Ignore(r => r.DiasRestantes);

            b.HasOne(r => r.Negocio)
             .WithMany(n => n.Resoluciones)
             .HasForeignKey(r => r.NegocioId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
