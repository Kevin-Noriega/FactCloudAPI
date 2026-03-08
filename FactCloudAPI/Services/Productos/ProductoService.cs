using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Productos;
using FactCloudAPI.Models;
using FactCloudAPI.Services.Productos;
using Microsoft.EntityFrameworkCore;

public class ProductoService : IProductoService
{
    private readonly ApplicationDbContext _context;

    public ProductoService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductoDto>> ObtenerAsync(int usuarioId)
    {
        return await _context.Productos
            .Where(p => p.UsuarioId == usuarioId && p.Activo)
            .Select(p => new ProductoDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                UnidadMedida = p.UnidadMedida,
                PrecioUnitario = p.PrecioUnitario,
                CantidadDisponible = p.CantidadDisponible,  // ← Ya es nullable
                Activo = p.Activo,
                EsServicio = p.EsServicio                   // ← Agrega
            })
            .ToListAsync();
    }

    public async Task<ProductoDetalleDto?> ObtenerPorIdAsync(int id, int usuarioId)
    {
        return await _context.Productos
            .Where(p => p.Id == id && p.UsuarioId == usuarioId)
            .Select(p => new ProductoDetalleDto
            {
                Id = p.Id,
                EsServicio = p.EsServicio,                  // ← Agrega
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                CodigoInterno = p.CodigoInterno,
                CodigoUNSPSC = p.CodigoUNSPSC,
                UnidadMedida = p.UnidadMedida,
                PrecioUnitario = p.PrecioUnitario,
                Costo = p.Costo,
                ImpuestoCargo = p.ImpuestoCargo,            // ← Nuevo
                Retencion = p.Retencion,                    // ← Nuevo
                CantidadDisponible = p.CantidadDisponible,  // ← Nullable
                CantidadMinima = p.CantidadMinima,
                Categoria = p.Categoria,
                IncluyeIVA = p.IncluyeIVA,                  // ← Nuevo
                Activo = p.Activo
            })
            .FirstOrDefaultAsync();
    }


    public async Task CrearAsync(ProductoCreateDto dto, int usuarioId)
    {
        var producto = new Producto
        {
            UsuarioId = usuarioId,
            EsServicio = dto.EsServicio,
            Nombre = dto.Nombre?.Trim() ?? "",
            Descripcion = dto.Descripcion?.Trim(),
            CodigoInterno = dto.CodigoInterno?.Trim(),
            CodigoUNSPSC = dto.CodigoUNSPSC?.Trim(),
            UnidadMedida = dto.UnidadMedida ?? "Unidad",
            Marca = dto.Marca?.Trim(),
            Modelo = dto.Modelo?.Trim(),
            Categoria = dto.Categoria?.Trim(),
            CodigoBarras = dto.CodigoBarras?.Trim(),
            PrecioUnitario = dto.PrecioUnitario,
            Costo = dto.Costo,
            IncluyeIVA = dto.IncluyeIVA,
            ImpuestoCargo = dto.ImpuestoCargo?.Trim(),
            Retencion = dto.Retencion?.Trim(),
            CantidadDisponible = dto.EsServicio ? null : dto.CantidadDisponible,
            CantidadMinima = dto.CantidadMinima,
            TipoProducto = dto.TipoProducto?.Trim(),
            Activo = true,
            FechaRegistro = DateTime.Now
        };

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();
    }


    public async Task ActualizarAsync(int id, ProductoUpdateDto dto, int usuarioId)
    {
        var producto = await _context.Productos
            .FirstOrDefaultAsync(p => p.Id == id && p.UsuarioId == usuarioId);

        if (producto == null)
            throw new KeyNotFoundException("Producto no encontrado");

        producto.Nombre = dto.Nombre;
        producto.Descripcion = dto.Descripcion;
        producto.PrecioUnitario = dto.PrecioUnitario;
        producto.Costo = dto.Costo;
        producto.CantidadDisponible = dto.CantidadDisponible;
        producto.Categoria = dto.Categoria;

        await _context.SaveChangesAsync();
    }

    public async Task DesactivarAsync(int id, int usuarioId)
    {
        var producto = await _context.Productos
            .FirstOrDefaultAsync(p => p.Id == id && p.UsuarioId == usuarioId);

        if (producto == null)
            throw new KeyNotFoundException("Producto no encontrado");

        producto.Activo = false;
        await _context.SaveChangesAsync();
    }
}
