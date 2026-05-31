using NubeeAPI.Models;
using NubeeAPI.Models.Impuestos;
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
            return await _http.GetByteArrayAsync($"/v2/bills/{numeroFactura}/download-pdf");
        }

        // ── Descargar XML ─────────────────────────────────────────────────
        public async Task<string> DescargarXmlAsync(string numeroFactura)
        {
            await AgregarAuthHeaderAsync();
             return await _http.GetStringAsync($"/v2/bills/{numeroFactura}/download-xml");
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

            // ── Teléfono ──────────────────────────────────────────────────────────
            var telefono = cliente.TelefonoFacturacion
                        ?? cliente.Telefonos?.FirstOrDefault()?.Numero
                        ?? "";

            // ── Tipo de persona ───────────────────────────────────────────────────
            bool esJuridica = cliente.TipoPersona?.Contains("ur", StringComparison.OrdinalIgnoreCase) == true;

            // ── Identificación: código DIAN ───────────────────────────────────────
            var tipoDocId = (cliente.TipoIdentificacion ?? "").ToUpper() switch
            {
                "CC" => 13,
                "NIT" => 31,
                "CE" => 22,
                "PASAPORTE" => 91,
                "TI" => 12,
                "PEP" => 47,
                "CÉDULA EXTRANJERA" or "DIE" => 22,
                _ => 13
            };

            // ── Nombres ───────────────────────────────────────────────────────────
            var company = esJuridica ? (cliente.NombreComercial ?? cliente.Nombre ?? "") : "";
            var tradeName = cliente.NombreComercial ?? cliente.Nombre ?? "";
            var names = esJuridica
                ? (cliente.NombreComercial ?? cliente.Nombre ?? "")
                : $"{cliente.Nombre} {cliente.Apellido}".Trim();

            // ── Municipio: usa el código guardado en el cliente ───────────────────
            // 🔴 FIX: ya no hardcodeado. Si el cliente no tiene municipio, usa 11001 (Bogotá) como fallback
            var municipioCodigo = !string.IsNullOrEmpty(cliente.CodigoMunicipio)
                ? cliente.CodigoMunicipio
                : "11001"; // Bogotá D.C. como fallback — nunca debe quedar vacío

            // ── Ítems ─────────────────────────────────────────────────────────────
            var items = f.DetalleFacturas!.Select((d, i) =>
            {
                var ivaLinea = d.Impuestos?
                    .FirstOrDefault(imp =>
                        imp.SnapshotCodigoDIAN == "01" ||
                        imp.TarifaImpuesto?.ImpuestoConcepto?.CodigoTributoDIAN == "01")
                    ?.TarifaUtilizada ?? 0m;

                var incLinea = d.Impuestos?
                    .FirstOrDefault(imp =>
                        imp.SnapshotCodigoDIAN == "04" ||
                        imp.TarifaImpuesto?.ImpuestoConcepto?.CodigoTributoDIAN == "04")
                    ?.TarifaUtilizada ?? 0m;

                var retenciones = d.Impuestos?
                    .Where(imp =>
                        imp.Naturaleza == NaturalezaFiscal.Retenido ||
                        imp.Naturaleza == NaturalezaFiscal.Autorretenido)
                    .Select(imp => new
                    {
                        code = imp.SnapshotCodigoDIAN ?? imp.TarifaImpuesto?.ImpuestoConcepto?.CodigoTributoDIAN,
                        rate = imp.TarifaUtilizada
                    })
                    .ToList();

                var unidadMedidaCodigo = int.TryParse(d.Producto?.UnidadMedida, out int umCodigo)
                    ? umCodigo
                    : 94;

                bool isExcluded = ivaLinea == 0 && incLinea == 0;

                var itemObj = new Dictionary<string, object>
                {
                    ["code_reference"] = d.Producto?.CodigoInterno ?? $"P{i + 1}",
                    ["name"] = d.Producto?.Nombre ?? d.Descripcion ?? $"Ítem {i + 1}",
                    ["quantity"] = d.Cantidad,
                    ["discount_rate"] = d.PorcentajeDescuento,
                    ["price"] = d.PrecioUnitario,
                    ["unit_measure_code"] = unidadMedidaCodigo,
                    ["standard_code"] = 999,
                    ["is_excluded"] = isExcluded
                };

                if (!isExcluded)
                {
                    var taxes = new List<object>();
                    if (ivaLinea > 0)
                        taxes.Add(new { code = "01", rate = ivaLinea });
                    if (incLinea > 0)
                        taxes.Add(new { code = "04", rate = incLinea });

                    itemObj["taxes"] = taxes;
                }

                if (retenciones.Any())
                    itemObj["withholding_taxes"] = retenciones;

                return itemObj;
            }).ToList();

            // ── Payload principal ─────────────────────────────────────────────────
            return new
            {
                // 🔴 FIX: campos obligatorios que faltaban
                reference_code = f.NumeroFactura,       // tu referencia interna única
                document = "01",                   // 01 = Factura de Venta
                numbering_range_id = f.FactusRangoId,
                operation_type = f.TipoOperacion,        // "10" = estándar, "09" = mandatos
                send_email = false,                  // manejar envío de email desde tu sistema

                // 🔴 FIX: payment_details como array (estructura correcta de Factus V2)
                payment_details = new[]
                {
            new
            {
                payment_form        = f.FormaPago,     // "1" contado | "2" crédito
                payment_method_code = f.MedioPago,     // "10", "42", "48", etc.
                amount              = f.TotalFactura,
                due_date            = (f.FechaVencimiento ?? f.FechaEmision)
                                        .ToString("yyyy-MM-dd")
            }
        },

                cash_rounding_amount = 0.00m,
                observation = f.Observaciones ?? "",

                // ── Cliente ───────────────────────────────────────────────────────
                customer = new
                {
                    identification_document_code = tipoDocId,
                    identification = cliente.NumeroIdentificacion,
                    dv = cliente.DigitoVerificacion?.ToString() ?? "",
                    company = company,
                    trade_name = tradeName,
                    names = names,
                    address = cliente.Direccion ?? "",
                    email = cliente.Correo ?? "",
                    phone = telefono,
                    legal_organization_code = esJuridica ? 1 : 2,  // 1=Jurídica, 2=Natural
                    tribute_code = cliente.CodigoTributo ?? "ZZ", // ZZ = No responsable IVA
                    municipality_code = municipioCodigo        // 🔴 FIX: dinámico, no hardcodeado
                },

                items
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
