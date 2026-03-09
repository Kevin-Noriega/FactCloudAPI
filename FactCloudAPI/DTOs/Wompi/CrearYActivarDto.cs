namespace FactCloudAPI.DTOs.Wompi
{
    public class CrearYActivarDto
    {
        // Datos usuario
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }
        public string TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }

        // Datos negocio
        public string NombreNegocio { get; set; }
        public string Nit { get; set; }
        public int? DvNit { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string Departamento { get; set; }
        public string TelefonoNegocio { get; set; }
        public string CorreoNegocio { get; set; }

        // Datos suscripción
        public int PlanFacturacionId { get; set; }
        public string TransaccionId { get; set; }
        public string TipoPago { get; set; }
        public decimal PrecioPagado { get; set; }
    }
}
