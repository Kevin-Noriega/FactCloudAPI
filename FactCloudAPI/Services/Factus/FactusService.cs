using Microsoft.Extensions.Options;
using NubeeAPI.Configuration;
using NubeeAPI.Models;
using NubeeAPI.Models.Impuestos;
using NubeeAPI.Models.Usuarios;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NubeeAPI.Services.Factus
{
    /// <summary>
    /// Cliente de la API Factus V2 (https://developers.factus.com.co).
    /// Diseñado para registrarse como Singleton: NO muta el HttpClient compartido
    /// (cada request lleva su propio encabezado Authorization vía HttpRequestMessage),
    /// cachea el access_token y lo refresca automáticamente ante un 401.
    /// </summary>
    public class FactusService : IFactusService
    {
        private readonly HttpClient _http;
        private readonly FactusOptions _opts;
        private readonly ILogger<FactusService> _logger;

        private static readonly JsonSerializerOptions JsonOpts =
            new() { PropertyNameCaseInsensitive = true };

        // Estado del token (protegido por el semáforo)
        private string _accessToken = "";
        private DateTime _tokenExpiry = DateTime.MinValue;
        private readonly SemaphoreSlim _tokenLock = new(1, 1);

        public FactusService(
            IHttpClientFactory factory,
            IOptions<FactusOptions> options,
            ILogger<FactusService> logger)
        {
            _http = factory.CreateClient("Factus");
            _opts = options.Value;
            _logger = logger;

            if (_http.Timeout == System.Threading.Timeout.InfiniteTimeSpan ||
                _http.Timeout > TimeSpan.FromSeconds(60))
            {
                _http.Timeout = TimeSpan.FromSeconds(60);
            }
        }

        // ══ Autenticación OAuth2 (password grant) ═════════════════════════
        private async Task<string> ObtenerTokenAsync(CancellationToken ct)
        {
            await _tokenLock.WaitAsync(ct);
            try
            {
                if (!string.IsNullOrEmpty(_accessToken) && DateTime.UtcNow < _tokenExpiry)
                    return _accessToken;

                using var body = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["grant_type"] = "password",
                    ["client_id"] = _opts.ClientId,
                    ["client_secret"] = _opts.ClientSecret,
                    ["username"] = _opts.Username,
                    ["password"] = _opts.Password
                });

                using var req = new HttpRequestMessage(HttpMethod.Post, "/oauth/token") { Content = body };
                req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using var resp = await _http.SendAsync(req, ct);
                var contenido = await resp.Content.ReadAsStringAsync(ct);

                if (!resp.IsSuccessStatusCode)
                {
                    _logger.LogError("Factus auth error {Status}: {Body}", resp.StatusCode, contenido);
                    throw new FactusException($"Factus auth error {resp.StatusCode}", contenido);
                }

                var token = JsonSerializer.Deserialize<FactusTokenResponse>(contenido, JsonOpts)
                            ?? throw new FactusException("Factus devolvió un token vacío", contenido);

                _accessToken = token.AccessToken;
                // margen de 60s para evitar usar un token a punto de expirar
                _tokenExpiry = DateTime.UtcNow.AddSeconds(Math.Max(30, token.ExpiresIn - 60));
                _logger.LogInformation("Token Factus renovado, expira en ~{Seg}s", token.ExpiresIn);
                return _accessToken;
            }
            finally { _tokenLock.Release(); }
        }

        private void InvalidarToken()
        {
            // No requiere lock: peor caso, una renovación de más.
            _tokenExpiry = DateTime.MinValue;
            _accessToken = "";
        }

        // ══ Envío genérico con auth por-request + refresh ante 401 ════════
        // reintentarRed: solo para operaciones idempotentes (GET). NUNCA para POST validate.
        private async Task<HttpResponseMessage> EnviarAsync(
            HttpMethod metodo, string ruta, object? cuerpo, CancellationToken ct,
            bool reintentarRed = false)
        {
            const int maxIntentosRed = 3;
            int intento = 0;
            bool tokenRefrescado = false;

            while (true)
            {
                intento++;
                var token = await ObtenerTokenAsync(ct);

                using var req = new HttpRequestMessage(metodo, ruta);
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (cuerpo is not null)
                    req.Content = JsonContent.Create(cuerpo);

                HttpResponseMessage resp;
                try
                {
                    resp = await _http.SendAsync(req, ct);
                }
                catch (Exception ex) when (
                    (ex is HttpRequestException || ex is TaskCanceledException) &&
                    !ct.IsCancellationRequested && reintentarRed && intento < maxIntentosRed)
                {
                    var espera = TimeSpan.FromMilliseconds(300 * Math.Pow(2, intento - 1));
                    _logger.LogWarning(ex,
                        "Fallo de red llamando a Factus {Metodo} {Ruta} (intento {Intento}). Reintentando en {Ms}ms",
                        metodo, ruta, intento, espera.TotalMilliseconds);
                    await Task.Delay(espera, ct);
                    continue;
                }

                // Token expirado/revocado → refrescar una sola vez y reintentar
                if (resp.StatusCode == HttpStatusCode.Unauthorized && !tokenRefrescado)
                {
                    resp.Dispose();
                    _logger.LogWarning("Factus respondió 401; refrescando token y reintentando {Ruta}", ruta);
                    InvalidarToken();
                    tokenRefrescado = true;
                    continue;
                }

                return resp;
            }
        }

        // ══ Registrar rango de numeración ═════════════════════════════════
        public async Task<int> RegistrarRangoAsync(ResolucionDIAN resolucion, Negocio negocio, CancellationToken ct = default)
        {
            // Según la colección Factus V2, para crear un rango de FACTURA electrónica
            // el código de documento es "21" (NO "01", que es el código del documento factura).
            var payload = new
            {
                document = "21",                       // 21 = Factura electrónica de venta
                prefix = resolucion.Prefijo ?? "",
                from = resolucion.RangoDesde,
                to = resolucion.RangoHasta,
                resolution_number = resolucion.NumeroAutorizacion,
                start_date = resolucion.FechaInicio.ToString("yyyy-MM-dd"),
                end_date = resolucion.FechaFin.ToString("yyyy-MM-dd"),
                technical_key = resolucion.ClaveTecnica ?? ""
            };

            using var resp = await EnviarAsync(HttpMethod.Post, "/v2/numbering-ranges", payload, ct);
            var contenido = await resp.Content.ReadAsStringAsync(ct);

            if (!resp.IsSuccessStatusCode)
            {
                _logger.LogError("Factus rango error {Status}: {Body}", resp.StatusCode, contenido);
                throw new FactusException($"Factus rechazó el rango ({resp.StatusCode})", contenido);
            }

            using var json = JsonDocument.Parse(contenido);
            return json.RootElement.GetProperty("data").GetProperty("id").GetInt32();
        }

        // ══ Enviar factura (POST /v2/bills/validate) ══════════════════════
        public async Task<FactusRespuestaFactura> EnviarFacturaAsync(Factura factura, CancellationToken ct = default)
        {
            var payload = MapearFactura(factura);

            // POST validate NO es idempotente → sin reintento de red automático.
            using var resp = await EnviarAsync(HttpMethod.Post, "/v2/bills/validate", payload, ct);
            var contenido = await resp.Content.ReadAsStringAsync(ct);

            if (!resp.IsSuccessStatusCode)
            {
                _logger.LogError("Factus factura error {Status}: {Body}", resp.StatusCode, contenido);
                throw new FactusException($"Factus rechazó la factura ({resp.StatusCode})", contenido);
            }

            return JsonSerializer.Deserialize<FactusRespuestaFactura>(contenido, JsonOpts)
                   ?? throw new FactusException("Factus devolvió una respuesta vacía", contenido);
        }

        // ══ Descargar PDF ═════════════════════════════════════════════════
        public async Task<byte[]> DescargarPdfAsync(string numeroFactus, CancellationToken ct = default)
        {
            using var resp = await EnviarAsync(HttpMethod.Get, $"/v2/bills/{numeroFactus}/download-pdf", null, ct, reintentarRed: true);
            if (!resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadAsStringAsync(ct);
                throw new FactusException($"Factus no entregó el PDF de {numeroFactus} ({resp.StatusCode})", body);
            }
            return await resp.Content.ReadAsByteArrayAsync(ct);
        }

        // ══ Descargar XML ═════════════════════════════════════════════════
        public async Task<string> DescargarXmlAsync(string numeroFactus, CancellationToken ct = default)
        {
            using var resp = await EnviarAsync(HttpMethod.Get, $"/v2/bills/{numeroFactus}/download-xml", null, ct, reintentarRed: true);
            if (!resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadAsStringAsync(ct);
                throw new FactusException($"Factus no entregó el XML de {numeroFactus} ({resp.StatusCode})", body);
            }
            return await resp.Content.ReadAsStringAsync(ct);
        }

        // ══ Consultar factura (GET /v2/bills/{number}) ════════════════════
        public async Task<string> ConsultarFacturaAsync(string numeroFactus, CancellationToken ct = default)
        {
            using var resp = await EnviarAsync(HttpMethod.Get, $"/v2/bills/{numeroFactus}", null, ct, reintentarRed: true);
            var contenido = await resp.Content.ReadAsStringAsync(ct);
            if (!resp.IsSuccessStatusCode)
                throw new FactusException($"Factus no encontró la factura {numeroFactus} ({resp.StatusCode})", contenido);
            return contenido;
        }

        public async Task<string> ConsultarEstadoAsync(string numeroFactus, CancellationToken ct = default)
        {
            var contenido = await ConsultarFacturaAsync(numeroFactus, ct);
            try
            {
                using var json = JsonDocument.Parse(contenido);
                // La factura puede venir como data.bill.status o data.status según el endpoint.
                if (json.RootElement.TryGetProperty("data", out var data))
                {
                    if (data.TryGetProperty("bill", out var bill) &&
                        bill.TryGetProperty("status", out var st1))
                        return st1.ToString();
                    if (data.TryGetProperty("status", out var st2))
                        return st2.ToString();
                }
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "No se pudo parsear el estado de la factura {Numero}", numeroFactus);
            }
            return "";
        }

        // ══ Listar facturas ═══════════════════════════════════════════════
        public async Task<string> ListarFacturasAsync(string? queryString = null, CancellationToken ct = default)
        {
            var ruta = "/v2/bills";
            if (!string.IsNullOrWhiteSpace(queryString))
                ruta += queryString.StartsWith('?') ? queryString : "?" + queryString;

            using var resp = await EnviarAsync(HttpMethod.Get, ruta, null, ct, reintentarRed: true);
            var contenido = await resp.Content.ReadAsStringAsync(ct);
            if (!resp.IsSuccessStatusCode)
                throw new FactusException($"Factus rechazó el listado ({resp.StatusCode})", contenido);
            return contenido;
        }

        // ══ Eliminar factura no validada ══════════════════════════════════
        public async Task<bool> EliminarFacturaNoValidadaAsync(string referenceCode, CancellationToken ct = default)
        {
            using var resp = await EnviarAsync(
                HttpMethod.Delete, $"/v2/bills/destroy/reference/{referenceCode}", null, ct);

            if (resp.IsSuccessStatusCode) return true;

            var body = await resp.Content.ReadAsStringAsync(ct);
            _logger.LogWarning("Factus no pudo eliminar la factura {Ref}: {Status} {Body}",
                referenceCode, resp.StatusCode, body);
            return false;
        }

        // ══ Mapeo Factura → payload Factus V2 ═════════════════════════════
        public static object MapearFactura(Factura f)
        {
            if (f.Cliente is null)
                throw new InvalidOperationException("La factura no tiene Cliente cargado; inclúyelo antes de enviar a Factus.");
            if (f.DetalleFacturas is null || f.DetalleFacturas.Count == 0)
                throw new InvalidOperationException("La factura no tiene ítems (DetalleFacturas) para enviar a Factus.");

            var cliente = f.Cliente;

            var telefono = cliente.TelefonoFacturacion
                        ?? cliente.Telefonos?.FirstOrDefault()?.Numero
                        ?? "";

            // ⚠️ OJO: "Natural" también contiene "ur" → no se puede buscar "ur".
            // Jurídica se detecta por "jur" (Jurídica/Juridica) o el código DIAN "1".
            bool esJuridica =
                cliente.TipoPersona?.Contains("jur", StringComparison.OrdinalIgnoreCase) == true
                || cliente.TipoPersona?.Trim() == "1";

            var tipoDocId = (cliente.TipoIdentificacion ?? "").ToUpperInvariant() switch
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

            var company = esJuridica ? (cliente.NombreComercial ?? cliente.Nombre ?? "") : "";
            var tradeName = cliente.NombreComercial ?? cliente.Nombre ?? "";
            var names = esJuridica
                ? (cliente.NombreComercial ?? cliente.Nombre ?? "")
                : $"{cliente.Nombre} {cliente.Apellido}".Trim();

            var municipioCodigo = !string.IsNullOrEmpty(cliente.CodigoMunicipio)
                ? cliente.CodigoMunicipio
                : "11001"; // Bogotá D.C. como fallback — nunca debe quedar vacío

            var items = f.DetalleFacturas.Select((d, i) =>
            {
                var ivaLinea = d.Impuestos?
                    .FirstOrDefault(imp =>
                        imp.SnapshotCodigoDIAN == "01" ||
                        imp.TarifaImpuesto?.ImpuestoConcepto?.CodigoTributoDIAN == "01")
                    ?.TarifaUtilizada ?? 0m;

                // INC = código 04. Se exige Naturaleza Trasladado porque ReteIVA también
                // usa el código 04 en el seed; sin este filtro una retención se leería como INC.
                var incLinea = d.Impuestos?
                    .FirstOrDefault(imp =>
                        imp.Naturaleza == NaturalezaFiscal.Trasladado &&
                        (imp.SnapshotCodigoDIAN == "04" ||
                         imp.TarifaImpuesto?.ImpuestoConcepto?.CodigoTributoDIAN == "04"))
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
                    if (ivaLinea > 0) taxes.Add(new { code = "01", rate = ivaLinea });
                    if (incLinea > 0) taxes.Add(new { code = "04", rate = incLinea });
                    itemObj["taxes"] = taxes;
                }

                if (retenciones is { Count: > 0 })
                    itemObj["withholding_taxes"] = retenciones;

                return itemObj;
            }).ToList();

            bool esCredito = f.FormaPago == "2";
            var dueDate = (f.FechaVencimiento ?? f.FechaEmision).ToString("yyyy-MM-dd");

            Dictionary<string, object> ConstruirPago(string metodo, decimal monto)
            {
                var p = new Dictionary<string, object>
                {
                    ["payment_form"] = f.FormaPago,    // "1" contado | "2" crédito (nivel factura)
                    ["payment_method_code"] = metodo,  // "10", "42", "48", etc.
                    ["amount"] = monto
                };
                // due_date sólo aplica a crédito (DIAN/Factus no lo exige en contado)
                if (esCredito) p["due_date"] = dueDate;
                return p;
            }

            // Desglose de pagos del frontend; si no hay, un único pago con el total.
            var payments = (f.FormasPago is { Count: > 0 })
                ? f.FormasPago.Select(fp => ConstruirPago(fp.MetodoPagoCodigo, fp.Valor)).ToArray()
                : new[] { ConstruirPago(f.MedioPago, f.TotalFactura) };

            return new
            {
                // reference_code DEBE ser único por documento en Factus.
                // Combinamos número + Id para garantizar unicidad y permitir reenvío idempotente.
                reference_code = string.IsNullOrEmpty(f.NumeroFactura)
                    ? $"FAC-{f.Id}"
                    : $"{f.Prefijo}{f.NumeroFactura}-{f.Id}",
                document = "01",                       // 01 = Factura de Venta
                numbering_range_id = f.FactusRangoId,
                operation_type = f.TipoOperacion,      // "10" = estándar, "09" = mandatos
                send_email = false,

                payment_details = payments,

                cash_rounding_amount = 0.00m,
                observation = f.Observaciones ?? "",

                customer = new
                {
                    identification_document_code = tipoDocId,
                    identification = cliente.NumeroIdentificacion,
                    dv = cliente.DigitoVerificacion?.ToString() ?? "",
                    company,
                    trade_name = tradeName,
                    names,
                    address = cliente.Direccion ?? "",
                    email = cliente.Correo ?? "",
                    phone = telefono,
                    legal_organization_code = esJuridica ? 1 : 2,  // 1=Jurídica, 2=Natural
                    tribute_code = cliente.CodigoTributo ?? "ZZ",  // ZZ = No responsable IVA
                    municipality_code = municipioCodigo
                },

                items
            };
        }
    }

    // ══ DTOs de respuesta Factus ══════════════════════════════════════════
    public class FactusTokenResponse
    {
        [JsonPropertyName("access_token")] public string AccessToken { get; set; } = "";
        [JsonPropertyName("expires_in")] public int ExpiresIn { get; set; }
        [JsonPropertyName("token_type")] public string TokenType { get; set; } = "";
        [JsonPropertyName("refresh_token")] public string? RefreshToken { get; set; }
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

        // En /v2/bills/validate Factus anida la factura bajo "bill".
        [JsonPropertyName("bill")] public FactusBill? Bill { get; set; }
    }

    public class FactusBill
    {
        [JsonPropertyName("number")] public string? Number { get; set; }
        [JsonPropertyName("cufe")] public string? Cufe { get; set; }
        [JsonPropertyName("qr")] public string? Qr { get; set; }
        [JsonPropertyName("status")] public string? Status { get; set; }
        [JsonPropertyName("public_url")] public string? PublicUrl { get; set; }
    }

    // ══ Excepción personalizada con el body de Factus ═════════════════════
    public class FactusException : Exception
    {
        public string FactusBody { get; }
        public FactusException(string message, string factusBody) : base(message)
            => FactusBody = factusBody;
    }
}
