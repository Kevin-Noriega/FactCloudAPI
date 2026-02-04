using FactCloudAPI.Models;
using FactCloudAPI.Models.Planes;
using FactCloudAPI.Models.Suscripciones;
using FactCloudAPI.Models.Usuarios;
using Microsoft.EntityFrameworkCore;

namespace FactCloudAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<DetalleFactura> DetalleFacturas { get; set; }
        public DbSet<NotaDebito> NotasDebito { get; set; }
        public DbSet<DetalleNotaDebito> DetalleNotaDebito { get; set; }
        public DbSet<FormaPagoNotaDebito> FormasPagoNotaDebito { get; set; }
        public DbSet<NotaCredito> NotasCredito { get; set; }
        public DbSet<DetalleNotaCredito> DetalleNotaCredito { get; set; }
        public DbSet<FormaPagoNotaCredito> FormasPagoNotaCredito { get; set; }
        public DbSet<SuscripcionFacturacion> SuscripcionesFacturacion { get; set; }
        public DbSet<PlanFacturacion> PlanesFacturacion { get; set; }
        public DbSet<Negocio> Negocios { get; set; }
        public DbSet<ConfiguracionDian> ConfiguracionesDian { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===============================
            // CLIENTE -> USUARIO (1:N)
            // ===============================
            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.Clientes)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
            // Usuario → Negocio (1 a 1)
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Negocio)
                .WithOne(n => n.Usuario)
                .HasForeignKey<Negocio>(n => n.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Negocio → ConfiguracionDian (1 a 1)
            modelBuilder.Entity<Negocio>()
                .HasOne(n => n.ConfiguracionDIAN)
                .WithOne(c => c.Negocio)
                .HasForeignKey<ConfiguracionDian>(c => c.NegocioId)
                .OnDelete(DeleteBehavior.Cascade);

            // ===============================
            // PRODUCTO -> USUARIO (1:N)
            // ===============================
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Productos)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===============================
            // FACTURA -> USUARIO (1:N)
            // ===============================
            modelBuilder.Entity<Factura>()
                .HasOne(f => f.Usuario)
                .WithMany(u => u.Facturas)
                .HasForeignKey(f => f.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===============================
            // FACTURA -> CLIENTE (1:N)
            // ===============================
            modelBuilder.Entity<Factura>()
                .HasOne(f => f.Cliente)
                .WithMany(c => c.Facturas)
                .HasForeignKey(f => f.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===============================
            // DETALLE FACTURA -> FACTURA (1:N, CASCADE)
            // ===============================
            modelBuilder.Entity<DetalleFactura>()
                .HasOne(d => d.Factura)
                .WithMany(f => f.DetalleFacturas)
                .HasForeignKey(d => d.FacturaId)
                .OnDelete(DeleteBehavior.Cascade);

            // ===============================
            // DETALLE FACTURA -> PRODUCTO (1:N)
            // ===============================
            modelBuilder.Entity<DetalleFactura>()
                .HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);
            // ===============================
            // SUSCRIPCION FACTURACION -> USUARIO (1:N)
            modelBuilder.Entity<SuscripcionFacturacion>()
               .HasOne(s => s.Usuario)
               .WithMany(u => u.Suscripciones)
                .HasForeignKey(s => s.UsuarioId);

            modelBuilder.Entity<SuscripcionFacturacion>()
                .HasOne(s => s.PlanFacturacion)
                .WithMany(p => p.Suscripciones)
                .HasForeignKey(s => s.PlanFacturacionId);
            modelBuilder.Entity<PlanFacturacion>().HasData(
                 new PlanFacturacion
                 {
                     Id = 1,
                     Codigo = "STARTER",
                     Nombre = "Starter",
                     PrecioMensual = 5900,
                     PrecioAnual = 70800,
                     LimiteDocumentosMensual = 100,
                     LimiteUsuarios = 1,
                     Activo = true
                 },
                  new PlanFacturacion
                  {
                      Id = 2,
                      Codigo = "PAY_PER_USE",
                      Nombre = "Pago por Uso",
                      PrecioMensual = 0,
                      PrecioAnual = 0,
                      LimiteDocumentosMensual = null,
                      LimiteUsuarios = null,
                      Activo = true
                  }
            );

            // NotaDebito
            modelBuilder.Entity<NotaDebito>(entity =>
            {
                entity.HasKey(nd => nd.Id);

                entity.HasOne(nd => nd.Usuario)
                    .WithMany()
                    .HasForeignKey(nd => nd.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(nd => nd.Factura)
                    .WithMany()
                    .HasForeignKey(nd => nd.FacturaId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(nd => nd.Cliente)
                    .WithMany()
                    .HasForeignKey(nd => nd.ClienteId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.Property(nd => nd.NumeroNota)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(nd => nd.Estado)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValue("Pendiente");

                entity.HasIndex(nd => nd.NumeroNota);
                entity.HasIndex(nd => nd.FechaElaboracion);
                entity.HasIndex(nd => nd.Estado);
            });

            // DetalleNotaDebito
            modelBuilder.Entity<DetalleNotaDebito>(entity =>
            {
                entity.HasKey(d => d.Id);

                entity.HasOne(d => d.NotaDebito)
                    .WithMany(nd => nd.DetalleNotaDebito)
                    .HasForeignKey(d => d.NotaDebitoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Producto)
                    .WithMany()
                    .HasForeignKey(d => d.ProductoId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(d => d.Descripcion)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(d => d.UnidadMedida)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("Unidad");
            });

            // FormaPagoNotaDebito
            modelBuilder.Entity<FormaPagoNotaDebito>(entity =>
            {
                entity.HasKey(fp => fp.Id);

                entity.HasOne(fp => fp.NotaDebito)
                    .WithMany(nd => nd.FormasPago)
                    .HasForeignKey(fp => fp.NotaDebitoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(fp => fp.Metodo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("Efectivo");
            });
            // NotaCredito
            modelBuilder.Entity<NotaCredito>(entity =>
            {
                entity.HasKey(nc => nc.Id);

                entity.HasOne(nc => nc.Usuario)
                    .WithMany()
                    .HasForeignKey(nc => nc.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(nc => nc.Factura)
                    .WithMany()
                    .HasForeignKey(nc => nc.FacturaId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(nc => nc.Cliente)
                    .WithMany()
                    .HasForeignKey(nc => nc.ClienteId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.Property(nc => nc.NumeroNota)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(nc => nc.Tipo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValue("devolucion");

                entity.Property(nc => nc.Estado)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValue("Pendiente");

                entity.HasIndex(nc => nc.NumeroNota);
                entity.HasIndex(nc => nc.FechaElaboracion);
                entity.HasIndex(nc => nc.Estado);
                entity.HasIndex(nc => nc.Tipo);
            });

            // DetalleNotaCredito
            modelBuilder.Entity<DetalleNotaCredito>(entity =>
            {
                entity.HasKey(d => d.Id);

                entity.HasOne(d => d.NotaCredito)
                    .WithMany(nc => nc.DetalleNotaCredito)
                    .HasForeignKey(d => d.NotaCreditoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Producto)
                    .WithMany()
                    .HasForeignKey(d => d.ProductoId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(d => d.Descripcion)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(d => d.UnidadMedida)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("Unidad");
            });

            // FormaPagoNotaCredito
            modelBuilder.Entity<FormaPagoNotaCredito>(entity =>
            {
                entity.HasKey(fp => fp.Id);

                entity.HasOne(fp => fp.NotaCredito)
                    .WithMany(nc => nc.FormasPago)
                    .HasForeignKey(fp => fp.NotaCreditoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(fp => fp.Metodo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("Efectivo");
            });
        }

    }
}
