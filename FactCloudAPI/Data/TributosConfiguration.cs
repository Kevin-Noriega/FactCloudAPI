using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NubeeAPI.Models.Impuestos;

namespace NubeeAPI.Data
{
    /// <summary>
    /// Configuraciones fluent API para entidades tributarias.
    /// Centraliza constraints, índices, relaciones y conversiones de tipos.
    /// </summary>

    // ════════════════════════════════════════════════════════════════

    public class ImpuestoConceptoConfiguration : IEntityTypeConfiguration<ImpuestoConcepto>
    {
        public void Configure(EntityTypeBuilder<ImpuestoConcepto> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CodigoInterno)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Nombre)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.CodigoTributoDIAN)
                .HasMaxLength(2);

            builder.HasIndex(x => new { x.EmpresaId, x.CodigoInterno })
                .IsUnique()
                .HasDatabaseName("IX_ImpuestoConcepto_Empresa_Codigo");

            builder.HasMany(x => x.Tarifas)
                .WithOne(x => x.ImpuestoConcepto)
                .HasForeignKey(x => x.ImpuestoConceptoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    // ════════════════════════════════════════════════════════════════

    public class TarifaImpuestoConfiguration : IEntityTypeConfiguration<TarifaImpuesto>
    {
        public void Configure(EntityTypeBuilder<TarifaImpuesto> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Tarifa)
                .IsRequired()
                .HasPrecision(18, 4);

            builder.Property(x => x.BaseMinima)
                .HasPrecision(18, 2);

            builder.Property(x => x.UnidadBaseMinima)
                .HasMaxLength(10);

            builder.HasIndex(x => new { x.ImpuestoConceptoId, x.Nombre })
                .IsUnique()
                .HasDatabaseName("IX_TarifaImpuesto_Concepto_Nombre");

            builder.HasMany(x => x.MapeosContables)
                .WithOne(x => x.TarifaImpuesto)
                .HasForeignKey(x => x.TarifaImpuestoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Reglas)
                .WithOne(x => x.TarifaImpuesto)
                .HasForeignKey(x => x.TarifaImpuestoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Configuraciones)
                .WithOne(x => x.TarifaImpuesto)
                .HasForeignKey(x => x.TarifaImpuestoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    // ════════════════════════════════════════════════════════════════

    public class ConfiguracionImpuestoEmpresaConfiguration : IEntityTypeConfiguration<ConfiguracionImpuestoEmpresa>
    {
        public void Configure(EntityTypeBuilder<ConfiguracionImpuestoEmpresa> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Observaciones)
                .HasMaxLength(500);

            builder.HasIndex(x => new { x.EmpresaId, x.TarifaImpuestoId })
                .IsUnique()
                .HasDatabaseName("IX_ConfiguracionImpuesto_Empresa_Tarifa");

            builder.HasOne(x => x.TarifaImpuesto)
                .WithMany(x => x.Configuraciones)
                .HasForeignKey(x => x.TarifaImpuestoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    // ════════════════════════════════════════════════════════════════

    public class MapeoContableTarifaConfiguration : IEntityTypeConfiguration<MapeoContableTarifa>
    {
        public void Configure(EntityTypeBuilder<MapeoContableTarifa> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => new { x.TarifaImpuestoId, x.Contexto, x.RolCuenta })
                .IsUnique()
                .HasDatabaseName("IX_MapeoContable_Tarifa_Contexto_Rol");

            builder.HasOne(x => x.TarifaImpuesto)
                .WithMany(x => x.MapeosContables)
                .HasForeignKey(x => x.TarifaImpuestoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.CuentaContable)
                .WithMany()
                .HasForeignKey(x => x.CuentaContableId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

    // ════════════════════════════════════════════════════════════════

    public class ReglaImpuestoConfiguration : IEntityTypeConfiguration<ReglaImpuesto>
    {
        public void Configure(EntityTypeBuilder<ReglaImpuesto> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Descripcion)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.CondicionJSON)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.Property(x => x.Accion)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Ignore(x => x.TarifaAlternativa);

            builder.HasIndex(x => new { x.TarifaImpuestoId, x.Activa })
                .HasDatabaseName("IX_ReglaImpuesto_Tarifa_Activa");

            builder.HasOne(x => x.TarifaImpuesto)
                .WithMany(x => x.Reglas)
                .HasForeignKey(x => x.TarifaImpuestoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
    // ════════════════════════════════════════════════════════════════

    public class DocumentoLineaImpuestoConfiguration : IEntityTypeConfiguration<DocumentoLineaImpuesto>
    {
        public void Configure(EntityTypeBuilder<DocumentoLineaImpuesto> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.BaseGravable)
                .HasPrecision(18, 2);

            builder.Property(x => x.TarifaUtilizada)
                .HasPrecision(18, 4);

            builder.Property(x => x.ValorCalculado)
                .HasPrecision(18, 2);

            builder.Property(x => x.ReglaAplicada)
                .HasMaxLength(200);

            builder.HasIndex(x => new { x.DetalleFacturaId, x.TarifaImpuestoId })
                .HasDatabaseName("IX_DocumentoLineaImpuesto_Detalle_Tarifa");

            builder.HasOne(x => x.DetalleFactura)
                .WithMany(x => x.Impuestos)
                .HasForeignKey(x => x.DetalleFacturaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.TarifaImpuesto)
                .WithMany()
                .HasForeignKey(x => x.TarifaImpuestoId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

    // ════════════════════════════════════════════════════════════════

    public class DocumentoResumenImpuestoConfiguration : IEntityTypeConfiguration<DocumentoResumenImpuesto>
    {
        public void Configure(EntityTypeBuilder<DocumentoResumenImpuesto> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.BaseTotal)
                .HasPrecision(18, 2);

            builder.Property(x => x.TasaAplicada)
                .HasPrecision(18, 4);

            builder.Property(x => x.ValorTotal)
                .HasPrecision(18, 2);

            builder.HasIndex(x => new { x.FacturaId, x.TarifaImpuestoId })
                .IsUnique()
                .HasDatabaseName("IX_DocumentoResumenImpuesto_Factura_Tarifa");

            builder.HasOne(x => x.TarifaImpuesto)
                .WithMany()
                .HasForeignKey(x => x.TarifaImpuestoId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}