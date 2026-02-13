namespace FactCloudAPI.DTOs.FotoPerfil
{
    public class FotoPerfilDto
    {
        public int Id { get; set; }
        public string Url { get; set; } = "";
        public DateTime FechaSubida { get; set; }
        public bool EsPrincipal { get; set; }
    }
}
