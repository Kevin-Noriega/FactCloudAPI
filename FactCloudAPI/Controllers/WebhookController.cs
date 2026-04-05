using FactCloudAPI.Data;
using FactCloudAPI.Models;
using FactCloudAPI.Models.Suscripciones;
using FactCloudAPI.Models.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace FactCloudAPI.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class WebhookController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger<WebhookController> _logger;

        public WebhookController(
            ApplicationDbContext context,
            IConfiguration config,
            ILogger<WebhookController> logger)
        {
            _context = context;
            _config = config;
            _logger = logger;
        }

        [HttpPost("webhook")]
        [AllowAnonymous] // Wompi envía sin autenticación JWT
        public async Task<IActionResult> HandleWebhook([FromBody] JsonElement payload)
        {
            try
            {
                _logger.LogInformation("Webhook recibido: {Event}",
                    payload.GetProperty("event").GetString());

                // ── 1. Extraer datos ──
                var eventType = payload.GetProperty("event").GetString();

                // Solo nos interesa transaction.updated
                if (eventType != "transaction.updated")
                {
                    _logger.LogInformation("Evento ignorado: {Event}", eventType);
                    return Ok();
                }

                var data = payload.GetProperty("data");
                var transaction = data.GetProperty("transaction");

                var transaccionId = transaction.GetProperty("id").GetString()!;
                var status = transaction.GetProperty("status").GetString()!;
                var reference = transaction.GetProperty("reference").GetString()!;

                _logger.LogInformation(
                    "Transacción actualizada: {Id} → {Status}",
                    transaccionId, status);

                // ── 2. Verificar firma SHA256 ──
                if (!VerificarFirmaWebhook(payload, transaction))
                {
                    _logger.LogWarning("Firma de webhook inválida para {Id}", transaccionId);
                    return BadRequest(new { error = "Firma inválida" });
                }

                // ── 3. Buscar registro pendiente ──
                var registro = await _context.RegistrosPendientes
                    .FirstOrDefaultAsync(r => r.TransaccionId == transaccionId);

                if (registro == null)
                {
                    _logger.LogWarning(
                        "Registro pendiente no encontrado: {Id}", transaccionId);
                    return Ok(); // Retornar 200 para que Wompi no reintente
                }

                // ── 4. Procesar según estado ──
                switch (status)
                {
                    case "APPROVED":
                        if (registro.Estado != "APPROVED")
                        {
                            await CrearUsuarioDesdeRegistro(registro);
                            registro.Estado = "APPROVED";
                            registro.FechaAprobacion = DateTime.UtcNow;
                            _logger.LogInformation(
                                "✅ Usuario creado para transacción {Id}", transaccionId);
                        }
                        break;

                    case "DECLINED":
                        registro.Estado = "DECLINED";
                        _logger.LogInformation(
                            "❌ Pago rechazado para {Id}", transaccionId);
                        break;

                    case "ERROR":
                        registro.Estado = "ERROR";
                        _logger.LogError(
                            "⚠️ Error en pago {Id}", transaccionId);
                        break;

                    case "VOIDED":
                        registro.Estado = "VOIDED";
                        break;

                    default:
                        registro.Estado = status;
                        break;
                }

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando webhook");
                // SIEMPRE retornar 200 para evitar reintentos infinitos
                return Ok();
            }
        }

        // ─────────────────────────────────────────
        // Verificar firma SHA256 del webhook
        // ─────────────────────────────────────────
        private bool VerificarFirmaWebhook(JsonElement payload, JsonElement transaction)
        {
            try
            {
                var eventSecret = _config["Wompi:EventSecret"];
                if (string.IsNullOrEmpty(eventSecret))
                {
                    _logger.LogWarning("EventSecret no configurado, omitiendo verificación");
                    return true; // En desarrollo, permite sin verificación
                }

                var signature = payload.GetProperty("signature");
                var checksum = signature.GetProperty("checksum").GetString()!;
                var properties = signature.GetProperty("properties")
                    .EnumerateArray()
                    .Select(p => p.GetString()!)
                    .ToList();

                // Construir string concatenando valores
                var values = new StringBuilder();
                foreach (var prop in properties)
                {
                    var value = GetNestedValue(transaction, prop);
                    values.Append(value);
                }
                values.Append(transaction.GetProperty("id").GetString());
                values.Append(eventSecret);

                // Calcular SHA256
                var hash = ComputeSHA256(values.ToString());

                var isValid = hash == checksum;
                if (!isValid)
                {
                    _logger.LogWarning(
                        "Firma no coincide. Esperado: {Expected}, Calculado: {Actual}",
                        checksum, hash);
                }

                return isValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verificando firma");
                return false;
            }
        }

        private static string GetNestedValue(JsonElement element, string path)
        {
            var parts = path.Split('.');
            var current = element;

            foreach (var part in parts)
            {
                if (!current.TryGetProperty(part, out current))
                    return "";
            }

            return current.ValueKind switch
            {
                JsonValueKind.String => current.GetString() ?? "",
                JsonValueKind.Number => current.GetRawText(),
                JsonValueKind.True => "true",
                JsonValueKind.False => "false",
                _ => current.GetRawText()
            };
        }

        private static string ComputeSHA256(string input)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes).ToLower();
        }

        // ─────────────────────────────────────────
        // Crear usuario desde registro pendiente
        // ─────────────────────────────────────────
        private async Task CrearUsuarioDesdeRegistro(RegistroPendiente registro)
        {
            // ══════════════════════════════════════════════════════
            // ADAPTA ESTO A TU LÓGICA EXISTENTE DE CREAR USUARIO
            // Esto es equivalente a tu endpoint /Usuarios/crear-y-activar
            // ══════════════════════════════════════════════════════

            // Verificar si ya existe
            var existente = await _context.Usuarios
                .AnyAsync(u => u.Correo == registro.Correo);

            if (existente)
            {
                _logger.LogWarning(
                    "Usuario ya existe: {Correo}", registro.Correo);
                return;
            }

            // Crear usuario
            var usuario = new Usuario
            {
                Nombre = registro.Nombre,
                Correo = registro.Correo,
                Telefono = registro.Telefono,
                ContrasenaHash = registro.PasswordHash, // Ya está hasheado
                TipoIdentificacion = registro.TipoIdentificacion,
                NumeroIdentificacion = registro.NumeroIdentificacion,
                Estado = true,
                FechaRegistro = DateTime.UtcNow
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Crear negocio
            var negocio = new Negocio
            {
                UsuarioId = usuario.Id,
                NombreNegocio = registro.NombreNegocio,
                Nit = registro.Nit,
                DvNit = !string.IsNullOrEmpty(registro.DvNit)
                    ? int.Parse(registro.DvNit)
                    : null,
                Direccion = registro.Direccion,
                Ciudad = registro.Ciudad,
                Departamento = registro.Departamento,
                Telefono = registro.TelefonoNegocio,
                Correo = registro.CorreoNegocio
            };

            _context.Negocios.Add(negocio);

            // Crear suscripción
            var suscripcion = new SuscripcionFacturacion
            {
                UsuarioId = usuario.Id,
                PlanFacturacionId = registro.PlanFacturacionId,
                Activa = true,
                FechaInicio = DateTime.UtcNow,
                FechaFin = DateTime.UtcNow.AddYears(1), // O AddMonths(1)
                TransaccionId = registro.TransaccionId
            };

            _context.SuscripcionesFacturacion.Add(suscripcion);

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "✅ Usuario {Id} creado y activado via PSE webhook", usuario.Id);
        }
    }
}
