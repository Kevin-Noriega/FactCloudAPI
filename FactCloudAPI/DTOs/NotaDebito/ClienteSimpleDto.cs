namespace FactCloudAPI.DTOs.NotaDebito
{
    public class ClienteSimpleDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Apellido { get; set; }
        public string? Documento { get; set; }
    }
}
