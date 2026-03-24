using FactCloudAPI.DTOs.Clientes;
using FactCloudAPI.Models;
using FactCloudAPI.Services.Clientes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;

    public ClientesController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    private int UsuarioId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
    private int GetCurrentUserId()
    {
        // Opción A: Si usas JWT/Claims
        var userIdClaim = User.FindFirst("userId")?.Value
                       ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(userIdClaim, out int userId))
            return userId;

        // Opción B: Si usas HttpContext.User.Identity
        // var userId = int.Parse(User.Identity.Name); // Ajusta según tu auth

        throw new UnauthorizedAccessException("Usuario no autenticado");
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var clientes = await _clienteService.ObtenerClientesAsync(UsuarioId);
        return Ok(clientes);
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
            await _clienteService.CrearAsync(dto, GetCurrentUserId());  // Sin 'var result'
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
