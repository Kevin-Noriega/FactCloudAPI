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

    // ── Listar ──────────────────────────────────────────────────────
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
                TelefonoFacturacion = c.TelefonoFacturacion,
                Departamento = c.Departamento,
                Ciudad = c.Ciudad,
                Direccion = c.Direccion,
                CodigoPostal = c.CodigoPostal,
                Correo = c.Correo,
                Activo = c.Activo
            })
            .ToListAsync();
    }

    // ── Por ID ──────────────────────────────────────────────────────
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
                TipoIdentificacion = c.TipoIdentificacion, TelefonoFacturacion = c.TelefonoFacturacion,
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
        var c = await _context.Clientes
            .Include(c => c.Telefonos)
            .Include(c => c.Contactos)
            .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == usuarioId);

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
            TelefonoFacturacion = dto.Telefono,
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


    // ── Actualizar ───────────────────────────────────────────────────
    public async Task ActualizarAsync(int id, ClienteCreateDto dto, int usuarioId)
    {
        var cliente = await _context.Clientes
            .Include(c => c.Telefonos)
            .Include(c => c.Contactos)
            .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == usuarioId);

        if (cliente == null)
            throw new KeyNotFoundException();

        cliente.Nombre = dto.Nombre;
        cliente.Apellido = dto.Apellido;
        cliente.NombreComercial = dto.NombreComercial;
        cliente.Correo = dto.Correo;
        cliente.TelefonoFacturacion = dto.TelefonoFacturacion;
        cliente.Direccion = dto.Direccion;
        // Agrega los campos faltantes:
        cliente.TipoIdentificacion = dto.TipoIdentificacion;
        cliente.NumeroIdentificacion = dto.NumeroIdentificacion;
        cliente.DigitoVerificacion = dto.DigitoVerificacion;
        cliente.TipoPersona = dto.TipoPersona;
        cliente.RegimenTributario = dto.RegimenTributario;
        cliente.Departamento = dto.Departamento;
        cliente.DepartamentoCodigo = dto.DepartamentoCodigo;
        cliente.Ciudad = dto.Ciudad;
        cliente.CiudadCodigo = dto.CiudadCodigo;
        cliente.Direccion = dto.Direccion;
        cliente.CodigoPostal = dto.CodigoPostal;
        cliente.NombreComercial = dto.NombreComercial;

        cliente.Contactos = dto.Contactos
            .Where(c => !string.IsNullOrWhiteSpace(c.Nombre))
            .Select(c => new ContactoCliente
            {
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                Correo = c.Correo,
                Cargo = c.Cargo,
                Indicativo = c.Indicativo,
                Telefono = c.Telefono,
            }).ToList();

        await _context.SaveChangesAsync();
    }

    // ── Desactivar ───────────────────────────────────────────────────
    public async Task DesactivarAsync(int id, int usuarioId)
    {
        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == usuarioId);

        if (cliente == null)
            throw new KeyNotFoundException();

        cliente.Activo = false;
        await _context.SaveChangesAsync();
    }

    // ── Mapper privado ───────────────────────────────────────────────
    private static ClienteDetalleDto MapToDto(Cliente c) => new()
    {
        Id = c.Id,
        TipoPersona = c.TipoPersona,
        TipoIdentificacion = c.TipoIdentificacion,
        NumeroIdentificacion = c.NumeroIdentificacion,
        DigitoVerificacion = c.DigitoVerificacion,
        CodigoSucursal = c.CodigoSucursal,
        Nombre = c.Nombre,
        Apellido = c.Apellido,
        NombreComercial = c.NombreComercial,
        Departamento = c.Departamento,
        DepartamentoCodigo = c.DepartamentoCodigo,
        Ciudad = c.Ciudad,
        CiudadCodigo = c.CiudadCodigo,
        Direccion = c.Direccion,
        CodigoPostal = c.CodigoPostal,
        Correo = c.Correo,
        RegimenTributario = c.RegimenTributario,
        NombreContactoFacturacion = c.NombreContactoFacturacion,
        ApellidoContactoFacturacion = c.ApellidoContactoFacturacion,
        IndicativoFacturacion = c.IndicativoFacturacion,
        TelefonoFacturacion = c.TelefonoFacturacion,
        GranContribuyente = c.GranContribuyente,
        AutoretenedorRenta = c.AutoretenedorRenta,
        RetenedorIVA = c.RetenedorIVA,
        RegimenSimple = c.RegimenSimple,
        NoAplica = c.NoAplica,
        RetenedorICA = c.RetenedorICA,
        RetenedorRenta = c.RetenedorRenta,
        Activo = c.Activo,
        FechaRegistro = c.FechaRegistro,
        Telefonos = c.Telefonos.Select(t => new TelefonoDto
        {
            Indicativo = t.Indicativo,
            Numero = t.Numero,
            Extension = t.Extension,
        }).ToList(),
        Contactos = c.Contactos.Select(ct => new ContactoDto
        {
            Nombre = ct.Nombre,
            Apellido = ct.Apellido,
            Correo = ct.Correo,
            Cargo = ct.Cargo,
            Indicativo = ct.Indicativo,
            Telefono = ct.Telefono,
        }).ToList(),
    };
}