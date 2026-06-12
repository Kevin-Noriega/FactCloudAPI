using Microsoft.EntityFrameworkCore;
using NubeeAPI.Data;
using NubeeAPI.Data.Seeds;
using NubeeAPI.Models;
using NubeeAPI.Models.Cupones;
using NubeeAPI.Models.Impuestos;
using NubeeAPI.Models.Planes;
using NubeeAPI.Models.Pos;
using NubeeAPI.Models.Sesiones;
using NubeeAPI.Models.Suscripciones;
using NubeeAPI.Models.Usuarios;
using NubeeAPI.Models.Wompi;
using static NubeeAPI.Models.Factura;

namespace NubeeAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // ── DbSets existentes ──────────────────────────────────────────────
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<DetalleFactura> DetalleFacturas { get; set; }
        public DbSet<FacturaFormaPago> FacturasFormasPago { get; set; }
        public DbSet<NotaDebito> NotasDebito { get; set; }
        public DbSet<DetalleNotaDebito> DetalleNotaDebito { get; set; }
        public DbSet<FormaPagoNotaDebito> FormasPagoNotaDebito { get; set; }
        public DbSet<NotaCredito> NotasCredito { get; set; }
        public DbSet<DetalleNotaCredito> DetalleNotaCredito { get; set; }
        public DbSet<FormaPagoNotaCredito> FormasPagoNotaCredito { get; set; }
        public DbSet<DocumentoSoporte> DocumentosSoporte { get; set; }
        public DbSet<SuscripcionFacturacion> SuscripcionesFacturacion { get; set; }
        public DbSet<Addon> Addons { get; set; }
        public DbSet<PlanFacturacion> PlanesFacturacion { get; set; }
        public DbSet<FotoPerfil> FotoPerfils { get; set; }
        public DbSet<Cupon> Cupones { get; set; }
        public DbSet<Negocio> Negocios { get; set; }
        public DbSet<ConfiguracionDian> ConfiguracionesDian { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<HistorialSesion> HistorialSesiones { get; set; }
        public DbSet<RegistroPendiente> RegistrosPendientes { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }
        public DbSet<ContactoCliente> ContactosCliente { get; set; }
        public DbSet<ResolucionDIAN> ResolucionesDIAN { get; set; }
        public DbSet<CertificadoDigital> CertificadosDigitales { get; set; } = null!;
        public DbSet<UsuarioAddon> UsuariosAddons { get; set; }
        public DbSet<RepresentanteLegal> RepresentantesLegales { get; set; }

        // ── DbSets nuevos: PUC / Impuestos / Autoretenciones ──────────────
        public DbSet<CuentaContable> CuentasContables { get; set; }
        public DbSet<Impuesto> Impuestos { get; set; }
        public DbSet<Autoretencion> Autorretenciones { get; set; }
        public DbSet<DetalleFacturaImpuesto> DetalleFacturaImpuestos { get; set; }
        public DbSet<PerfilTributario> PerfilesTributarios { get; set; }
        public DbSet<ImpuestoConcepto> ImpuestosConceptos { get; set; } = null!;
        public DbSet<TarifaImpuesto> TarifasImpuestos { get; set; } = null!;
        public DbSet<ConfiguracionImpuestoEmpresa> ConfiguracionesImpuestoEmpresa { get; set; } = null!;
        public DbSet<MapeoContableTarifa> MapeosContablesTarifa { get; set; } = null!;
        public DbSet<ReglaImpuesto> ReglasImpuesto { get; set; } = null!;
        public DbSet<DocumentoLineaImpuesto> DocumentosLineasImpuesto { get; set; } = null!;
        public DbSet<DocumentoResumenImpuesto> DocumentosResumenImpuesto { get; set; } = null!;

        public DbSet<AuditoriaAdmin> AuditoriaAdmin { get; set; }

        // ── POS ────────────────────────────────────────────────────────────
        public DbSet<TurnoPos> TurnosPos { get; set; }
        public DbSet<MovimientoCajaPos> MovimientosCajaPos { get; set; }
        public DbSet<EtiquetaPos> EtiquetasPos { get; set; }
        public DbSet<ConfiguracionImpresionPos> ConfiguracionesImpresionPos { get; set; }
        public DbSet<PosVenta> PosVentas { get; set; }
        public DbSet<PosVentaDetalle> PosVentaDetalles { get; set; }

        // ── Notificaciones in-app ──────────────────────────────────────────
        public DbSet<Notificacion> Notificaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // ── 🆕 Aplicar configuraciones del motor tributario ──────────────
            modelBuilder.ApplyConfiguration(new ImpuestoConceptoConfiguration());
            modelBuilder.ApplyConfiguration(new TarifaImpuestoConfiguration());
            modelBuilder.ApplyConfiguration(new ConfiguracionImpuestoEmpresaConfiguration());
            modelBuilder.ApplyConfiguration(new MapeoContableTarifaConfiguration());
            modelBuilder.ApplyConfiguration(new ReglaImpuestoConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentoLineaImpuestoConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentoResumenImpuestoConfiguration());

            // ══════════════════════════════════════════════════════════════
            // PRODUCTO
            // ══════════════════════════════════════════════════════════════
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.HasOne(p => p.Usuario)
                    .WithMany(u => u.Productos)
                    .HasForeignKey(p => p.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(p => p.DetalleFacturas)
                    .WithOne(df => df.Producto)
                    .HasForeignKey(df => df.ProductoId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(p => p.Nombre)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(p => p.UnidadMedida)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("Unidad");

                entity.Property(p => p.PrecioUnitario)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(p => p.Costo)
                    .HasColumnType("decimal(18,2)");

                entity.Property(p => p.CantidadDisponible)
                    .HasColumnType("int");

                entity.HasIndex(p => p.UsuarioId);
                entity.HasIndex(p => new { p.UsuarioId, p.Activo });
                entity.HasIndex(p => p.CodigoInterno);
                entity.HasIndex(p => p.CodigoUNSPSC);
            });

            // ══════════════════════════════════════════════════════════════
            // NEGOCIO
            // ══════════════════════════════════════════════════════════════
            modelBuilder.Entity<Negocio>(entity =>
            {
                entity.HasIndex(n => n.Nit)
                      .IsUnique();

                entity.HasIndex(n => n.UsuarioId)
                      .IsUnique();

                entity.Property(n => n.Pais)
                      .HasDefaultValue("CO");

                entity.Property(n => n.DatosFacturacionCompletos)
                      .HasDefaultValue(false);
            });

            // ══════════════════════════════════════════════════════════════
            // PERFIL TRIBUTARIO
            // ══════════════════════════════════════════════════════════════
            modelBuilder.Entity<PerfilTributario>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.HasOne(p => p.Negocio)
                      .WithOne(n => n.PerfilTributario)
                      .HasForeignKey<PerfilTributario>(p => p.NegocioId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(p => p.NegocioId)
                      .IsUnique();

                entity.Property(p => p.RegimenIvaCodigo)
                      .HasMaxLength(20);

                entity.Property(p => p.ActividadEconomicaCIIU)
                      .HasMaxLength(10);

                entity.Property(p => p.TributosJson)
                      .HasColumnType("nvarchar(max)");

                entity.Property(p => p.ResponsabilidadesFiscalesJson)
                      .HasColumnType("nvarchar(max)");
            });

            // ══════════════════════════════════════════════════════════════
            // REPRESENTANTE LEGAL
            // ══════════════════════════════════════════════════════════════
            modelBuilder.Entity<RepresentanteLegal>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.HasOne(r => r.Negocio)
                      .WithOne(n => n.RepresentanteLegal)
                      .HasForeignKey<RepresentanteLegal>(r => r.NegocioId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(r => r.NegocioId)
                      .IsUnique();

                entity.Property(r => r.Nombre)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(r => r.Apellidos)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(r => r.NumeroIdentificacion)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(r => r.CiudadExpedicion)
                      .HasMaxLength(100);

                entity.Property(r => r.CiudadResidencia)
                      .HasMaxLength(100);
            });

            // ══════════════════════════════════════════════════════════════
            // CONFIGURACION DIAN
            // ══════════════════════════════════════════════════════════════
            
            modelBuilder.Entity<ConfiguracionDian>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.HasIndex(c => c.NegocioId)
                      .IsUnique();

                entity.Property(c => c.SoftwareProveedor)
                      .HasMaxLength(100);

                entity.Property(c => c.SoftwarePIN)
                      .HasMaxLength(50);

                entity.Property(c => c.PrefijoAutorizadoDIAN)
                      .HasMaxLength(4);

                entity.Property(c => c.NumeroResolucionDIAN)
                      .HasMaxLength(30);

                entity.Property(c => c.RangoNumeracionDesde)
                      .HasMaxLength(20);

                entity.Property(c => c.RangoNumeracionHasta)
                      .HasMaxLength(20);

                // AmbienteDIAN ahora es int: 1=Producción, 2=Habilitación
                entity.Property(c => c.AmbienteDIAN)
                      .HasDefaultValue(2);
            });

            // ══════════════════════════════════════════════════════════════
            // RESOLUCIÓN DIAN
            // ══════════════════════════════════════════════════════════════
            modelBuilder.Entity<ResolucionDIAN>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.HasOne(r => r.Negocio)
                      .WithMany(n => n.Resoluciones)
                      .HasForeignKey(r => r.NegocioId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(r => r.NegocioId);
                entity.HasIndex(r => new { r.NegocioId, r.Activa });
            });

            // ══════════════════════════════════════════════════════════════
            // REFRESH TOKEN
            // ══════════════════════════════════════════════════════════════
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(rt => rt.Id);

                entity.HasOne(rt => rt.Usuario)
                    .WithMany()
                    .HasForeignKey(rt => rt.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(rt => rt.Token)
                    .IsUnique();

                entity.Property(rt => rt.Token)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(rt => rt.JwtId)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            // ══════════════════════════════════════════════════════════════
            // CLIENTE
            // ══════════════════════════════════════════════════════════════
            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.Clientes)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // ══════════════════════════════════════════════════════════════
            // USUARIO → NEGOCIO / CONFIG DIAN
            // ══════════════════════════════════════════════════════════════
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Negocio)
                .WithOne(n => n.Usuario)
                .HasForeignKey<Negocio>(n => n.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Negocio>()
                .HasOne(n => n.ConfiguracionDIAN)
                .WithOne(c => c.Negocio)
                .HasForeignKey<ConfiguracionDian>(c => c.NegocioId)
                .OnDelete(DeleteBehavior.Cascade);

            // ══════════════════════════════════════════════════════════════
            // FACTURA
            // ══════════════════════════════════════════════════════════════
            modelBuilder.Entity<Factura>(entity =>
            {
                entity.HasKey(f => f.Id);

                entity.HasOne(f => f.Usuario)
                    .WithMany(u => u.Facturas)
                    .HasForeignKey(f => f.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(f => f.Cliente)
                    .WithMany(c => c.Facturas)
                    .HasForeignKey(f => f.ClienteId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(f => f.DetalleFacturas!)
                    .WithOne(df => df.Factura)
                    .HasForeignKey(df => df.FacturaId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(f => f.NotasDebito)
                    .WithOne(nd => nd.Factura)
                    .HasForeignKey(nd => nd.FacturaId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(f => f.NumeroAutorizacion)
                    .IsRequired()
                    .HasMaxLength(14);

                entity.Property(f => f.TipoAmbiente)
                    .IsRequired()
                    .HasDefaultValue(2);

                entity.Property(f => f.TipoFactura)
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasDefaultValue("01");

                entity.Property(f => f.TipoOperacion)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValue("10");

                entity.Property(f => f.ClaveTecnica)
                    .HasMaxLength(200);

                entity.Property(f => f.Prefijo)
                    .HasMaxLength(4);

                entity.Property(f => f.NumeroFactura)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(f => f.HoraEmision)
                    .HasMaxLength(14);

                entity.Property(f => f.Cufe)
                    .HasMaxLength(96);

                entity.Property(f => f.QRCode)
                    .HasMaxLength(150);

                entity.Property(f => f.Subtotal).HasPrecision(18, 2);
                entity.Property(f => f.TotalIVA).HasPrecision(18, 2);
                entity.Property(f => f.TotalINC).HasPrecision(18, 2);
                entity.Property(f => f.TotalICA).HasPrecision(18, 2).HasDefaultValue(0);
                entity.Property(f => f.TotalDescuentos).HasPrecision(18, 2).HasDefaultValue(0);
                entity.Property(f => f.TotalRetenciones).HasPrecision(18, 2).HasDefaultValue(0);
                entity.Property(f => f.TotalFactura).HasPrecision(18, 2);
                entity.Property(f => f.MontoPagado).HasPrecision(18, 2);

                entity.Property(f => f.FormaPago)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValue("1");

                entity.Property(f => f.MedioPago)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValue("10");

                entity.Property(f => f.Estado)
                     .HasConversion<string>()
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue(EstadoFactura.Emitida);

                entity.Property(f => f.RespuestaDIAN)
                    .HasMaxLength(1000);

                entity.Property(f => f.NumeroFactus)
                    .HasMaxLength(40);

                entity.HasMany(f => f.FormasPago)
                    .WithOne(fp => fp.Factura)
                    .HasForeignKey(fp => fp.FacturaId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(f => f.UsuarioId);
                entity.HasIndex(f => new { f.UsuarioId, f.FechaEmision });
                entity.HasIndex(f => new { f.Prefijo, f.NumeroFactura });
                entity.HasIndex(f => f.Cufe);
                entity.HasIndex(f => f.Estado);
                entity.HasIndex(f => f.EnviadaDIAN);
                entity.HasIndex(f => f.FechaLimiteEnvioDIAN);
                entity.HasIndex(f => f.FechaVencimiento);
            });

            // ══════════════════════════════════════════════════════════════
            // DETALLE FACTURA
            // ══════════════════════════════════════════════════════════════
            modelBuilder.Entity<DetalleFactura>(entity =>
            {
                entity.HasKey(d => d.Id);

                entity.HasOne(d => d.Factura)
                    .WithMany(f => f.DetalleFacturas!)
                    .HasForeignKey(d => d.FacturaId)
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

                entity.Property(d => d.Cantidad)
                    .IsRequired()
                    .HasPrecision(12, 6);

                entity.Property(d => d.PrecioUnitario)
                    .IsRequired()
                    .HasPrecision(18, 2);

                entity.Property(d => d.PorcentajeDescuento)
                    .HasPrecision(6, 4)
                    .HasDefaultValue(0);

                entity.Property(d => d.ValorDescuento)
                    .HasPrecision(18, 2);

                entity.Property(d => d.SubtotalLinea)
                    .HasPrecision(18, 2);

                entity.Property(d => d.TotalLinea)
                    .HasPrecision(18, 2);

                entity.Property(d => d.CodigoUNSPSC)
                    .HasMaxLength(10);

                entity.Property(d => d.CodigoInterno)
                    .HasMaxLength(50);

                // Relación con impuestos por línea (nueva)
                entity.HasMany(d => d.Impuestos)
                    .WithOne(i => i.DetalleFactura)
                    .HasForeignKey(i => i.DetalleFacturaId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(d => d.FacturaId);
                entity.HasIndex(d => d.ProductoId);
            });

            // ══════════════════════════════════════════════════════════════
            // NOTA DEBITO
            // ══════════════════════════════════════════════════════════════
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

            // ══════════════════════════════════════════════════════════════
            // DETALLE NOTA DEBITO
            // ══════════════════════════════════════════════════════════════
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

            // ══════════════════════════════════════════════════════════════
            // FORMA PAGO NOTA DEBITO
            // ══════════════════════════════════════════════════════════════
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

            // ══════════════════════════════════════════════════════════════
            // NOTA CREDITO
            // ══════════════════════════════════════════════════════════════
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

            // ══════════════════════════════════════════════════════════════
            // DETALLE NOTA CREDITO
            // ══════════════════════════════════════════════════════════════
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

            // ══════════════════════════════════════════════════════════════
            // FORMA PAGO NOTA CREDITO
            // ══════════════════════════════════════════════════════════════
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

            // ══════════════════════════════════════════════════════════════
            // REGISTRO PENDIENTE
            // ══════════════════════════════════════════════════════════════
            modelBuilder.Entity<RegistroPendiente>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.TransaccionId)
                    .IsUnique()
                    .HasDatabaseName("IX_RegistrosPendientes_TransaccionId");

                entity.HasIndex(e => e.Email)
                    .HasDatabaseName("IX_RegistrosPendientes_Email");

                entity.HasIndex(e => e.Estado)
                    .HasDatabaseName("IX_RegistrosPendientes_Estado");

                entity.Property(e => e.Estado)
                    .HasDefaultValue("PENDING");

                entity.Property(e => e.FechaCreacion)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // ══════════════════════════════════════════════════════════════
            // USUARIO ADDON
            // ══════════════════════════════════════════════════════════════
            modelBuilder.Entity<UsuarioAddon>(entity =>
            {
                entity.HasKey(ua => ua.Id);

                entity.HasOne(ua => ua.Usuario)
                    .WithMany()
                    .HasForeignKey(ua => ua.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ua => ua.Addon)
                    .WithMany()
                    .HasForeignKey(ua => ua.AddonId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(ua => new { ua.UsuarioId, ua.Activo });
                entity.HasIndex(ua => new { ua.UsuarioId, ua.AddonId });
            });

            // ══════════════════════════════════════════════════════════════
            // SUSCRIPCION FACTURACION
            // ══════════════════════════════════════════════════════════════
            modelBuilder.Entity<SuscripcionFacturacion>()
                .HasOne(s => s.Usuario)
                .WithMany(u => u.Suscripciones)
                .HasForeignKey(s => s.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SuscripcionFacturacion>()
                .HasOne(s => s.PlanFacturacion)
                .WithMany(p => p.Suscripciones)
                .HasForeignKey(s => s.PlanFacturacionId)
                .OnDelete(DeleteBehavior.Restrict);

            // ══════════════════════════════════════════════════════════════
            // PLAN FEATURE
            // ══════════════════════════════════════════════════════════════
            modelBuilder.Entity<PlanFeature>()
                .HasOne(f => f.PlanFacturacion)
                .WithMany(p => p.Features)
                .HasForeignKey(f => f.PlanFacturacionId)
                .OnDelete(DeleteBehavior.Cascade);

            // ══════════════════════════════════════════════════════════════
            // FOTO PERFIL
            // ══════════════════════════════════════════════════════════════
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.FotoPerfil)
                .WithOne(fp => fp.Usuario)
                .HasForeignKey<FotoPerfil>("UsuarioId")
                .OnDelete(DeleteBehavior.Cascade);

            // ══════════════════════════════════════════════════════════════
            // HISTORIAL SESION
            // ══════════════════════════════════════════════════════════════
            modelBuilder.Entity<HistorialSesion>()
                .HasOne(h => h.Usuario)
                .WithMany()
                .HasForeignKey(h => h.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // ══════════════════════════════════════════════════════════════
            // ── NUEVO: CUENTA CONTABLE (PUC) ──────────────────────────────
            // ══════════════════════════════════════════════════════════════
            modelBuilder.Entity<CuentaContable>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => new { e.UsuarioId, e.Codigo })
                    .IsUnique()
                    .HasDatabaseName("IX_CuentaContable_Usuario_Codigo");

                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(12);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Naturaleza).IsRequired().HasMaxLength(1).HasDefaultValue("D");
                entity.Property(e => e.TipoAjuste).HasMaxLength(2).HasDefaultValue("N");
                entity.Property(e => e.Activa).HasDefaultValue(true);
                entity.Property(e => e.PermiteMovimiento).HasDefaultValue(true);

                entity.HasOne(e => e.Usuario)
                        .WithMany()
                        .HasForeignKey(e => e.UsuarioId)
                        .IsRequired(false)                    // ← FK opcional
                        .OnDelete(DeleteBehavior.SetNull);

                entity.Ignore(e => e.CodigoNombre);
                entity.Ignore(e => e.NombreClase);
            });

            // ══════════════════════════════════════════════════════════════
            // ── NUEVO: IMPUESTO ───────────────────────────────────────────
            // ══════════════════════════════════════════════════════════════
            modelBuilder.Entity<Impuesto>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => new { e.UsuarioId, e.Codigo })
                    .IsUnique()
                    .HasDatabaseName("IX_Impuesto_Usuario_Codigo");

                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.TipoImpuesto).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Tarifa).HasColumnType("decimal(7,4)");
                entity.Property(e => e.CodigoTributoDIAN).HasMaxLength(2);
                entity.Property(e => e.EnUso).HasDefaultValue(true);

                entity.HasOne(e => e.Usuario)
                     .WithMany()
                     .HasForeignKey(e => e.UsuarioId)
                     .IsRequired(false)
                      .OnDelete(DeleteBehavior.SetNull);
                // NoAction en todas las FK a CuentaContable para evitar
                // múltiples rutas de cascade en SQL Server
                entity.HasOne(e => e.CuentaDebitoVentas)
                    .WithMany(c => c.ImpuestosDebitoVentas)
                    .HasForeignKey(e => e.CuentaDebitoVentasId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.CuentaCreditoVentas)
                    .WithMany(c => c.ImpuestosCreditoVentas)
                    .HasForeignKey(e => e.CuentaCreditoVentasId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.CuentaDebitoCompras)
                    .WithMany(c => c.ImpuestosDebitoCompras)
                    .HasForeignKey(e => e.CuentaDebitoComprasId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.CuentaCreditoCompras)
                    .WithMany(c => c.ImpuestosCreditoCompras)
                    .HasForeignKey(e => e.CuentaCreditoComprasId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.CuentaDevolucionVentas)
                    .WithMany()
                    .HasForeignKey(e => e.CuentaDevolucionVentasId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.CuentaDevolucionCompras)
                    .WithMany()
                    .HasForeignKey(e => e.CuentaDevolucionComprasId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.Ignore(e => e.TarifaDisplay);
            });

            // ══════════════════════════════════════════════════════════════
            // ── NUEVO: AUTORETENCION ──────────────────────────────────────
            // ══════════════════════════════════════════════════════════════
            modelBuilder.Entity<Autoretencion>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => new { e.UsuarioId, e.Codigo })
                    .IsUnique()
                    .HasDatabaseName("IX_Autoretencion_Usuario_Codigo");

                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.TipoAutoretencion).IsRequired().HasMaxLength(30);
                entity.Property(e => e.Tarifa).HasColumnType("decimal(7,4)");
                entity.Property(e => e.BaseMinimaAplicacion).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TipoBase).HasMaxLength(10).HasDefaultValue("Pesos");
                entity.Property(e => e.EnUso).HasDefaultValue(true);

                entity.HasOne(e => e.Usuario)
                      .WithMany()
                      .HasForeignKey(e => e.UsuarioId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.CuentaDebito)
                    .WithMany()
                    .HasForeignKey(e => e.CuentaDebitoId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.CuentaCredito)
                    .WithMany()
                    .HasForeignKey(e => e.CuentaCreditoId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.Ignore(e => e.TarifaDisplay);
            });



            // ══════════════════════════════════════════════════════════════
            // POS — TURNO
            // ══════════════════════════════════════════════════════════════
            modelBuilder.Entity<TurnoPos>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.HasOne(t => t.Usuario)
                    .WithMany()
                    .HasForeignKey(t => t.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(t => t.VendedorNombre).HasMaxLength(200);
                entity.Property(t => t.CerradoPorNombre).HasMaxLength(200);
                entity.Property(t => t.Observaciones).HasMaxLength(500);
                entity.Property(t => t.Estado).HasMaxLength(20).HasDefaultValue("Abierto");

                entity.Property(t => t.BaseInicial).HasPrecision(18, 2);
                entity.Property(t => t.TotalEfectivoReal).HasPrecision(18, 2);
                entity.Property(t => t.TotalTarjeta).HasPrecision(18, 2);
                entity.Property(t => t.TotalPagosLinea).HasPrecision(18, 2);
                entity.Property(t => t.TotalOtros).HasPrecision(18, 2);
                entity.Property(t => t.TotalEsperado).HasPrecision(18, 2);
                entity.Property(t => t.Diferencia).HasPrecision(18, 2);

                entity.HasIndex(t => t.UsuarioId);
                entity.HasIndex(t => new { t.UsuarioId, t.Estado });
            });

            modelBuilder.Entity<MovimientoCajaPos>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.HasOne(m => m.Usuario)
                    .WithMany()
                    .HasForeignKey(m => m.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.Property(m => m.Tipo).HasMaxLength(20);
                entity.Property(m => m.Descripcion).HasMaxLength(300);
                entity.Property(m => m.NumeroComprobante).HasMaxLength(30);
                entity.Property(m => m.Monto).HasPrecision(18, 2);
                entity.HasIndex(m => new { m.UsuarioId, m.Fecha });
            });

            modelBuilder.Entity<EtiquetaPos>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Usuario)
                    .WithMany()
                    .HasForeignKey(e => e.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Activa).HasDefaultValue(true);
                entity.HasIndex(e => e.UsuarioId);
            });

            modelBuilder.Entity<ConfiguracionImpresionPos>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasOne(c => c.Usuario)
                    .WithMany()
                    .HasForeignKey(c => c.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.Property(c => c.MetodoImpresion).HasMaxLength(20).HasDefaultValue("navegador");
                entity.Property(c => c.ImpresoraDefecto).HasMaxLength(150);
                entity.Property(c => c.TamanoPapel).HasMaxLength(30);
                entity.HasIndex(c => c.UsuarioId).IsUnique();
            });

            modelBuilder.Entity<PosVenta>(entity =>
            {
                entity.HasKey(v => v.Id);
                // Restrict en Usuario para evitar múltiples rutas de cascada
                // (Usuario→PosVenta y Usuario→Turno→PosVenta).
                entity.HasOne(v => v.Usuario)
                    .WithMany()
                    .HasForeignKey(v => v.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(v => v.Turno)
                    .WithMany()
                    .HasForeignKey(v => v.TurnoId)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.Property(v => v.ClienteNombre).HasMaxLength(200);
                entity.Property(v => v.Estado).HasMaxLength(20).HasDefaultValue("Registrada");
                foreach (var prop in new[] { "Subtotal", "Impuestos", "Total", "Efectivo", "Tarjeta", "PagosLinea", "Otros", "Credito" })
                    entity.Property(prop).HasPrecision(18, 2);
                entity.HasIndex(v => new { v.UsuarioId, v.Fecha });
                entity.HasIndex(v => v.TurnoId);
            });

            modelBuilder.Entity<PosVentaDetalle>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.HasOne(d => d.PosVenta)
                    .WithMany(v => v.Detalles)
                    .HasForeignKey(d => d.PosVentaId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.Property(d => d.Nombre).HasMaxLength(500);
                entity.Property(d => d.Cantidad).HasPrecision(12, 2);
                entity.Property(d => d.PrecioUnitario).HasPrecision(18, 2);
                entity.Property(d => d.Descuento).HasPrecision(6, 2);
                entity.Property(d => d.TotalLinea).HasPrecision(18, 2);
            });

            // ══════════════════════════════════════════════════════════════
            // AUDITORIA ADMIN
            // ══════════════════════════════════════════════════════════════
            modelBuilder.Entity<AuditoriaAdmin>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.HasOne(a => a.Admin)
                    .WithMany()
                    .HasForeignKey(a => a.AdminId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(a => a.FechaHora);
                entity.HasIndex(a => a.AdminId);
                entity.Property(a => a.FechaHora).HasDefaultValueSql("GETUTCDATE()");
            });

            // ══════════════════════════════════════════════════════════════
            // SEED DATA
            // ══════════════════════════════════════════════════════════════

            // ── Planes de facturación ─────────────────────────────────────
            modelBuilder.Entity<PlanFacturacion>().HasData(
                // ═══ Planes de FACTURACIÓN (suscripción base) ═══════════════
                // El POS NO se incluye aquí: se contrata aparte (planes Tipo="POS").
                new PlanFacturacion
                {
                    Id = 1,
                    Codigo = "STARTER",
                    Nombre = "Starter",
                    Tipo = "FACTURACION",
                    Descripcion = "Ideal para emprendedores iniciando",
                    PrecioAnual = 135000,
                    DescuentoActivo = true,
                    DescuentoPorcentaje = 15,
                    LimiteDocumentosAnuales = 30,
                    LimiteUsuarios = 1,
                    Destacado = false,
                    IncluyePOS = false,
                    IncluyeInventario = false,
                    IncluyeNomina = false,
                    IncluyeContabilidad = false,
                    IncluyeSucursales = false,
                    Activo = true
                },
                new PlanFacturacion
                {
                    Id = 2,
                    Codigo = "BASICO",
                    Nombre = "Básico",
                    Tipo = "FACTURACION",
                    Descripcion = "Para pequeños negocios en crecimiento",
                    PrecioAnual = 300000,
                    DescuentoActivo = true,
                    DescuentoPorcentaje = 10,
                    LimiteDocumentosAnuales = 140,
                    LimiteUsuarios = 2,
                    Destacado = false,
                    IncluyePOS = false,
                    IncluyeInventario = true,
                    IncluyeNomina = false,
                    IncluyeContabilidad = false,
                    IncluyeSucursales = false,
                    Activo = true
                },
                new PlanFacturacion
                {
                    Id = 3,
                    Codigo = "PROFESIONAL",
                    Nombre = "Profesional",
                    Tipo = "FACTURACION",
                    Descripcion = "Perfecto para PYMES establecidas",
                    PrecioAnual = 770000,
                    DescuentoActivo = true,
                    DescuentoPorcentaje = 10,
                    LimiteDocumentosAnuales = 540,
                    LimiteUsuarios = 5,
                    Destacado = true,
                    IncluyePOS = false,
                    IncluyeInventario = true,
                    IncluyeNomina = false,
                    IncluyeContabilidad = false,
                    IncluyeSucursales = false,
                    Activo = true
                },
                new PlanFacturacion
                {
                    Id = 4,
                    Codigo = "EMPRESARIAL",
                    Nombre = "Empresarial",
                    Tipo = "FACTURACION",
                    Descripcion = "Solución completa para empresas grandes",
                    PrecioAnual = 1300000,
                    DescuentoActivo = true,
                    DescuentoPorcentaje = 15,
                    LimiteDocumentosAnuales = 1550,
                    LimiteUsuarios = 15,
                    Destacado = false,
                    IncluyePOS = false,
                    IncluyeInventario = true,
                    IncluyeNomina = true,
                    IncluyeContabilidad = true,
                    IncluyeSucursales = true,
                    Activo = true
                },

                // ═══ Sistema POS (módulo que se vende por separado) ═════════
                // Se contrata sobre cualquier plan de facturación. IncluyePOS=true.
                new PlanFacturacion
                {
                    Id = 5,
                    Codigo = "POS_ESENCIAL",
                    Nombre = "Sistema POS Esencial",
                    Tipo = "POS",
                    Descripcion = "Punto de venta para empezar a vender desde el mostrador",
                    PrecioAnual = 345000,
                    DescuentoActivo = false,
                    DescuentoPorcentaje = null,
                    LimiteDocumentosAnuales = 240,
                    LimiteUsuarios = 1,
                    Destacado = true,
                    IncluyePOS = true,
                    IncluyeInventario = true,
                    IncluyeNomina = false,
                    IncluyeContabilidad = false,
                    IncluyeSucursales = false,
                    Activo = true
                },
                new PlanFacturacion
                {
                    Id = 6,
                    Codigo = "POS_INICIO",
                    Nombre = "Sistema POS Inicio",
                    Tipo = "POS",
                    Descripcion = "Para negocios con mayor volumen de ventas",
                    PrecioAnual = 585900,
                    DescuentoActivo = true,
                    DescuentoPorcentaje = 10,
                    LimiteDocumentosAnuales = null,
                    LimiteUsuarios = 2,
                    Destacado = false,
                    IncluyePOS = true,
                    IncluyeInventario = true,
                    IncluyeNomina = false,
                    IncluyeContabilidad = false,
                    IncluyeSucursales = false,
                    Activo = true
                },
                new PlanFacturacion
                {
                    Id = 7,
                    Codigo = "POS_AVANZADO",
                    Nombre = "Sistema POS Avanzado",
                    Tipo = "POS",
                    Descripcion = "Operación profesional con inventario y reportes avanzados",
                    PrecioAnual = 899900,
                    DescuentoActivo = true,
                    DescuentoPorcentaje = 15,
                    LimiteDocumentosAnuales = null,
                    LimiteUsuarios = 5,
                    Destacado = false,
                    IncluyePOS = true,
                    IncluyeInventario = true,
                    IncluyeNomina = false,
                    IncluyeContabilidad = false,
                    IncluyeSucursales = true,
                    Activo = true
                }
            );
            modelBuilder.Entity<Cupon>(entity =>
            {
                entity.Property(c => c.DescuentoPorcentaje).HasPrecision(5, 2);
            });

            modelBuilder.Entity<CuponUso>(entity =>
            {
                entity.Property(c => c.DescuentoAplicado).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Addon>(entity =>
            {
                entity.Property(a => a.Precio).HasPrecision(18, 2);
            });

            modelBuilder.Entity<PlanFacturacion>(entity =>
            {
                entity.Property(p => p.PrecioAnual).HasPrecision(18, 2);
                // DescuentoPorcentaje es int? → no soporta HasPrecision
            });

            modelBuilder.Entity<PlanFeature>().HasData(
                // ═══ FACTURACIÓN ═══
                // ── Starter (1) ──
                new PlanFeature { Id = 1, PlanFacturacionId = 1, Texto = "1 Usuario", Tooltip = "Cuenta individual para emprendedores que están empezando." },
                new PlanFeature { Id = 2, PlanFacturacionId = 1, Texto = "30 Documentos anuales", Tooltip = "Emite hasta 30 facturas electrónicas al año." },
                new PlanFeature { Id = 3, PlanFacturacionId = 1, Texto = "Facturación electrónica DIAN", Tooltip = "Creación de facturas, gestión de clientes y productos. Reportes simples incluidos." },

                // ── Básico (2): + inventario ──
                new PlanFeature { Id = 4, PlanFacturacionId = 2, Texto = "2 Usuarios", Tooltip = "Hasta dos usuarios para tu negocio." },
                new PlanFeature { Id = 5, PlanFacturacionId = 2, Texto = "140 Documentos electrónicos al año", Tooltip = "Perfecto para negocios que emiten hasta 8 documentos diarios." },
                new PlanFeature { Id = 6, PlanFacturacionId = 2, Texto = "Facturación electrónica DIAN", Tooltip = "Creación de facturas, gestión de clientes, productos, notas débito y crédito." },
                new PlanFeature { Id = 45, PlanFacturacionId = 2, Texto = "Control de inventario", Tooltip = "Administra existencias y stock de tus productos." },

                // ── Profesional (3): + inventario (POS se contrata aparte) ──
                new PlanFeature { Id = 7, PlanFacturacionId = 3, Texto = "5 Usuarios", Tooltip = "Hasta cinco usuarios con acceso al sistema." },
                new PlanFeature { Id = 8, PlanFacturacionId = 3, Texto = "540 Documentos electrónicos al año", Tooltip = "Ideal para PYMES que facturan de forma constante durante todo el año." },
                new PlanFeature { Id = 9, PlanFacturacionId = 3, Texto = "Facturación electrónica DIAN", Tooltip = "Emisión de facturas electrónicas válidas ante la DIAN." },
                new PlanFeature { Id = 10, PlanFacturacionId = 3, Texto = "Notas crédito y débito", Tooltip = "Corrección y ajustes de facturas mediante notas crédito y débito electrónicas." },
                new PlanFeature { Id = 46, PlanFacturacionId = 3, Texto = "Control de inventario", Tooltip = "Descuento automático de stock y control de existencias." },
                new PlanFeature { Id = 11, PlanFacturacionId = 3, Texto = "Gestión avanzada de clientes y productos", Tooltip = "Administra clientes, productos, precios e impuestos de forma organizada." },
                new PlanFeature { Id = 12, PlanFacturacionId = 3, Texto = "Reportes y control de facturación", Tooltip = "Consulta reportes básicos de ventas, documentos emitidos y estado de facturación." },

                // ── Empresarial (4): + inventario, nómina, contabilidad, sucursales ──
                new PlanFeature { Id = 13, PlanFacturacionId = 4, Texto = "15 Usuarios", Tooltip = "Hasta quince usuarios para equipos grandes." },
                new PlanFeature { Id = 14, PlanFacturacionId = 4, Texto = "1550 Documentos electrónicos al año", Tooltip = "Pensado para empresas con alto volumen de facturación anual." },
                new PlanFeature { Id = 15, PlanFacturacionId = 4, Texto = "Facturación electrónica DIAN", Tooltip = "Cumple con todos los requisitos exigidos por la DIAN." },
                new PlanFeature { Id = 16, PlanFacturacionId = 4, Texto = "Notas crédito y débito ilimitadas", Tooltip = "Emite notas crédito y débito sin restricciones dentro del límite anual." },
                new PlanFeature { Id = 47, PlanFacturacionId = 4, Texto = "Control de inventario", Tooltip = "Inventario por bodega y existencias en tiempo real." },
                new PlanFeature { Id = 48, PlanFacturacionId = 4, Texto = "Nómina electrónica", Tooltip = "Liquidación y emisión de nómina electrónica ante la DIAN." },
                new PlanFeature { Id = 49, PlanFacturacionId = 4, Texto = "Contabilidad integrada", Tooltip = "Causación contable automática de tus documentos." },
                new PlanFeature { Id = 50, PlanFacturacionId = 4, Texto = "Multi-sucursal", Tooltip = "Administra varias sucursales desde una sola cuenta." },
                new PlanFeature { Id = 17, PlanFacturacionId = 4, Texto = "Gestión completa de clientes y productos", Tooltip = "Control detallado de clientes, productos, impuestos y precios." },
                new PlanFeature { Id = 18, PlanFacturacionId = 4, Texto = "Reportes administrativos", Tooltip = "Accede a reportes de ventas y facturación para control interno y contable." },
                new PlanFeature { Id = 19, PlanFacturacionId = 4, Texto = "Soporte prioritario", Tooltip = "Atención prioritaria para resolución de dudas y soporte técnico." },

                // ═══ SISTEMA POS (se contrata aparte) ═══
                // ── POS Esencial (5) ──
                new PlanFeature { Id = 20, PlanFacturacionId = 5, Texto = "Cumplimiento normativo DIAN", Tooltip = "Documentos POS válidos ante la DIAN." },
                new PlanFeature { Id = 21, PlanFacturacionId = 5, Texto = "240 Facturas electrónicas anuales desde POS", Tooltip = "Hasta 240 documentos electrónicos al año desde el punto de venta." },
                new PlanFeature { Id = 22, PlanFacturacionId = 5, Texto = "1 Caja registradora", Tooltip = "Una terminal de venta para atender en mostrador." },
                new PlanFeature { Id = 23, PlanFacturacionId = 5, Texto = "Control de inventario", Tooltip = "Descuento automático de stock por cada venta." },
                new PlanFeature { Id = 24, PlanFacturacionId = 5, Texto = "Reportes de ventas diarios", Tooltip = "Total de ventas del día y productos más vendidos." },

                // ── POS Inicio (6) ──
                new PlanFeature { Id = 25, PlanFacturacionId = 6, Texto = "Cumplimiento normativo DIAN", Tooltip = "Documentos POS válidos ante la DIAN." },
                new PlanFeature { Id = 26, PlanFacturacionId = 6, Texto = "Facturas electrónicas desde POS ilimitadas", Tooltip = "Sin límite de documentos electrónicos desde el POS." },
                new PlanFeature { Id = 27, PlanFacturacionId = 6, Texto = "2 Cajas registradoras", Tooltip = "Atiende en dos terminales simultáneas." },
                new PlanFeature { Id = 28, PlanFacturacionId = 6, Texto = "Inventario con alertas de stock mínimo", Tooltip = "Recibe avisos cuando un producto esté por agotarse." },
                new PlanFeature { Id = 29, PlanFacturacionId = 6, Texto = "Cierre de caja por turno", Tooltip = "Controla el dinero por turno con apertura y cierre de caja." },

                // ── POS Avanzado (7) ──
                new PlanFeature { Id = 31, PlanFacturacionId = 7, Texto = "Cumplimiento normativo DIAN", Tooltip = "Documentos POS válidos ante la DIAN." },
                new PlanFeature { Id = 32, PlanFacturacionId = 7, Texto = "Facturas electrónicas desde POS ilimitadas", Tooltip = "Sin límite de documentos electrónicos desde el POS." },
                new PlanFeature { Id = 33, PlanFacturacionId = 7, Texto = "Cajas registradoras ilimitadas", Tooltip = "Sin límite en el número de terminales de venta." },
                new PlanFeature { Id = 34, PlanFacturacionId = 7, Texto = "Inventario avanzado por bodega", Tooltip = "Controla existencias en varias bodegas o ubicaciones." },
                new PlanFeature { Id = 35, PlanFacturacionId = 7, Texto = "Arqueo y reportes avanzados", Tooltip = "Cuadre de caja detallado y reportes exportables." },
                new PlanFeature { Id = 36, PlanFacturacionId = 7, Texto = "Soporte prioritario 24/7", Tooltip = "Atención preferencial todos los días a cualquier hora." }
            );

            // ── Cupones ───────────────────────────────────────────────────
            modelBuilder.Entity<Cupon>().HasData(
                new Cupon { Id = 1, Codigo = "WELCOMEFC", DescuentoPorcentaje = 20, MaxUsos = 30, IsActive = true },
                new Cupon { Id = 2, Codigo = "NUBEE S.A.SPRO", DescuentoPorcentaje = 30, MaxUsos = 30, PlanId = 3, IsActive = true },
                new Cupon { Id = 3, Codigo = "STARTEFC25", DescuentoPorcentaje = 12, MaxUsos = 20, PlanId = 1, IsActive = true }
            );

            // ── Addons ────────────────────────────────────────────────────
            modelBuilder.Entity<Addon>().HasData(
                new Addon { Id = 1, Nombre = "Documentos extra (150)", Descripcion = "Agrega 150 documentos electrónicos adicionales a tu plan actual.", Precio = 45000, Unidad = "año", Tipo = "Capacidad", Color = "#1a73e8", Activo = true },
                new Addon { Id = 2, Nombre = "Documentos extra (500)", Descripcion = "Agrega 500 documentos electrónicos adicionales a tu plan actual.", Precio = 120000, Unidad = "año", Tipo = "Capacidad", Color = "#1a73e8", Activo = true },
                new Addon { Id = 3, Nombre = "Usuario adicional", Descripcion = "Permite añadir un usuario adicional a tu cuenta.", Precio = 80000, Unidad = "año", Tipo = "Usuarios", Color = "#34a853", Activo = true }
            );

            // ── Cuentas PUC semilla (UsuarioId = 0 → plantilla sistema) ──
            SeedData.Seed(modelBuilder);
        }
    }
}
