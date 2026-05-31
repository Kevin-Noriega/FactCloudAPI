namespace NubeeAPI.Models.Impuestos
{
    public class PeriodoContable
    {
        
        public int Id { get; set; }
        public int? UsuarioId { get; set; }

        public int Anio { get; set; }
        public int Mes { get; set; }  // 1-12, 13=cierre

        public EstadoPeriodo Estado { get; set; } = EstadoPeriodo.Abierto;

        public DateTime? FechaCierre { get; set; }
        public int? UsuarioCierreId { get; set; }

        // Índice único: un período por empresa por mes/año
    }

    public enum EstadoPeriodo
    {
        Abierto = 1,
        CierrePrevio = 2,  // solo ajustes de cierre
        Cerrado = 3,       // bloqueado totalmente
        Bloqueado = 4      // bloqueado por auditoría
    }
}
