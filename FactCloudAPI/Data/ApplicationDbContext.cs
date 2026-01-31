using FactCloudAPI.Models;
using FactCloudAPI.Models.NotasDebito;
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
            // NOTA DÉBITO -> USUARIO (1:N)
            // ===============================
            modelBuilder.Entity<NotaDebito>()
                .HasOne(nd => nd.Usuario)
                .WithMany(u => u.NotasDebito)
                .HasForeignKey(nd => nd.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===============================
            // NOTA DÉBITO -> CLIENTE (1:N)
            // ===============================
            modelBuilder.Entity<NotaDebito>()
                .HasOne(nd => nd.Cliente)
                .WithMany(c => c.NotasDebito)
                .HasForeignKey(nd => nd.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===============================
            // NOTA DÉBITO -> FACTURA (1:N)
            // ===============================
            modelBuilder.Entity<NotaDebito>()
                .HasOne(nd => nd.Factura)
                .WithMany(f => f.NotasDebito)
                .HasForeignKey(nd => nd.FacturaId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
