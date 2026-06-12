using NubeeAPI.Data;
using NubeeAPI.DTOs.Productos;
using NubeeAPI.Models;
using NubeeAPI.Services.Notificaciones;
using NubeeAPI.Services.Productos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NubeeAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductoService _productoService;
        private readonly INotificacionService _notificaciones;

        public ProductosController(
            ApplicationDbContext context,
            IProductoService productoService,
            INotificacionService notificaciones)
        {
            _context = context;
            _productoService = productoService;
            _notificaciones = notificaciones;
        }

        // GET: api/productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var productos = await _context.Productos
                .Where(p => p.UsuarioId == usuarioId) // ? sin filtro de Activo
                .ToListAsync();

            return Ok(productos);
        }

        // GET: api/productos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound();

            return Ok(producto);
        }

        // POST: api/productos
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto([FromBody] ProductoCreateDto dto)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _productoService.CrearAsync(dto, usuarioId);

                await _notificaciones.CrearAsync(usuarioId, "success", "Producto creado",
                    $"El producto '{dto.Nombre}' se creó correctamente.", "producto", enlace: "/productos");

                return Ok(new { message = "Producto creado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/productos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, [FromBody] ProductoUpdateDto dto)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _productoService.ActualizarAsync(id, dto, usuarioId);

                await _notificaciones.CrearAsync(usuarioId, "info", "Producto actualizado",
                    $"El producto '{dto.Nombre}' se actualizó.", "producto", id, "/productos");

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("desactivar/{id}")]
        public async Task<IActionResult> DesactivarProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return NotFound();

            producto.Activo = false;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Producto desactivado correctamente" });
        }

        // DELETE: api/productos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound();

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            await _notificaciones.CrearAsync(producto.UsuarioId, "warning", "Producto eliminado",
                $"El producto '{producto.Nombre}' fue eliminado.", "producto");

            return NoContent();
        }
    }
}
