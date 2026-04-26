using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Wompi;
using FactCloudAPI.DTOs.Wompi.Webhook;
using FactCloudAPI.Models;
using FactCloudAPI.Models.Wompi;
using FactCloudAPI.Services.Usuarios;
using FactCloudAPI.Services.Wompi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using FactCloudAPI.DTOs.Usuarios;

namespace FactCloudAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        // ✅ CAMBIO 1: IWompiService en lugar de WompiService (concreto)
        private readonly IWompiService _wompiService;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _wompiBaseUrl;
        private readonly string _wompiPrivateKey;
        private readonly string _wompiEventSecret;
        private readonly string _frontendUrl;
        private readonly string _wompiPublicKey;
        private readonly IUsuarioService _usuarioService;

        // ✅ CAMBIO 2: IWompiService en el constructor
        public PaymentController(
            IWompiService wompiService,
            ApplicationDbContext context,
            IConfiguration config,
            IHttpClientFactory httpClientFactory,
            IUsuarioService usuarioService)
        {
            _wompiService = wompiService;
            _context = context;
            _config = config;
            _httpClientFactory = httpClientFactory;
            _wompiPublicKey = config["Wompi:PublicKey"]!;
            _usuarioService = usuarioService;
            _wompiBaseUrl = config["Wompi:BaseUrl"] ?? "https://sandbox.wompi.co/v1";
            _wompiPrivateKey = config["Wompi:PrivateKey"]!;
            _wompiEventSecret = config["Wompi:EventSecret"] ?? "";
            _frontendUrl = config["Frontend:Url"] ?? "http://localhost:5173";
        }

        [HttpGet("acceptance-token")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAcceptanceToken()
        {
            try
            {
                Console.WriteLine("🔵 GetAcceptanceToken llamado - consultando Wompi real");
                var response = await _wompiService.GetAcceptanceTokenAsync();
                Console.WriteLine($"✅ Token obtenido: {response.Data.PresignedAcceptance.AcceptanceToken}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("create-transaction")]
        public async Task<IActionResult> CreateTransaction(
            [FromBody] WompiTransactionRequest request)
        {
            try
            {
                // ✅ CAMBIO 3: CreateCardTransactionAsync en lugar de CreateTransactionAsync
                var result = await _wompiService.CreateCardTransactionAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("guardar-registro-pendiente")]
        [AllowAnonymous]
        public async Task<IActionResult> GuardarRegistroPendiente(
            [FromBody] RegistroPendienteDto dto)
        {
            try
            {
                Console.WriteLine("🔵 Guardando registro pendiente...");
                Console.WriteLine($"📋 TransaccionId: {dto.TransaccionId}");

                var registro = new RegistroPendiente
                {
                    TransaccionId = dto.TransaccionId,
                    Email = dto.DatosRegistro.Correo,
                    DatosRegistro = JsonSerializer.Serialize(dto.DatosRegistro),
                    DatosPlan = JsonSerializer.Serialize(dto.DatosPlan),
                    DatosNegocio = JsonSerializer.Serialize(dto.DatosNegocio),
                    Estado = dto.Estado,
                    FechaCreacion = DateTime.UtcNow
                };

                _context.RegistrosPendientes.Add(registro);
                await _context.SaveChangesAsync();

                Console.WriteLine($"✅ Registro pendiente guardado: {dto.TransaccionId}");
                return Ok(new { message = "Registro guardado" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error guardando registro: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("webhook/wompi")]
        [AllowAnonymous]
        public async Task<IActionResult> WebhookWompi()
        {
            try
            {
                Console.WriteLine("🔔 Webhook de Wompi recibido");

                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                Console.WriteLine($"📄 Body completo: {body}");

                var webhookData = JsonSerializer.Deserialize<WompiWebhookDto>(
                    body,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (webhookData?.Data?.Transaction == null)
                {
                    Console.WriteLine("❌ Webhook sin datos de transacción");
                    return BadRequest("Datos inválidos");
                }

                if (!VerificarFirmaWebhook(webhookData))
                {
                    Console.WriteLine("❌ Firma del webhook inválida");
                    return Unauthorized("Firma inválida");
                }

                var transactionId = webhookData.Data.Transaction.Id;
                var status = webhookData.Data.Transaction.Status;

                Console.WriteLine($"📊 Transacción: {transactionId}");
                Console.WriteLine($"📊 Estado: {status}");

                var registroPendiente = await _context.RegistrosPendientes
                    .FirstOrDefaultAsync(r => r.TransaccionId == transactionId);

                if (registroPendiente == null)
                {
                    Console.WriteLine($"⚠️ Registro pendiente no encontrado: {transactionId}");
                    return NotFound(new { message = "Registro no encontrado" });
                }

                Console.WriteLine($"✅ Registro encontrado: {registroPendiente.Email}");

                if (status == "APPROVED")
                {
                    Console.WriteLine("✅ Pago aprobado, procesando usuario...");

                    var datosRegistro = JsonSerializer.Deserialize<DatosRegistroDto>(
                        registroPendiente.DatosRegistro);
                    var datosPlan = JsonSerializer.Deserialize<DatosPlanDto>(
                        registroPendiente.DatosPlan);
                    var datosNegocio = JsonSerializer.Deserialize<DatosNegocioDto>(
                        registroPendiente.DatosNegocio);

                    // TODO: CrearUsuarioCompleto(datosRegistro, datosNegocio, datosPlan, transactionId)

                    registroPendiente.Estado = "COMPLETED";
                    registroPendiente.FechaActualizacion = DateTime.UtcNow;
                    await _context.SaveChangesAsync();

                    Console.WriteLine($"✅ Proceso completado para: {datosRegistro.Correo}");
                }
                else if (status == "DECLINED" || status == "ERROR")
                {
                    Console.WriteLine("❌ Pago rechazado o con error");
                    registroPendiente.Estado = "DECLINED";
                    registroPendiente.NotasError = webhookData.Data.Transaction.Status;
                    registroPendiente.FechaActualizacion = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }

                return Ok(new { message = "Webhook procesado correctamente" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en webhook: {ex.Message}");
                Console.WriteLine($"❌ StackTrace: {ex.StackTrace}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("transaction/{id}")]
        public async Task<IActionResult> GetTransaction(string id)
        {
            try
            {
                var result = await _wompiService.GetTransactionAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        private bool VerificarFirmaWebhook(WompiWebhookDto webhook)
        {
            try
            {
                var integrityKey = _config["Wompi:IntegrityKey"];

                if (string.IsNullOrEmpty(integrityKey))
                {
                    Console.WriteLine("⚠️ IntegrityKey no configurada");
                    return true;
                }

                var dataToSign = $"{webhook.Event}{webhook.Data.Transaction.Id}{webhook.Timestamp}";
                var expectedSignature = GenerateSignature(dataToSign, integrityKey);

                Console.WriteLine($"🔐 Firma recibida: {webhook.Signature?.CheckSum}");
                Console.WriteLine($"🔐 Firma esperada: {expectedSignature}");

                return webhook.Signature?.CheckSum == expectedSignature;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error verificando firma: {ex.Message}");
                return false;
            }
        }

        private string ComputeHMACSHA256(string data, string key)
        {
            var encoding = new UTF8Encoding();
            var hash = new HMACSHA256(encoding.GetBytes(key));
            var bytes = hash.ComputeHash(encoding.GetBytes(data));
            return string.Concat(bytes.Select(b => b.ToString("x2")));
        }

        private string GenerateSignature(string data, string integrityKey)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(data + integrityKey);
            var hash = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}