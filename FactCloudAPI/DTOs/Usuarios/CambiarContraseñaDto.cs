namespace FactCloudAPI.DTOs.Usuarios
{
    public class CambiarContraseñaDto
    {
            public string ContraseñaActual { get; set; } = "";
            public string NuevaContraseña { get; set; } = "";
            public string ConfirmarContraseña { get; set; } = "";
        
    }
}
