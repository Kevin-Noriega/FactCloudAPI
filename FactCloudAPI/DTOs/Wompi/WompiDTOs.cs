namespace FactCloudAPI.DTOs.Wompi
{
    public class RegistroPendienteDto
    {
        public string TransaccionId { get; set; }
        public string Estado { get; set; }
        public DatosRegistroDto DatosRegistro { get; set; }
        public DatosNegocioDto DatosNegocio { get; set; }
        public DatosPlanDto DatosPlan { get; set; }
    }

    public class DatosRegistroDto
    {
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }
        public string TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
    }

    public class DatosNegocioDto
    {
        public string NombreNegocio { get; set; }
        public string Nit { get; set; }
        public string DvNit { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string Departamento { get; set; }
        public string TelefonoNegocio { get; set; }
        public string CorreoNegocio { get; set; }
    }

    public class DatosPlanDto
    {
        public int PlanFacturacionId { get; set; }
        public string TipoPago { get; set; }
        public decimal PrecioPagado { get; set; }
    }
}
