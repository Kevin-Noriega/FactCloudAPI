//using NubeeAPI.Models.Impuestos;

//namespace NubeeAPI.Services.Impuestos
//{
//    public interface IPeriodoContableService
//    {
//        Task<bool> EstaAbiertoAsync(int usuarioId, int anio, int mes);
//        Task<PeriodoContable> ObtenerOCrearAsync(int usuarioId, int anio, int mes);
//    }
//    // Usar en cualquier endpoint que genere asientos:
//    public async Task ValidarPeriodoAbierto(int usuarioId, DateTime fechaDocumento)
//        {
//            var periodo = fechaDocumento.Year * 100 + fechaDocumento.Month;
//            var estaAbierto = await _periodoService.EstaAbiertoAsync(
//                usuarioId, fechaDocumento.Year, fechaDocumento.Month);

//            if (!estaAbierto)
//                throw new BusinessException(
//                    $"El período {fechaDocumento:MM/yyyy} está cerrado. " +
//                    "No se pueden registrar documentos en períodos cerrados.");
//        }
// }
