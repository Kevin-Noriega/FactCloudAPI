using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Clientes;
using FactCloudAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FactCloudAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Clientes/{clienteId}/contactos")]
    public class ContactoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContactoController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int UsuarioId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        [HttpGet]
        public async Task<IActionResult> Get(int clienteId)
        {
            // Verificar que el cliente pertenece al usuario
            var existe = await _context.Clientes
                .AnyAsync(c => c.Id == clienteId && c.UsuarioId == UsuarioId);

            if (!existe) return NotFound();

            var contactos = await _context.ContactosCliente
                .Where(c => c.ClienteId == clienteId)
                .Select(c => new { c.Id, c.Nombre, c.Apellido, c.Correo, c.Cargo, c.Telefono })
                .ToListAsync();

            return Ok(contactos);
        }

        [HttpPost]
        public async Task<IActionResult> Post(int clienteId, [FromBody] ContactoDto dto)
        {
            // Verificar que el cliente pertenece al usuario
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == clienteId && c.UsuarioId == UsuarioId);

            if (cliente == null) return NotFound("Cliente no encontrado");

            var contacto = new ContactoCliente
            {
                ClienteId = clienteId,
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Correo = dto.Correo,
                Cargo = dto.Cargo,
                Indicativo = dto.Indicativo,
                Telefono = dto.Telefono,
            };

            _context.ContactosCliente.Add(contacto);
            await _context.SaveChangesAsync();
            return Ok(contacto);
        }
    }
}
