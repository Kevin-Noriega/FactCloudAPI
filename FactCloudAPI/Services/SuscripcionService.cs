using FactCloudAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace FactCloudAPI.Services
{
    public class SuscripcionService : ISuscripcionService
    {
        private readonly ApplicationDbContext _context;

        public SuscripcionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task IncrementarDocumentosUsados(int usuarioId)
        {
            var suscripcion = await _context.SuscripcionesFacturacion
                .Where(s => s.UsuarioId == usuarioId && s.Activa)
                .FirstOrDefaultAsync();

            if (suscripcion != null)
            {
                suscripcion.DocumentosUsados++;
                await _context.SaveChangesAsync();
            }
        }
    }
}
