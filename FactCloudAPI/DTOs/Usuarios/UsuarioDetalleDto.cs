using System.ComponentModel.DataAnnotations;

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
        public string NitNegocio { get; set; }

        public string DireccionNegocio { get; set; }

        public string CiudadNegocio { get; set; }

        public string DepartamentoNegocio { get; set; }

        public string CorreoNegocio { get; set; }
        public string LogoNegocio { get; set; }
        public string TipoIdentificacion { get; set; }
        public string TelefonoNegocio { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
