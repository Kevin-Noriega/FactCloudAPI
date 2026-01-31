using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Productos;
using FactCloudAPI.Models;
using FactCloudAPI.Models.DTOs;
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
                CantidadDisponible = p.CantidadDisponible,
                Activo = p.Activo
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
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                CodigoInterno = p.CodigoInterno,
                CodigoUNSPSC = p.CodigoUNSPSC,
                UnidadMedida = p.UnidadMedida,
                PrecioUnitario = p.PrecioUnitario,
                Costo = p.Costo,
                TipoImpuesto = p.TipoImpuesto,
                TarifaIVA = p.TarifaIVA,
                ProductoExento = p.ProductoExento,
                ProductoExcluido = p.ProductoExcluido,
                CantidadDisponible = p.CantidadDisponible,
                CantidadMinima = p.CantidadMinima,
                Categoria = p.Categoria,
                Activo = p.Activo
            })
            .FirstOrDefaultAsync();
    }

    public async Task CrearAsync(ProductoCreateDto dto, int usuarioId)
    {
        var producto = new Producto
        {
            UsuarioId = usuarioId,
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            CodigoInterno = dto.CodigoInterno,
            CodigoUNSPSC = dto.CodigoUNSPSC,
            UnidadMedida = dto.UnidadMedida,
            Marca = dto.Marca,
            Modelo = dto.Modelo,
            PrecioUnitario = dto.PrecioUnitario,
            Costo = dto.Costo,
            TipoImpuesto = dto.TipoImpuesto,
            TarifaIVA = dto.TarifaIVA,
            ProductoExento = dto.ProductoExento,
            ProductoExcluido = dto.ProductoExcluido,
            GravaINC = dto.GravaINC,
            TarifaINC = dto.TarifaINC,
            CantidadDisponible = dto.CantidadDisponible,
            CantidadMinima = dto.CantidadMinima,
            Categoria = dto.Categoria,
            CodigoBarras = dto.CodigoBarras
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
