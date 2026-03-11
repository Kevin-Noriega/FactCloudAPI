namespace FactCloudAPI.Models
{
    public class ContactoCliente
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Apellido { get; set; }
        public string? Correo { get; set; }
        public string? Cargo { get; set; }
        public string? Indicativo { get; set; }
        public string? Telefono { get; set; }

        public Cliente Cliente { get; set; } = null!;
    }
}
