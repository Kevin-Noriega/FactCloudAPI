namespace NubeeAPI.DTOs.Factus
{
    /// <summary>Respuesta al frontend cuando una factura es validada por Factus/DIAN</summary>
    public class FactusEnvioResultDto
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = "";
        public string? Cufe { get; set; }
        public string? Qr { get; set; }
        public string? NumeroFactus { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public string? ErrorDetalle { get; set; }
    }

    /// <summary>Payload para registrar una resolución DIAN en Factus (habilitación)</summary>
    public class HabilitarEmpresaDto
    {
        public int ResolucionId { get; set; }
        public bool Forzar { get; set; } = false;   // re-registrar si ya existe
    }
}
