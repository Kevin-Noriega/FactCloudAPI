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
        private readonly WompiService _wompiService;
        private readonly ApplicationDbContext _context; 
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;  
        private readonly string _wompiBaseUrl;                 
        private readonly string _wompiPrivateKey;              
        private readonly string _wompiEventSecret;               
        private readonly string _frontendUrl;
        private readonly string _wompiPublicKey;
        private readonly IUsuarioService _usuarioService;


        public PaymentController(
        WompiService wompiService,
        ApplicationDbContext context,
        IConfiguration config,
        IHttpClientFactory httpClientFactory,
             IUsuarioService usuarioService)  // ✅ INYECTAR
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

                // ✅ Consultar a Wompi, no devolver mock
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
                var result = await _wompiService.CreateTransactionAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpPost("guardar-registro-pendiente")]
        [AllowAnonymous]
        public async Task<IActionResult> GuardarRegistroPendiente([FromBody] RegistroPendienteDto dto)
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

        [HttpGet("pse/bancos")]
        public async Task<IActionResult> GetBancosPSE()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var url = $"{_wompiBaseUrl}/pse/financial_institutions?public-key={_wompiPublicKey}";
            var response = await httpClient.GetAsync(url);
            var data = await response.Content.ReadAsStringAsync();
            return Ok(JsonSerializer.Deserialize<object>(data));
        }

        [HttpPost("pse/crear-transaccion")]
        public async Task<IActionResult> CrearTransaccionPSE([FromBody] PseTransaccionDto dto)
        {
            var reference = $"FACTCLOUD-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";

            // 1. Llamar a Wompi con llave PRIVADA
            var wompiBody = new
            {
                amount_in_cents = dto.PrecioEnCentavos,
                currency = "COP",
                customer_email = dto.Email,
                reference = reference,
                acceptance_token = dto.AcceptanceToken,
                redirect_url = $"{_frontendUrl}/pago-resultado",
                payment_method = new
                {
                    type = "PSE",
                    user_type = dto.TipoPersona == "Natural" ? 0 : 1,
                    user_legal_id_type = dto.TipoDocumento,
                    user_legal_id = dto.NumeroDocumento,
                    financial_institution_code = dto.CodigoBanco,
                    payment_description = $"Plan {dto.NombrePlan} - FactCloud"
                }
            };

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _wompiPrivateKey); // 🔑 Llave PRIVADA

            var response = await httpClient.PostAsJsonAsync($"{_wompiBaseUrl}/transactions", wompiBody);
            var result = await response.Content.ReadFromJsonAsync<WompiTransactionResponse>(
                 new JsonSerializerOptions
            {
                   PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
              });

            // 2. Guardar en tu DB como PENDIENTE (NO crear usuario aún)
            await _context.Transacciones.AddAsync(new Transaccion
            {
                WompiId = result.Data.Id,
                Reference = reference,
                Estado = "PENDING",
                PlanId = dto.PlanId,
                DatosRegistro = JsonSerializer.Serialize(dto.DatosRegistro),
                DatosNegocio = JsonSerializer.Serialize(dto.DatosNegocio),
                CreadoEn = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            return Ok(new { transaccionId = result.Data.Id });
        }

        [HttpGet("pse/estado/{transaccionId}")]
        public async Task<IActionResult> GetEstado(string transaccionId)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _wompiPrivateKey);

            var response = await httpClient.GetAsync($"{_wompiBaseUrl}/transactions/{transaccionId}");
            var result = await response.Content.ReadFromJsonAsync<WompiTransactionResponse>(
                 new JsonSerializerOptions
             {
                   PropertyNameCaseInsensitive = true,
                  PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
              });
            dynamic transaccion = result.Data;
            var status = (string)transaccion.Status;
            var asyncUrl = (string?)transaccion.PaymentMethodObject?.Extra?.AsyncPaymentUrl;
            // Si está aprobada, crear usuario
            // En GetEstado (línea ~199):
            if (status == "APPROVED")
            {
                var tx = await _context.Transacciones.FirstAsync(t => t.WompiId == transaccionId);
                if (tx.Estado != "APPROVED")
                {
                    tx.Estado = "APPROVED";
                    await _context.SaveChangesAsync();

                    var datosRegistro = JsonSerializer.Deserialize<DatosRegistroDto>(tx.DatosRegistro,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    var datosNegocio = JsonSerializer.Deserialize<DatosNegocioDto>(tx.DatosNegocio,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    await _usuarioService.CrearYActivarAsync(new CrearYActivarDto
                    {
                        Nombre = datosRegistro!.Nombre,
                        Correo = datosRegistro.Correo,
                        Telefono = datosRegistro.Telefono,
                        Password = datosRegistro.Password,
                        TipoIdentificacion = datosRegistro.TipoIdentificacion,
                        NumeroIdentificacion = datosRegistro.NumeroIdentificacion,
                        NombreNegocio = datosNegocio!.NombreNegocio,
                        Nit = datosNegocio.Nit,
                        DvNit = int.TryParse(datosNegocio.DvNit, out var dv) ? dv : null,
                        Direccion = datosNegocio.Direccion,
                        Ciudad = datosNegocio.Ciudad,
                        Departamento = datosNegocio.Departamento,
                        TelefonoNegocio = datosNegocio.TelefonoNegocio,
                        CorreoNegocio = datosNegocio.CorreoNegocio,
                        PlanFacturacionId = tx.PlanId,
                        TransaccionId = transaccionId,
                        TipoPago = "anual",
                        PrecioPagado = 0 // o guardarlo en la Transaccion si lo necesitas
                    });
                }
            }


            return Ok(new { status, asyncPaymentUrl = asyncUrl });
        }

       

        private string ComputeHMACSHA256(string data, string key)
        {
            var encoding = new UTF8Encoding();
            var hash = new HMACSHA256(encoding.GetBytes(key));
            var bytes = hash.ComputeHash(encoding.GetBytes(data));
            return string.Concat(bytes.Select(b => b.ToString("x2")));
        }







        [HttpPost("webhook/wompi")]
        [AllowAnonymous]
        public async Task<IActionResult> WebhookWompi()
        {
            try
            {
                Console.WriteLine("🔔 Webhook de Wompi recibido");

                // Leer el body del webhook
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();

                Console.WriteLine($"📄 Body completo: {body}");

                var webhookData = JsonSerializer.Deserialize<WompiWebhookDto>(
                    body,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (webhookData?.Data?.Transaction == null)
                {
                    Console.WriteLine("❌ Webhook sin datos de transacción");
                    return BadRequest("Datos inválidos");
                }

                //  Verificar firma (comentar en desarrollo si da problemas)
                 if (!VerificarFirmaWebhook(webhookData))
                 {
                         Console.WriteLine("❌ Firma del webhook inválida");
                         return Unauthorized("Firma inválida");
                 }

                var transactionId = webhookData.Data.Transaction.Id;
                var status = webhookData.Data.Transaction.Status;

                Console.WriteLine($"📊 Transacción: {transactionId}");
                Console.WriteLine($"📊 Estado: {status}");

                // Buscar registro pendiente
                var registroPendiente = await _context.RegistrosPendientes
                    .FirstOrDefaultAsync(r => r.TransaccionId == transactionId);

                if (registroPendiente == null)
                {
                    Console.WriteLine($"⚠️ Registro pendiente no encontrado para: {transactionId}");
                    return NotFound(new { message = "Registro no encontrado" });
                }

                Console.WriteLine($"✅ Registro encontrado: {registroPendiente.Email}");

                // Si el pago fue aprobado
                if (status == "APPROVED")
                {
                    Console.WriteLine("✅ Pago aprobado, procesando usuario...");

                    var datosRegistro = JsonSerializer.Deserialize<DatosRegistroDto>(
                        registroPendiente.DatosRegistro);
                    var datosPlan = JsonSerializer.Deserialize<DatosPlanDto>(
                        registroPendiente.DatosPlan);
                    var datosNegocio = JsonSerializer.Deserialize<DatosNegocioDto>(
                        registroPendiente.DatosNegocio);

                    // TODO: Aquí llamas a tu método de creación de usuario
                    // var usuario = await CrearUsuarioCompleto(datosRegistro, datosNegocio, datosPlan, transactionId);

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


        private bool VerificarFirmaWebhook(WompiWebhookDto webhook)
        {
            try
            {
                var integrityKey = _config["Wompi:IntegrityKey"];

                if (string.IsNullOrEmpty(integrityKey))
                {
                    Console.WriteLine("⚠️ IntegrityKey no configurada");
                    return true; // En desarrollo, permitir sin validación
                }

                // Construir la cadena de datos según la documentación de Wompi
                var dataToSign = $"{webhook.Event}{webhook.Data.Transaction.Id}{webhook.Timestamp}";

                // Generar la firma esperada
                var expectedSignature = GenerateSignature(dataToSign, integrityKey);

                Console.WriteLine($"🔐 Firma recibida: {webhook.Signature?.CheckSum}");
                Console.WriteLine($"🔐 Firma esperada: {expectedSignature}");

                // Comparar firmas
                return webhook.Signature?.CheckSum == expectedSignature;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error verificando firma: {ex.Message}");
                return false;
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

        
        private string GenerateSignature(string data, string integrityKey)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(data + integrityKey);
                var hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
