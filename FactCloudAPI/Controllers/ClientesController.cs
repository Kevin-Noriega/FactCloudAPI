using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Clientes;
using FactCloudAPI.Models;
using FactCloudAPI.Services.Clientes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FactCloudAPI.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context, IClienteService clienteService)
        {
            _clienteService = clienteService;
            _context = context;
        }

        private int UsuarioId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        [HttpGet]
        public async Task<IActionResult> GetClientes(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] bool? esProveedor = null,
        [FromQuery] string? search = null)
        {
            var query = _context.Clientes
                .Include(c => c.Usuario)
                .Include(c => c.Contactos)
                .AsQueryable();

           

            // ? Búsqueda global
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c =>
                    EF.Functions.Like(c.Nombre, $"%{search}%") ||
                    EF.Functions.Like(c.NumeroIdentificacion, $"%{search}%") ||
                    EF.Functions.Like(c.Correo, $"%{search}%") ||
                    EF.Functions.Like(c.NombreComercial, $"%{search}%")
                );
            }

            var total = await query.CountAsync();
            var clientes = await query
                .OrderBy(c => c.Nombre)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new {
                    c.Id,
                    c.Nombre,
                    c.NombreComercial,
                    c.NumeroIdentificacion,
                    c.TipoIdentificacion,
                    c.EsProveedor,
                    c.RetenedorIVA,
                    c.RetenedorICA,
                    c.RetenedorRenta,
                    c.AutoretenedorRenta,
                    c.Activo
                })
                .ToListAsync();

            return Ok(new { data = clientes, total, page, pageSize });
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] ClienteUpdateDto dto)
        {
            var result = await _clienteService.ActualizarParcialAsync(id, dto, UsuarioId);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPut("activar/{id}")]
        public async Task<IActionResult> Activar(int id)
        {
            var result = await _clienteService.ActivarAsync(id, UsuarioId);
            if (!result) return NotFound();
            return Ok(new { message = "Cliente activado" });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var cliente = await _clienteService.ObtenerPorIdAsync(id, UsuarioId);
            if (cliente == null) return NotFound();
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ClienteCreateDto dto)
        {
            try
            {
                await _clienteService.CrearAsync(dto, UsuarioId);  // Sin 'var result'
                return Ok(new { mensaje = "Cliente creado exitosamente" });  // O el ID generado
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("DUPLICATE"))
            {
                return Conflict("Ya existe un cliente con esa identificación.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ClienteCreateDto dto)
        {
            await _clienteService.ActualizarAsync(id, dto, UsuarioId);
            return NoContent();
        }



        [HttpPut("desactivar/{id}")]
        public async Task<IActionResult> Desactivar(int id)
        {
            await _clienteService.DesactivarAsync(id, UsuarioId);
            return Ok(new { message = "Cliente desactivado" });
        }

    }

}