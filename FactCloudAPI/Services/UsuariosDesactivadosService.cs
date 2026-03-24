using FactCloudAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace FactCloudAPI.Services
{
    public class UsuariosDesactivadosService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public UsuariosDesactivadosService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var limite = DateTime.UtcNow.AddDays(-30);

                var usuariosParaEliminar = await context.Usuarios
                    .Where(u => u.Estado &&
                                u.FechaDesactivacion != null &&
                                u.FechaDesactivacion <= limite)
                    .ToListAsync(stoppingToken);

                if (usuariosParaEliminar.Any())
                {
                    context.Usuarios.RemoveRange(usuariosParaEliminar);
                    await context.SaveChangesAsync(stoppingToken);
                }

                // Espera 24 horas
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    
    }
}
