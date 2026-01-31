using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Usuarios;
using FactCloudAPI.Models;
using FactCloudAPI.Services.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace FactCloudAPI.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuariosController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Ok(await _service.GetMeAsync(userId));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(CreateUsuarioDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return Ok(new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateUsuarioDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }



}



