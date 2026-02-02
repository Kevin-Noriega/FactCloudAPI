using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Clientes;
using FactCloudAPI.Models;
using FactCloudAPI.Models.DTOs;
using FactCloudAPI.Services.Clientes;
using Microsoft.EntityFrameworkCore;

public class ClienteService : IClienteService
{
    private readonly ApplicationDbContext _context;

    public ClienteService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ClienteDetalleDto>> ObtenerClientesAsync(int usuarioId)
    {
        return await _context.Clientes
            .Where(c => c.UsuarioId == usuarioId && c.Activo)
            .Select(c => new ClienteDetalleDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                NombreComercial = c.NombreComercial,
                Correo = c.Correo,
                Activo = c.Activo
            })
            .ToListAsync();
    }

    public async Task<ClienteDetalleDto?> ObtenerPorIdAsync(int id, int usuarioId)
    {
        return await _context.Clientes
            .Where(c => c.Id == id && c.UsuarioId == usuarioId)
            .Select(c => new ClienteDetalleDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                NombreComercial = c.NombreComercial,
                Correo = c.Correo,
                Activo = c.Activo
            })
            .FirstOrDefaultAsync();
    }

    public async Task CrearAsync(ClienteCreateDto dto, int usuarioId)
    {
        var cliente = new Cliente
        {
            UsuarioId = usuarioId,
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            NombreComercial = dto.NombreComercial,
            TipoIdentificacion = dto.TipoIdentificacion,
            NumeroIdentificacion = dto.NumeroIdentificacion,
            DigitoVerificacion = dto.DigitoVerificacion,
            TipoPersona = dto.TipoPersona,
            RegimenTributario = dto.RegimenTributario,
            Correo = dto.Correo,
            Telefono = dto.Telefono,
            Departamento = dto.Departamento,
            Ciudad = dto.Ciudad,
            Direccion = dto.Direccion,
            CodigoPostal = dto.CodigoPostal
        };

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(int id, ClienteCreateDto dto, int usuarioId)
    {
        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == usuarioId);

        if (cliente == null)
            throw new KeyNotFoundException();

        cliente.Nombre = dto.Nombre;
        cliente.Apellido = dto.Apellido;
        cliente.NombreComercial = dto.NombreComercial;
        cliente.Correo = dto.Correo;
        cliente.Telefono = dto.Telefono;
        cliente.Direccion = dto.Direccion;

        await _context.SaveChangesAsync();
    }

    public async Task DesactivarAsync(int id, int usuarioId)
    {
        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == usuarioId);

        if (cliente == null)
            throw new KeyNotFoundException();

        cliente.Activo = false;
        await _context.SaveChangesAsync();
    }
}
