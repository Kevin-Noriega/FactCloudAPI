using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Usuarios;
using FactCloudAPI.Models;
using FactCloudAPI.Utils.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
                NombreNegocio = usuario.NombreNegocio,
                NitNegocio = usuario.NitNegocio,
               DireccionNegocio = usuario.DireccionNegocio,
                CiudadNegocio = usuario.CiudadNegocio,
                DepartamentoNegocio = usuario.DepartamentoNegocio,
                CorreoNegocio = usuario.CorreoNegocio,
                LogoNegocio = usuario.LogoNegocio,
                TelefonoNegocio = usuario.TelefonoNegocio,
                 FechaRegistro = usuario.FechaRegistro
            };
        }

    }
}
