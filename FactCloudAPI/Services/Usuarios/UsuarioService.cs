using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Usuarios;
using FactCloudAPI.Models;
using FactCloudAPI.Models.Suscripciones;
using FactCloudAPI.Models.Usuarios;
using FactCloudAPI.Utils.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using FactCloudAPI.Models.Suscripciones;

namespace FactCloudAPI.Services.Usuarios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;
        public UsuarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UsuarioListDto>> GetAllAsync()
        {
            return await _context.Usuarios
                .Select(u => new UsuarioListDto
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Correo = u.Correo,
                    Estado = u.Estado
                })
                .ToListAsync();
        }
        public async Task<UsuarioDetalleDto> GetByIdAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                throw new BusinessException("Usuario no encontrado");

            return MapToDetalleDto(usuario);
        }
        public async Task<UsuarioDetalleDto> GetMeAsync(int userId)
           => await GetByIdAsync(userId);

        public async Task<int> CreateAsync(CreateUsuarioDto dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Correo == dto.Correo))
                throw new BusinessException("El correo ya está registrado");

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Correo = dto.Correo,
                Telefono = dto.Telefono,
                TipoIdentificacion = dto.TipoIdentificacion,
                NumeroIdentificacion = dto.NumeroIdentificacion,
                ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FechaRegistro = DateTime.UtcNow,
                Estado = true
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return usuario.Id;
        }

        public async Task UpdateAsync(int id, UpdateUsuarioDto dto)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Clientes)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                throw new BusinessException("Usuario no encontrado");

            // REGLA DE NEGOCIO 
            if (usuario.Clientes?.Count >= 100)
                throw new BusinessException("No puedes tener más de 100 clientes");

            usuario.Nombre = dto.Nombre;
            usuario.Apellido = dto.Apellido;
            usuario.Telefono = dto.Telefono;
            usuario.Estado = dto.Estado;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                throw new BusinessException("Usuario no encontrado");

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }
        private static UsuarioDetalleDto MapToDetalleDto(Usuario usuario)
        {
            return new UsuarioDetalleDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                Estado = usuario.Estado,
                 FechaRegistro = usuario.FechaRegistro
            };
        }
        public async Task<(Usuario usuario, string token)> CrearYActivarAsync(CrearYActivarDto dto)
        {
            // Validar correo duplicado
            if (await _context.Usuarios.AnyAsync(u => u.Correo == dto.Correo))
                throw new BusinessException("El correo ya está registrado");

            // 1. Crear usuario
            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Correo = dto.Correo,
                Telefono = dto.Telefono,
                TipoIdentificacion = dto.TipoIdentificacion,
                NumeroIdentificacion = dto.NumeroIdentificacion,
                ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FechaRegistro = DateTime.UtcNow,
                Estado = true
            };
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync(); // Genera usuario.Id

            // 2. Crear negocio vinculado al usuario
            var negocio = new Negocio
            {
                UsuarioId = usuario.Id,
                NombreComercial = dto.NombreComercial,
                NumeroIdentificacionE = dto.NumeroIdentificacionE,
                DvNit = dto.DvNit,
                Direccion = dto.Direccion,
                Ciudad = dto.Ciudad,
                Departamento = dto.Departamento,
                Telefono = dto.TelefonoNegocio,
                CorreoRecepcionDian = dto.CorreoRecepcionDian,
                
            };
            _context.Negocios.Add(negocio);
            await _context.SaveChangesAsync(); // Genera negocio.Id

            // 3. Crear suscripción activa
            // 3. Crear suscripción activa
            var suscripcion = new SuscripcionFacturacion
            {
                UsuarioId = usuario.Id,
                PlanFacturacionId = dto.PlanFacturacionId,
                TransaccionId = dto.TransaccionId,
                FechaInicio = DateTime.UtcNow,
                FechaFin = dto.TipoPago == "anual"
                    ? DateTime.UtcNow.AddYears(1)
                    : DateTime.UtcNow.AddMonths(1),
                Activa = true,
                DocumentosUsados = 0
            };
            _context.SuscripcionesFacturacion.Add(suscripcion);
            await _context.SaveChangesAsync();


            return (usuario, ""); // El token lo genera el AuthService si lo tienes
        }


    }
}
