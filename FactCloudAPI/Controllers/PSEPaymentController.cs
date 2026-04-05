
using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Wompi;
using FactCloudAPI.Models;
using FactCloudAPI.Services;
using FactCloudAPI.Services.Wompi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace FactCloudAPI.Controllers
{
    [ApiController]
    [Route("api/payment/pse")]
    public class PSEPaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWompiService _wompi;
        private readonly IConfiguration _config;
        private readonly ILogger<PSEPaymentController> _logger;

        public PSEPaymentController(
            ApplicationDbContext context,
            IWompiService wompi,
            IConfiguration config,
            ILogger<PSEPaymentController> logger)
        {
            _context = context;
            _wompi = wompi;
            _config = config;
            _logger = logger;
        }

        // ═══════════════════════════════════════════
        // 1. GET /api/payment/pse/bancos
        // ═══════════════════════════════════════════
        [HttpGet("bancos")]
        public async Task<IActionResult> GetBancos()
        {
            try
            {
                var banks = await _wompi.GetFinancialInstitutionsAsync();
                return Ok(new { data = banks });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo bancos PSE");
                return StatusCode(500, new { error = "Error obteniendo bancos" });
            }
        }

        // ═══════════════════════════════════════════
        // 2. POST /api/payment/pse/crear-transaccion
        // ═══════════════════════════════════════════
        [HttpPost("crear-transaccion")]
        public async Task<IActionResult> CrearTransaccion(
            [FromBody] PseTransaccionDto request)
        {
            try
            {
                _logger.LogInformation(
                    "Creando transacción PSE: {Reference}, Monto: {Amount}",
                    request.Reference, request.PrecioEnCentavos);

                // 1. Construir payload para Wompi
                var frontendUrl = _config["Wompi:FrontendUrl"]
                    ?? "http://localhost:5173";

                var wompiPayload = new
                {
                    amount_in_cents = request.PrecioEnCentavos,
                    currency = request.Currency,
                    customer_email = request.Email,
                    reference = request.Reference,
                    acceptance_token = request.AcceptanceToken,
                    payment_method = new
                    {
                        type = "PSE",
                        user_type = request.PaymentMethod.User_type,
                        user_legal_id_type = request.PaymentMethod.User_legal_id_type,
                        user_legal_id = request.PaymentMethod.User_legal_id,
                        financial_institution_code =
                            request.PaymentMethod.Financial_institution_code,
                        payment_description =
                            request.PaymentMethod.Payment_description
                    },
                    customer_data = new
                    {
                        full_name = request.CustomerData.FullName,
                        phone_number = request.CustomerData.PhoneNumber,
                        legal_id = request.CustomerData.LegalId.Number,
                        legal_id_type = request.CustomerData.LegalId.Type
                    },
                    redirect_url = $"{frontendUrl}/pse/resultado"
                };

                // 2. Enviar a Wompi
                var transactionData = await _wompi
                    .CreatePSETransactionAsync(wompiPayload);

                var transactionId = transactionData.GetProperty("id").GetString()!;
                var status = transactionData.GetProperty("status").GetString()!;

                // 3. Extraer async_payment_url
                string? asyncPaymentUrl = null;
                if (transactionData.TryGetProperty("payment_method", out var pm) &&
                    pm.TryGetProperty("extra", out var extra) &&
                    extra.TryGetProperty("async_payment_url", out var urlProp))
                {
                    asyncPaymentUrl = urlProp.GetString();
                }

                // 4. Hashear password antes de guardar
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(
                    request.DatosRegistro.Password);

                // 5. Guardar registro pendiente
                var registro = new RegistroPendiente
                {
                    TransaccionId = transactionId,
                    WompiReference = request.Reference,
                    Estado = status,

                    Nombre = request.DatosRegistro.Nombre,
                    Correo = request.DatosRegistro.Correo,
                    Telefono = request.DatosRegistro.Telefono,
                    PasswordHash = passwordHash,
                    TipoIdentificacion = request.DatosRegistro.TipoIdentificacion,
                    NumeroIdentificacion = request.DatosRegistro.NumeroIdentificacion,

                    NombreNegocio = request.DatosNegocio.NombreNegocio,
                    Nit = request.DatosNegocio.Nit,
                    DvNit = request.DatosNegocio.DvNit,
                    Direccion = request.DatosNegocio.Direccion,
                    Ciudad = request.DatosNegocio.Ciudad,
                    Departamento = request.DatosNegocio.Departamento,
                    TelefonoNegocio = request.DatosNegocio.TelefonoNegocio,
                    CorreoNegocio = request.DatosNegocio.CorreoNegocio,

                    PlanFacturacionId = request.DatosPlan.PlanFacturacionId,

                    FechaCreacion = DateTime.UtcNow
                };

                _context.RegistrosPendientes.Add(registro);
                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Transacción PSE creada: {Id}, URL: {Url}",
                    transactionId, asyncPaymentUrl);

                // 6. Responder al frontend
                return Ok(new
                {
                    transaccionId = transactionId,
                    asyncPaymentUrl,
                    status
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando transacción PSE");
                return BadRequest(new { error = ex.Message });
            }
        }

        // ═══════════════════════════════════════════
        // 3. GET /api/payment/pse/estado/{transaccionId}
        // ═══════════════════════════════════════════
        [HttpGet("estado/{transaccionId}")]
        public async Task<IActionResult> EstadoTransaccion(string transaccionId)
        {
            try
            {
                var data = await _wompi.GetTransactionStatusAsync(transaccionId);

                return Ok(new
                {
                    status = data.GetProperty("status").GetString(),
                    reference = data.GetProperty("reference").GetString(),
                    transactionId = transaccionId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consultando estado PSE: {Id}", transaccionId);
                return StatusCode(500, new { error = "Error consultando estado" });
            }
        }
    }
}
