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
        await _clienteService.CrearAsync(dto, UsuarioId);
        return Ok(new { message = "Cliente creado correctamente" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, ClienteCreateDto dto)
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
