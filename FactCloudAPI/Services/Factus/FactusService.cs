using NubeeAPI.Models;
using NubeeAPI.Models.Usuarios;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NubeeAPI.Services.Factus
{
    public class FactusService : IFactusService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;
        private readonly ILogger<FactusService> _logger;

        private string _accessToken = "";
        private DateTime _tokenExpiry = DateTime.MinValue;
        private readonly SemaphoreSlim _tokenLock = new(1, 1);

        public FactusService(
            IHttpClientFactory factory,
            IConfiguration config,
            ILogger<FactusService> logger)
        {
            _http = factory.CreateClient("Factus");
            _config = config;
            _logger = logger;
        }

        // ── Autenticación OAuth2 password grant ───────────────────────────
        private async Task<string> ObtenerTokenAsync()
        {
            await _tokenLock.WaitAsync();
            try
            {
                if (!string.IsNullOrEmpty(_accessToken) && DateTime.UtcNow < _tokenExpiry)
                    return _accessToken;

                var body = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["grant_type"] = "password",
                    ["client_id"] = _config["Factus:ClientId"]!,
                    ["client_secret"] = _config["Factus:ClientSecret"]!,
                    ["username"] = _config["Factus:Username"]!,
                    ["password"] = _config["Factus:Password"]!
                });

                var resp = await _http.PostAsync("/oauth/token", body);
                var contenido = await resp.Content.ReadAsStringAsync();

                if (!resp.IsSuccessStatusCode)
                    throw new Exception($"Factus auth error {resp.StatusCode}: {contenido}");

                var token = JsonSerializer.Deserialize<FactusTokenResponse>(contenido,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

                _accessToken = token.AccessToken;
                _tokenExpiry = DateTime.UtcNow.AddSeconds(token.ExpiresIn - 60);
                return _accessToken;
            }
            finally { _tokenLock.Release(); }
        }

        private async Task AgregarAuthHeaderAsync()
        {
            var token = await ObtenerTokenAsync();
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        // ── Registrar rango de numeración ─────────────────────────────────
        public async Task<int> RegistrarRangoAsync(ResolucionDIAN resolucion, Negocio negocio)
        {
            await AgregarAuthHeaderAsync();

            var payload = new
            {
                document = "01",   // 01 = Factura electrónica de venta
                prefix = resolucion.Prefijo ?? "",
                from = resolucion.RangoDesde,
                to = resolucion.RangoHasta,
                resolution_number = resolucion.NumeroAutorizacion,
                start_date = resolucion.FechaInicio.ToString("yyyy-MM-dd"),
                end_date = resolucion.FechaFin.ToString("yyyy-MM-dd"),
                technical_key = resolucion.ClaveTecnica ?? ""
            };

            var resp = await _http.PostAsJsonAsync("/v2/numbering-ranges", payload);
            var contenido = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
            {
                _logger.LogError("Factus rango error {Status}: {Body}", resp.StatusCode, contenido);
                throw new Exception($"Factus rechazó el rango: {contenido}");
            }

            var json = JsonSerializer.Deserialize<JsonElement>(contenido);
            return json.GetProperty("data").GetProperty("id").GetInt32();
        }

        // ── Enviar factura ────────────────────────────────────────────────
        public async Task<FactusRespuestaFactura> EnviarFacturaAsync(Factura factura)
        {
            await AgregarAuthHeaderAsync();

            var payload = MapearFactura(factura);
            var resp = await _http.PostAsJsonAsync("/v2/bills/validate", payload);
            var contenido = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
            {
                _logger.LogError("Factus factura error {Status}: {Body}", resp.StatusCode, contenido);
                throw new FactusException($"Factus rechazó la factura: {contenido}", contenido);
            }

            return JsonSerializer.Deserialize<FactusRespuestaFactura>(contenido,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        // ── Descargar PDF ─────────────────────────────────────────────────
        public async Task<byte[]> DescargarPdfAsync(string numeroFactura)
        {
            await AgregarAuthHeaderAsync();
            return await _http.GetByteArrayAsync($"/v2/bills/download-pdf/{numeroFactura}");
        }

        // ── Descargar XML ─────────────────────────────────────────────────
        public async Task<string> DescargarXmlAsync(string numeroFactura)
        {
            await AgregarAuthHeaderAsync();
            return await _http.GetStringAsync($"/v2/bills/download-xml/{numeroFactura}");
        }

        // ── Consultar estado ──────────────────────────────────────────────
        public async Task<string> ConsultarEstadoAsync(string cufe)
        {
            await AgregarAuthHeaderAsync();
            var resp = await _http.GetAsync($"/v2/bills/status/{cufe}");
            return await resp.Content.ReadAsStringAsync();
        }

        // ── Mapeo Factura → payload Factus V2 ─────────────────────────────
        private static object MapearFactura(Factura f)
        {
            var cliente = f.Cliente!;

            // Teléfono: primero TelefonoFacturacion, luego el primero de la colección
            var telefono = cliente.TelefonoFacturacion
                        ?? cliente.Telefonos.FirstOrDefault()?.Numero
                        ?? "";

            // Para Factus: empresa = razón social si es Jurídica
            bool esJuridica = cliente.TipoPersona.Contains("ur", StringComparison.OrdinalIgnoreCase);
            var nombreEmpresa = esJuridica
                ? (cliente.NombreComercial ?? cliente.Nombre)
                : "";
            var nombres = esJuridica
                ? (cliente.NombreComercial ?? cliente.Nombre)
                : $"{cliente.Nombre} {cliente.Apellido}".Trim();

            // Código DIAN tipo identificación
            var tipoDocId = cliente.TipoIdentificacion?.ToUpper() switch
            {
                "CC" => 13,
                "NIT" => 31,
                "CE" => 22,
                "PASAPORTE" => 91,
                "TI" => 12,
                _ => 13
            };

            return new
            {
                numbering_range_id = f.FactusRangoId,
                reference_code = f.NumeroFactura,
                observation = f.Observaciones ?? "",
                payment_method_code = f.MedioPago,
                payment_due_date = (f.FechaVencimiento ?? f.FechaEmision).ToString("yyyy-MM-dd"),

                customer = new
                {
                    identification = cliente.NumeroIdentificacion,
                    dv = cliente.DigitoVerificacion?.ToString() ?? "",
                    company = nombreEmpresa,
                    trade_name = cliente.NombreComercial ?? cliente.Nombre ?? "",
                    names = nombres,
                    address = cliente.Direccion ?? "",
                    email = cliente.Correo ?? "",
                    phone = telefono,
                    identification_document = tipoDocId,
                    municipality_id = 980   // ← temporal, ver Opción 2
                },

                items = f.DetalleFacturas!.Select((d, i) =>
                {
                    var ivaLinea = d.Impuestos?
                        .FirstOrDefault(imp => imp.SnapshotCodigoDIAN == "01")
                        ?.TarifaImpuesto?.Tarifa ?? 0m;

                    return new
                    {
                        code_reference = d.Producto?.CodigoInterno ?? $"P{i + 1}",
                        name = d.Producto?.Nombre ?? d.Descripcion ?? $"Ítem {i + 1}",
                        quantity = d.Cantidad,
                        discount_rate = d.PorcentajeDescuento,
                        price = d.PrecioUnitario,
                        tax_rate = ivaLinea.ToString("0.00"),
                        unit_measure_id = d.Producto?.UnidadMedida,
                        standard_code_id = 1,
                        is_excluded = ivaLinea == 0 ? 1 : 0,
                        tribute_id = ivaLinea > 0 ? 1 : 22
                    };
                }).ToList()
            };
        }
    }

    // ── DTOs de respuesta Factus ──────────────────────────────────────────
    public class FactusTokenResponse
    {
        [JsonPropertyName("access_token")] public string AccessToken { get; set; } = "";
        [JsonPropertyName("expires_in")] public int ExpiresIn { get; set; }
        [JsonPropertyName("token_type")] public string TokenType { get; set; } = "";
    }

    public class FactusRespuestaFactura
    {
        [JsonPropertyName("data")] public FactusFacturaData? Data { get; set; }
    }

    public class FactusFacturaData
    {
        [JsonPropertyName("cufe")] public string? Cufe { get; set; }
        [JsonPropertyName("number")] public string? Number { get; set; }
        [JsonPropertyName("qr")] public string? Qr { get; set; }
        [JsonPropertyName("status")] public string? Status { get; set; }
    }

    // ── Excepción personalizada con el body de Factus ─────────────────────
    public class FactusException : Exception
    {
        public string FactusBody { get; }
        public FactusException(string message, string factusBody) : base(message)
            => FactusBody = factusBody;
    }
}
