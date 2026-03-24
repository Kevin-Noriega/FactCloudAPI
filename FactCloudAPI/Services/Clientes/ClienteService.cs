using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Clientes;
using FactCloudAPI.Models;
using FactCloudAPI.Services.Clientes;
using Microsoft.EntityFrameworkCore;
using FactCloudAPI.Models; 


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
                NumeroIdentificacion = c.NumeroIdentificacion,
                TipoIdentificacion = c.TipoIdentificacion,
                Telefono = c.Telefono,
                Departamento = c.Departamento,
                Ciudad = c.Ciudad,
                Direccion = c.Direccion,
                CodigoPostal = c.CodigoPostal,
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
                NumeroIdentificacion = c.NumeroIdentificacion,
                TipoIdentificacion = c.TipoIdentificacion,
                Telefono = c.Telefono,
                Departamento = c.Departamento,
                Ciudad = c.Ciudad,
                Direccion = c.Direccion,
                CodigoPostal = c.CodigoPostal,
                Correo = c.Correo,
                Activo = c.Activo
            })
            .FirstOrDefaultAsync();
    }
    public async Task<bool> ActualizarParcialAsync(int id, ClienteUpdateDto dto, int usuarioId)
    {
        var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == usuarioId);
        if (cliente == null) return false;

        if (dto.EsProveedor.HasValue) cliente.EsProveedor = dto.EsProveedor.Value;
        if (dto.RetenedorIVA.HasValue) cliente.RetenedorIVA = dto.RetenedorIVA.Value;
        if (dto.RetenedorICA.HasValue) cliente.RetenedorICA = dto.RetenedorICA.Value;
        if (dto.RetenedorRenta.HasValue) cliente.RetenedorRenta = dto.RetenedorRenta.Value;
        if (dto.AutoretenedorRenta.HasValue) cliente.AutoretenedorRenta = dto.AutoretenedorRenta.Value;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ActivarAsync(int id, int usuarioId)
    {
        var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == usuarioId);
        if (cliente == null) return false;

        cliente.Activo = true;
        await _context.SaveChangesAsync();
        return true;
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
            CodigoPostal = dto.CodigoPostal,
             Contactos = dto.Contactos.Select(c => new ContactoCliente
             {
                 Nombre = c.Nombre,
                 Apellido = c.Apellido,
                 Correo = c.Correo,
                 Cargo = c.Cargo,
                 Indicativo = c.Indicativo,
                 Telefono = c.Telefono,
             }).ToList()
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
        // Agrega los campos faltantes:
        cliente.TipoIdentificacion = dto.TipoIdentificacion;
        cliente.NumeroIdentificacion = dto.NumeroIdentificacion;
        cliente.DigitoVerificacion = dto.DigitoVerificacion;
        cliente.TipoPersona = dto.TipoPersona;
        cliente.RegimenTributario = dto.RegimenTributario;
        cliente.Departamento = dto.Departamento;
        cliente.Ciudad = dto.Ciudad;
        cliente.CodigoPostal = dto.CodigoPostal;
        cliente.NombreComercial = dto.NombreComercial;


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
