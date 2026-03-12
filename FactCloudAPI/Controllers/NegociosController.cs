using FactCloudAPI.Data;
using FactCloudAPI.Models.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace FactCloudAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NegociosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NegociosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Negocios/mio
        [HttpGet("mio")]
        public async Task<ActionResult<Negocio>> GetMiNegocio()
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var negocio = await _context.Negocios
                .FirstOrDefaultAsync(n => n.UsuarioId == usuarioId);

            if (negocio == null)
                return NotFound("El usuario no tiene negocio registrado.");

            return Ok(negocio);
        }
    }
}
