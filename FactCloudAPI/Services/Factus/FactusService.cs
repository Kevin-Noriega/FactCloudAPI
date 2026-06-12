using Microsoft.Extensions.Options;
using NubeeAPI.Configuration;
using NubeeAPI.DTOs.Habilitacion;
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
    /// Diseñado para registrarse como Singleton:
    /// - Usa HttpClient de IHttpClientFactory.
    /// - Cachea el access_token y lo renueva automáticamente ante 401.
    /// - Soporta cancelación y reintento de red en operaciones idempotentes.
    /// </summary>
    public class FactusService : IFactusService
    {
        private readonly HttpClient _http;
        private readonly FactusOptions _opts;
        private readonly ILogger<FactusService> _logger;

        private static readonly JsonSerializerOptions JsonOpts =
            new() { PropertyNameCaseInsensitive = true };

        // Estado del token (protegido por semáforo)
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

            // Timeout razonable
            if (_http.Timeout == Timeout.InfiniteTimeSpan ||
                _http.Timeout > TimeSpan.FromSeconds(60))
            {
                _http.Timeout = TimeSpan.FromSeconds(60);
            }
        }

        // ═══════════ Autenticación OAuth2 (password grant) ════════════════
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
                _tokenExpiry = DateTime.UtcNow.AddSeconds(Math.Max(30, token.ExpiresIn - 60)); // margen
                _logger.LogInformation("Token Factus renovado, expira en ~{Seg}s", token.ExpiresIn);
                return _accessToken;
            }
            finally
            {
                _tokenLock.Release();
            }
        }

        private void InvalidarToken()
        {
            _accessToken = "";
            _tokenExpiry = DateTime.MinValue;
        }
        // ═══════════ Rangos de numeración ════════════════════════════════
        public async Task<FactusRangoActivoDto?> ObtenerRangoActivoAsync(
          int factusRangoId,
          CancellationToken ct = default)
        {
            _logger.LogInformation("Consultando rango Factus. Id={Id}", factusRangoId);

            using var resp = await EnviarAsync(
                HttpMethod.Get,
                $"/v2/numbering-ranges/{factusRangoId}",
                null,
                ct,
                reintentarRed: true);

            var body = await resp.Content.ReadAsStringAsync(ct);

            if (resp.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Rango Factus {Id} no encontrado.", factusRangoId);
                return null;
            }

            if (!resp.IsSuccessStatusCode)
            {
                _logger.LogError("Error consultando rango Factus {Id}: {Status} {Body}",
                    factusRangoId, resp.StatusCode, body);
                throw new FactusException(
                    $"Error consultando rango en Factus ({resp.StatusCode})",
                    body);
            }

            using var json = JsonDocument.Parse(body);
            if (!json.RootElement.TryGetProperty("data", out var dataEl))
                return null;

            var dto = new FactusRangoActivoDto
            {
                id = dataEl.GetProperty("id").GetInt32(),
                prefix = dataEl.GetProperty("prefix").GetString(),
                from = dataEl.GetProperty("from").GetInt64(),
                to = dataEl.GetProperty("to").GetInt64(),
                current = dataEl.GetProperty("current").GetInt64(),
                active = dataEl.GetProperty("active").GetBoolean()
            };

            return dto;
        }

        // ═══════════ Envío genérico con auth + refresh 401 ════════════════
        /// <summary>
        /// reintentarRed: solo para GET/operaciones idempotentes.
        /// </summary>
        private async Task<HttpResponseMessage> EnviarAsync(
            HttpMethod metodo,
            string ruta,
            object? cuerpo,
            CancellationToken ct,
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
                        "Fallo de red Factus {Metodo} {Ruta} (intento {Intento}). Reintentando en {Ms}ms",
                        metodo, ruta, intento, espera.TotalMilliseconds);
                    await Task.Delay(espera, ct);
                    continue;
                }

                if (resp.StatusCode == HttpStatusCode.Unauthorized && !tokenRefrescado)
                {
                    resp.Dispose();
                    _logger.LogWarning("Factus 401 en {Ruta}; refrescando token y reintentando", ruta);
                    InvalidarToken();
                    tokenRefrescado = true;
                    continue;
                }

                return resp;
            }
        }
        public async Task<bool> VerificarEmpresaAsync(
    string nit,
    CancellationToken ct = default)
        {
            try
            {
                using var resp = await EnviarAsync(
                    HttpMethod.Get,
                    "/v2/companies",
                    null,
                    ct,
                    reintentarRed: true);

                if (!resp.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Factus /v2/companies devolvió {Status}", resp.StatusCode);
                    return false;
                }

                var body = await resp.Content.ReadAsStringAsync(ct);

                using var json = JsonDocument.Parse(body);
                if (json.RootElement.TryGetProperty("data", out var data) &&
                    data.TryGetProperty("identification_number", out var idNumEl))
                {
                    var nitFactus = idNumEl.GetString()?.Replace("-", "") ?? "";
                    var nitLocal = nit.Replace("-", "");
                    return nitFactus == nitLocal;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo verificar empresa en Factus para NIT {Nit}", nit);
                return false;
            }
        }

        // ═══════════ Rangos de numeración ════════════════════════════════
        public async Task<int> RegistrarRangoAsync(
            ResolucionDIAN resolucion,
            Negocio negocio,
            CancellationToken ct = default)
        {
            // Según colección Factus V2:
            // document = 21 (Factura electrónica),
            // resolution_number = número de autorización DIAN,
            // from/to y technical_key también se pueden enviar.
            var payload = new
            {
                document = 21, // también acepta string "21" según ejemplos
                prefix = resolucion.Prefijo ?? "",
                from = resolucion.RangoDesde,
                to = resolucion.RangoHasta,
                resolution_number = resolucion.NumeroAutorizacion,
                start_date = resolucion.FechaInicio.ToString("yyyy-MM-dd"),
                end_date = resolucion.FechaFin.ToString("yyyy-MM-dd"),
                technical_key = resolucion.ClaveTecnica ?? ""
            };

            _logger.LogInformation(
                "Registrando rango Factus para NIT {Nit}, resolución {Res}",
                negocio.Nit, resolucion.NumeroAutorizacion);

            using var resp = await EnviarAsync(
                HttpMethod.Post,
                "/v2/numbering-ranges",
                payload,
                ct,
                reintentarRed: false);

            var contenido = await resp.Content.ReadAsStringAsync(ct);

            if (!resp.IsSuccessStatusCode)
            {
                _logger.LogError("Factus rango error {Status}: {Body}", resp.StatusCode, contenido);
                throw new FactusException($"Factus rechazó el rango ({resp.StatusCode})", contenido);
            }

            using var json = JsonDocument.Parse(contenido);
            var id = json.RootElement.GetProperty("data").GetProperty("id").GetInt32();

            _logger.LogInformation("Rango Factus registrado. Id={Id}, Prefijo={Prefijo}", id, resolucion.Prefijo);
            return id;
        }

        // (Opcional) Obtener rango por id
        public async Task<string?> ObtenerRangoAsync(int numberingRangeId, CancellationToken ct = default)
        {
            using var resp = await EnviarAsync(
                HttpMethod.Get,
                $"/v2/numbering-ranges/{numberingRangeId}",
                null,
                ct,
                reintentarRed: true);

            var body = await resp.Content.ReadAsStringAsync(ct);
            return resp.IsSuccessStatusCode ? body : null;
        }

        // ═══════════ Enviar factura (POST /v2/bills/validate) ═════════════
        public async Task<FactusRespuestaFactura> EnviarFacturaAsync(Factura factura, CancellationToken ct = default)
        {
            var payload = MapearFactura(factura);

            using var resp = await EnviarAsync(
                HttpMethod.Post,
                "/v2/bills/validate",
                payload,
                ct,
                reintentarRed: false); // NO reintentar, no es idempotente

            var contenido = await resp.Content.ReadAsStringAsync(ct);

            if (!resp.IsSuccessStatusCode)
            {
                _logger.LogError("Factus factura error {Status}: {Body}", resp.StatusCode, contenido);
                throw new FactusException($"Factus rechazó la factura ({resp.StatusCode})", contenido);
            }

            return JsonSerializer.Deserialize<FactusRespuestaFactura>(contenido, JsonOpts)
                   ?? throw new FactusException("Factus devolvió respuesta vacía", contenido);
        }

        // ═══════════ Descargar PDF / XML ═════════════════════════════════
        public async Task<byte[]> DescargarPdfAsync(string numeroFactus, CancellationToken ct = default)
        {
            using var resp = await EnviarAsync(
                HttpMethod.Get,
                $"/v2/bills/{numeroFactus}/download-pdf",
                null,
                ct,
                reintentarRed: true);

            if (!resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadAsStringAsync(ct);
                throw new FactusException($"Factus no entregó PDF {numeroFactus} ({resp.StatusCode})", body);
            }

            return await resp.Content.ReadAsByteArrayAsync(ct);
        }

        public async Task<string> DescargarXmlAsync(string numeroFactus, CancellationToken ct = default)
        {
            using var resp = await EnviarAsync(
                HttpMethod.Get,
                $"/v2/bills/{numeroFactus}/download-xml",
                null,
                ct,
                reintentarRed: true);

            var body = await resp.Content.ReadAsStringAsync(ct);

            if (!resp.IsSuccessStatusCode)
                throw new FactusException($"Factus no entregó XML {numeroFactus} ({resp.StatusCode})", body);

            return body;
        }

        // ═══════════ Consultar / listar / eliminar facturas ══════════════
        public async Task<string> ConsultarFacturaAsync(string numeroFactus, CancellationToken ct = default)
        {
            using var resp = await EnviarAsync(
                HttpMethod.Get,
                $"/v2/bills/{numeroFactus}",
                null,
                ct,
                reintentarRed: true);

            var body = await resp.Content.ReadAsStringAsync(ct);

            if (!resp.IsSuccessStatusCode)
                throw new FactusException($"Factus no encontró factura {numeroFactus} ({resp.StatusCode})", body);

            return body;
        }

        public async Task<string> ConsultarEstadoAsync(string numeroFactus, CancellationToken ct = default)
        {
            var contenido = await ConsultarFacturaAsync(numeroFactus, ct);
            try
            {
                using var json = JsonDocument.Parse(contenido);
                if (json.RootElement.TryGetProperty("data", out var data))
                {
                    if (data.TryGetProperty("bill", out var bill)
                        && bill.TryGetProperty("status", out var st1))
                        return st1.ToString();

                    if (data.TryGetProperty("status", out var st2))
                        return st2.ToString();
                }
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "No se pudo parsear estado de factura {Numero}", numeroFactus);
            }

            return "";
        }

        public async Task<string> ListarFacturasAsync(string? query = null, CancellationToken ct = default)
        {
            var ruta = "/v2/bills";
            if (!string.IsNullOrWhiteSpace(query))
                ruta += query.StartsWith('?') ? query : "?" + query;

            using var resp = await EnviarAsync(HttpMethod.Get, ruta, null, ct, reintentarRed: true);
            var body = await resp.Content.ReadAsStringAsync(ct);

            if (!resp.IsSuccessStatusCode)
                throw new FactusException($"Factus rechazó el listado ({resp.StatusCode})", body);

            return body;
        }

        public async Task<bool> EliminarFacturaNoValidadaAsync(string referenceCode, CancellationToken ct = default)
        {
            using var resp = await EnviarAsync(
                HttpMethod.Delete,
                $"/v2/bills/destroy/reference/{referenceCode}",
                null,
                ct);

            if (resp.IsSuccessStatusCode) return true;

            var body = await resp.Content.ReadAsStringAsync(ct);
            _logger.LogWarning("Factus no pudo eliminar factura {Ref}: {Status} {Body}",
                referenceCode, resp.StatusCode, body);
            return false;
        }

        // ═══════════ Mapeo Factura → payload Factus ══════════════════════
        public static object MapearFactura(Factura f)
        {
            if (f.Cliente is null)
                throw new InvalidOperationException("La factura no tiene Cliente cargado.");
            if (f.DetalleFacturas is null || f.DetalleFacturas.Count == 0)
                throw new InvalidOperationException("La factura no tiene ítems.");

            var cliente = f.Cliente;

            var telefono = cliente.TelefonoFacturacion
                           ?? cliente.Telefonos?.FirstOrDefault()?.Numero
                           ?? "";

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
                : "11001"; // Bogotá D.C. por defecto

            var items = f.DetalleFacturas.Select((d, i) =>
            {
                var ivaLinea = d.Impuestos?
                    .FirstOrDefault(imp =>
                        imp.SnapshotCodigoDIAN == "01" ||
                        imp.TarifaImpuesto?.ImpuestoConcepto?.CodigoTributoDIAN == "01")
                    ?.TarifaUtilizada ?? 0m;

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
                    ["payment_form"] = f.FormaPago,    // "1" contado | "2" crédito
                    ["payment_method_code"] = metodo,  // "10", "42", etc.
                    ["amount"] = monto
                };
                if (esCredito) p["due_date"] = dueDate;
                return p;
            }

            var payments = (f.FormasPago is { Count: > 0 })
                ? f.FormasPago.Select(fp => ConstruirPago(fp.MetodoPagoCodigo, fp.Valor)).ToArray()
                : new[] { ConstruirPago(f.MedioPago, f.TotalFactura) };

            return new
            {
                reference_code = string.IsNullOrEmpty(f.NumeroFactura)
                    ? $"FAC-{f.Id}"
                    : $"{f.Prefijo}{f.NumeroFactura}-{f.Id}",
                document = "01",                       // 01 = Factura de venta
                numbering_range_id = f.FactusRangoId,
                operation_type = f.TipoOperacion,      // "10" estándar, "09" mandatos, etc.
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
                    legal_organization_code = esJuridica ? 1 : 2,
                    tribute_code = cliente.CodigoTributo ?? "ZZ",
                    municipality_code = municipioCodigo
                },

                items
            };
        }
    }

    // ═══════════ DTOs de token y factura ════════════════════════════════
  

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

    public class FactusException : Exception
    {
        public string FactusBody { get; }
        public FactusException(string message, string factusBody) : base(message)
            => FactusBody = factusBody;
    }
}