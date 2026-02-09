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
        public DbSet<DocumentoSoporte> DocumentosSoporte { get; set; }
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
                     Descripcion = "Ideal para emprendedores iniciando",
                     PrecioAnual = 135000,
                     DescuentoActivo = true,
                     DescuentoPorcentaje = 15,
                     LimiteDocumentosAnuales = 30,
                     LimiteUsuarios = 1,
                     Destacado = false,
                     Activo = true
                 },
                  new PlanFacturacion
                  {
                      Id = 2,
                      Codigo = "BASICO",
                      Nombre = "Básico",
                      Descripcion = "Para pequeños negocios en crecimiento",
                      PrecioAnual = 300000,
                      DescuentoActivo = true,
                      DescuentoPorcentaje = 10,
                      LimiteDocumentosAnuales = 140,
                      LimiteUsuarios = 1,
                      Destacado = false,
                      Activo = true
                  },
                  new PlanFacturacion
                  {
                      Id = 3,
                      Codigo = "PROFESIONAL",
                      Nombre = "Profesional",
                      Descripcion = "Perfecto para PYMES establecidas",
                      PrecioAnual = 770000,
                      DescuentoActivo = true,
                      DescuentoPorcentaje = 10,
                      LimiteDocumentosAnuales = 540,
                      LimiteUsuarios = 1,
                      Destacado = false,
                      Activo = true
                  },
                   new PlanFacturacion
                   {
                       Id = 4,
                       Codigo = "EMPRESARIAL",
                       Nombre = "Empresarial",
                       Descripcion = "Solución completa para empresas grandes",
                       PrecioAnual = 1300000,
                       DescuentoActivo = true,
                       DescuentoPorcentaje = 15,
                       LimiteDocumentosAnuales = 1550,
                       LimiteUsuarios = 1,
                       Destacado = false,
                       Activo = true
                   }
                    
            ); 
            modelBuilder.Entity<PlanFeature>().HasData(

                    // STARTER
                    new PlanFeature
                    {
                        Id = 1,
                        PlanFacturacionId = 1,
                        Texto = "1 Usuario",
                        Tooltip = "Cuenta individual para emprendedores que están empezando."
                    },
                    new PlanFeature
                    {
                        Id = 2,
                        PlanFacturacionId = 1,
                        Texto = "30 Documentos anuales",
                        Tooltip = "Emite hasta 30 facturas electrónicas al año."
                    },
                    new PlanFeature
                    {
                        Id = 3,
                        PlanFacturacionId = 1,
                        Texto = "Funciones básicas",
                        Tooltip = "Creación de facturas, gestión de clientes y productos. Reportes simples incluidos."
                    },
                    new PlanFeature
                    {
                        Id = 4,
                        PlanFacturacionId = 2,
                        Texto = "1 Usuario",
                        Tooltip = "Cuenta individual perfecta para emprendedores y negocios unipersonales.Acceso completo a todas las funciones."
                    },

                    new PlanFeature
                    {
                        Id = 5,
                        PlanFacturacionId = 2,
                        Texto = "140 Documentos electrónicos al año",
                        Tooltip = "Perfecto para negocios que emiten hasta 8 documentos diarios."
                    },
                    new PlanFeature
                    {
                        Id = 6,
                        PlanFacturacionId = 2,
                        Texto = "Funciones básicas",
                        Tooltip = "Creación de facturas, gestión de clientes, productos, notas débito y crédito. Reportes simples incluidos."
                    },
                    new PlanFeature
                    {
                        Id = 7,
                        PlanFacturacionId = 3,
                        Texto = "1 Usuario",
                        Tooltip = "Cuenta individual con acceso completo a todas las funcionalidades del sistema."
                    },
                    new PlanFeature
                    {
                        Id = 8,
                        PlanFacturacionId = 3,
                        Texto = "540 Documentos electrónicos al año",
                        Tooltip = "Ideal para PYMES que facturan de forma constante durante todo el año."
                    },
                    new PlanFeature
                    {
                        Id = 9,
                        PlanFacturacionId = 3,
                        Texto = "Facturación electrónica DIAN",
                        Tooltip = "Emisión de facturas electrónicas válidas ante la DIAN, cumpliendo la normativa vigente."
                    },
                    new PlanFeature
                    {
                        Id = 10,
                        PlanFacturacionId = 3,
                        Texto = "Notas crédito y débito",
                        Tooltip = "Corrección y ajustes de facturas mediante notas crédito y débito electrónicas."
                    },
                    new PlanFeature
                    {
                        Id = 11,
                        PlanFacturacionId = 3,
                        Texto = "Gestión avanzada de clientes y productos",
                        Tooltip = "Administra clientes, productos, precios e impuestos de forma organizada."
                    },
                    new PlanFeature
                    {
                        Id = 12,
                        PlanFacturacionId = 3,
                        Texto = "Reportes y control de facturación",
                        Tooltip = "Consulta reportes básicos de ventas, documentos emitidos y estado de facturación."
                    },
                    new PlanFeature
                    {
                        Id = 13,
                        PlanFacturacionId = 4,
                        Texto = "1 Usuario",
                        Tooltip = "Acceso completo al sistema con control total de la facturación empresarial."
                    },
                    new PlanFeature
                    {
                        Id = 14,
                        PlanFacturacionId = 4,
                        Texto = "1550 Documentos electrónicos al año",
                        Tooltip = "Pensado para empresas con alto volumen de facturación anual."
                    },
                    new PlanFeature
                    {
                        Id = 15,
                        PlanFacturacionId = 4,
                        Texto = "Facturación electrónica DIAN",
                        Tooltip = "Cumple con todos los requisitos exigidos por la DIAN para facturación electrónica."
                    },
                    new PlanFeature
                    {
                        Id = 16,
                        PlanFacturacionId = 4,
                        Texto = "Notas crédito y débito ilimitadas",
                        Tooltip = "Emite notas crédito y débito sin restricciones dentro del límite anual de documentos."
                    },
                    new PlanFeature
                    {
                        Id = 17,
                        PlanFacturacionId = 4,
                        Texto = "Gestión completa de clientes y productos",
                        Tooltip = "Control detallado de clientes, productos, impuestos y precios."
                    },
                    new PlanFeature
                    {
                        Id = 18,
                        PlanFacturacionId = 4,
                        Texto = "Reportes administrativos",
                        Tooltip = "Accede a reportes de ventas y facturación para control interno y contable."
                    },
                    new PlanFeature
                    {
                        Id = 19,
                        PlanFacturacionId = 4,
                        Texto = "Soporte prioritario",
                        Tooltip = "Atención prioritaria para resolución de dudas y soporte técnico."
                    }


             );

            modelBuilder.Entity<Factura>()
        .Property(f => f.MontoPagado)
        .HasPrecision(18, 2);

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
                    .WithMany(nd => nd.Detalles)
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
