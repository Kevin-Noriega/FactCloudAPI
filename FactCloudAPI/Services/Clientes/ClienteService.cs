using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Clientes;
using FactCloudAPI.Models;
using FactCloudAPI.Services.Clientes;
using Microsoft.EntityFrameworkCore;

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
            .Include(c => c.Telefonos)
            .Include(c => c.Contactos)
            .Select(c => MapToDto(c))
            .ToListAsync();
    }

    // ── Por ID ──────────────────────────────────────────────────────
    public async Task<ClienteDetalleDto?> ObtenerPorIdAsync(int id, int usuarioId)
    {
        var c = await _context.Clientes
            .Include(c => c.Telefonos)
            .Include(c => c.Contactos)
            .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == usuarioId);

        return c == null ? null : MapToDto(c);
    }

    // ── Crear ────────────────────────────────────────────────────────
    public async Task CrearAsync(ClienteCreateDto dto, int usuarioId)
    {
        bool existe = await _context.Clientes.AnyAsync(c =>
            c.NumeroIdentificacion == dto.NumeroIdentificacion &&
            c.UsuarioId == usuarioId);

        if (existe)
            throw new InvalidOperationException("Ya existe un cliente con esa identificación.");

        var cliente = new Cliente
        {
            UsuarioId = usuarioId,
            TipoPersona = dto.TipoPersona,
            TipoIdentificacion = dto.TipoIdentificacion,
            NumeroIdentificacion = dto.NumeroIdentificacion,
            DigitoVerificacion = dto.DigitoVerificacion,
            CodigoSucursal = dto.CodigoSucursal ?? "0",
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            NombreComercial = dto.NombreComercial,
            Departamento = dto.Departamento,
            DepartamentoCodigo = dto.DepartamentoCodigo,
            Ciudad = dto.Ciudad,
            CiudadCodigo = dto.CiudadCodigo,
            Direccion = dto.Direccion,
            CodigoPostal = dto.CodigoPostal,
            Correo = dto.Correo ?? "",
            RegimenTributario = dto.RegimenTributario ?? "",
            NombreContactoFacturacion = dto.NombreContactoFacturacion,
            ApellidoContactoFacturacion = dto.ApellidoContactoFacturacion,
            IndicativoFacturacion = dto.IndicativoFacturacion,
            TelefonoFacturacion = dto.TelefonoFacturacion,
            GranContribuyente = dto.GranContribuyente,
            AutoretenedorRenta = dto.AutoretenedorRenta,
            RetenedorIVA = dto.RetenedorIVA,
            RegimenSimple = dto.RegimenSimple,
            NoAplica = dto.NoAplica,
            RetenedorICA = dto.RetenedorICA,
            RetenedorRenta = dto.RetenedorRenta,
            Activo = true,
            Telefonos = dto.Telefonos
                .Where(t => !string.IsNullOrWhiteSpace(t.Numero))
                .Select(t => new TelefonoCliente
                {
                    Indicativo = t.Indicativo,
                    Numero = t.Numero,
                    Extension = t.Extension,
                }).ToList(),
            Contactos = dto.Contactos
                .Where(c => !string.IsNullOrWhiteSpace(c.Nombre))
                .Select(c => new ContactoCliente
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

        if (cliente == null) throw new KeyNotFoundException();

        cliente.TipoPersona = dto.TipoPersona;
        cliente.TipoIdentificacion = dto.TipoIdentificacion;
        cliente.NumeroIdentificacion = dto.NumeroIdentificacion;
        cliente.DigitoVerificacion = dto.DigitoVerificacion;
        cliente.CodigoSucursal = dto.CodigoSucursal ?? "0";
        cliente.Nombre = dto.Nombre;
        cliente.Apellido = dto.Apellido;
        cliente.NombreComercial = dto.NombreComercial;
        cliente.Departamento = dto.Departamento;
        cliente.DepartamentoCodigo = dto.DepartamentoCodigo;
        cliente.Ciudad = dto.Ciudad;
        cliente.CiudadCodigo = dto.CiudadCodigo;
        cliente.Direccion = dto.Direccion;
        cliente.CodigoPostal = dto.CodigoPostal;
        cliente.Correo = dto.Correo ?? "";
        cliente.RegimenTributario = dto.RegimenTributario ?? "";
        cliente.NombreContactoFacturacion = dto.NombreContactoFacturacion;
        cliente.ApellidoContactoFacturacion = dto.ApellidoContactoFacturacion;
        cliente.IndicativoFacturacion = dto.IndicativoFacturacion;
        cliente.TelefonoFacturacion = dto.TelefonoFacturacion;
        cliente.GranContribuyente = dto.GranContribuyente;
        cliente.AutoretenedorRenta = dto.AutoretenedorRenta;
        cliente.RetenedorIVA = dto.RetenedorIVA;
        cliente.RegimenSimple = dto.RegimenSimple;
        cliente.NoAplica = dto.NoAplica;
        cliente.RetenedorICA = dto.RetenedorICA;
        cliente.RetenedorRenta = dto.RetenedorRenta;

        // Reemplazar teléfonos y contactos
        _context.RemoveRange(cliente.Telefonos);
        _context.RemoveRange(cliente.Contactos);

        cliente.Telefonos = dto.Telefonos
            .Where(t => !string.IsNullOrWhiteSpace(t.Numero))
            .Select(t => new TelefonoCliente
            {
                Indicativo = t.Indicativo,
                Numero = t.Numero,
                Extension = t.Extension,
            }).ToList();

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

        if (cliente == null) throw new KeyNotFoundException();

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