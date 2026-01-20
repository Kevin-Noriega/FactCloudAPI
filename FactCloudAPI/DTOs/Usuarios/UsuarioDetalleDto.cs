namespace FactCloudAPI.DTOs.Usuarios
{
    public class UsuarioDetalleDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string NombreNegocio { get; set; }
    }
}
