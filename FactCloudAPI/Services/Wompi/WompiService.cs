using EllipticCurve;
using FactCloudAPI.Models.Wompi;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace FactCloudAPI.Services.Wompi
{
    public class WompiService : IWompiService
    {
            private readonly HttpClient _httpClient;
            private readonly IConfiguration _config;

            // ✅ Campos privados que faltaban en la primera versión
            private readonly string _publicKey;
            private readonly string _privateKey;
            private readonly string _baseUrl;

            public WompiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
            _publicKey  = config["Wompi:PublicKey"]?.Trim()  ?? throw new ArgumentNullException("Wompi:PublicKey");
            _privateKey = config["Wompi:PrivateKey"]?.Trim() ?? throw new ArgumentNullException("Wompi:PrivateKey");
            _baseUrl    = config["Wompi:BaseUrl"]?.Trim()    ?? "https://sandbox.wompi.co/v1";
        }

        // ─── ACCEPTANCE TOKEN ───────────────────────
        public async Task<WompiAcceptanceTokenResponse> GetAcceptanceTokenAsync()
        {
            var url = $"{_baseUrl}/merchants/{_publicKey}";
            Console.WriteLine($"🔍 URL: {url}");

            // Sin Authorization — este endpoint es público
            _httpClient.DefaultRequestHeaders.Clear();

            var response = await _httpClient.GetAsync(url);
            var content  = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"🔍 StatusCode: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error obteniendo acceptance token: {content}");

            var wompiResponse = JsonSerializer.Deserialize<WompiMerchantResponse>(
                content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                }
            ) ?? throw new Exception("Respuesta vacía de Wompi");

            Console.WriteLine("✅ Token obtenido correctamente");

            return new WompiAcceptanceTokenResponse
            {
                Data = new AcceptanceData
                {
                    PresignedAcceptance = wompiResponse.Data.PresignedAcceptance
                }
            };
        }

        // ─── TOKENIZAR TARJETA ──────────────────────
        public async Task<string> TokenizeCardAsync(CardData cardData)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _publicKey);

            var payload = new
            {
                number      = cardData.Number,
                cvc         = cardData.Cvc,
                exp_month   = cardData.ExpMonth,
                exp_year    = cardData.ExpYear,
                card_holder = cardData.CardHolder
            };

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync($"{_baseUrl}/tokens/cards", content);
            var result   = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error tokenizando tarjeta: {result}");

            var tokenResponse = JsonSerializer.Deserialize<JsonElement>(result);
            return tokenResponse.GetProperty("data").GetProperty("id").GetString()
                ?? throw new Exception("Token de tarjeta vacío");
        }

        // ─── TRANSACCIÓN TARJETA ────────────────────
        public async Task<WompiTransactionResponse> CreateCardTransactionAsync(
            WompiTransactionRequest request)
        {
            var integrityKey = _config["Wompi:IntegrityKey"]
                ?? throw new Exception("Wompi:IntegrityKey no configurada");

            var signature = GenerateIntegritySignature(
                request.Reference,
                request.AmountInCents,
                request.Currency,
                integrityKey
            );

            var payload = new
            {
                acceptance_token = request.AcceptanceToken,
                amount_in_cents  = request.AmountInCents,
                currency         = request.Currency,
                customer_email   = request.CustomerEmail,
                payment_method   = new
                {
                    type         = request.PaymentMethod.Type,
                    token        = request.PaymentMethod.Token,
                    installments = request.PaymentMethod.Installments
                },
                reference     = request.Reference,
                customer_data = new
                {
                    full_name     = request.CustomerData.FullName,
                    phone_number  = request.CustomerData.PhoneNumber,
                    legal_id      = request.CustomerData.LegalId.Number,
                    legal_id_type = request.CustomerData.LegalId.Type
                },
                signature
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            Console.WriteLine($"📤 Payload tarjeta: {jsonPayload}");

            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _privateKey);

            var response = await _httpClient.PostAsync($"{_baseUrl}/transactions", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"🔍 StatusCode: {response.StatusCode}");
            Console.WriteLine($"📄 Response: {responseContent}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error Wompi tarjeta: {responseContent}");

            var result = JsonSerializer.Deserialize<WompiTransactionResponse>(
                responseContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                }
            ) ?? throw new Exception("Respuesta vacía de Wompi");

            Console.WriteLine($"✅ Transacción creada - ID: {result.Data?.Id}");
            return result;
        }

        // ─── PSE: BANCOS ────────────────────────────
        public async Task<JsonElement> GetFinancialInstitutionsAsync()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _publicKey);

            var response = await _httpClient.GetAsync($"{_baseUrl}/pse/financial_institutions");
            var content  = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error obteniendo bancos PSE: {content}");

            var doc = JsonDocument.Parse(content);
            return doc.RootElement.GetProperty("data");
        }

        // ─── PSE: CREAR TRANSACCIÓN ─────────────────
        public async Task<JsonElement> CreatePSETransactionAsync(object payload)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _privateKey);

            var json = JsonSerializer.Serialize(payload,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                });
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response     = await _httpClient.PostAsync($"{_baseUrl}/transactions", httpContent);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Wompi PSE error: {response.StatusCode} - {responseBody}");

            var doc = JsonDocument.Parse(responseBody);
            return doc.RootElement.GetProperty("data");
        }

        // ─── CONSULTAR TRANSACCIÓN (tipado) ─────────
        public async Task<WompiTransactionResponse> GetTransactionAsync(string transactionId)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _privateKey);

            var response = await _httpClient.GetAsync($"{_baseUrl}/transactions/{transactionId}");
            var content  = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error consultando transacción: {content}");

            var result = JsonSerializer.Deserialize<WompiTransactionResponse>(
                content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                }
            ) ?? throw new Exception("Respuesta vacía");

            Console.WriteLine($"✅ Estado: {result.Data?.Status}");
            return result;
        }

        // ─── CONSULTAR TRANSACCIÓN (JsonElement, para PSEController) ─
        public async Task<JsonElement> GetTransactionStatusAsync(string transactionId)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _privateKey);

            var response = await _httpClient.GetAsync($"{_baseUrl}/transactions/{transactionId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var doc     = JsonDocument.Parse(content);
            return doc.RootElement.GetProperty("data");
        }

        // ─── FIRMA DE INTEGRIDAD ────────────────────
        private static string GenerateIntegritySignature(
            string reference, int amountInCents, string currency, string integrityKey)
        {
            var raw = $"{reference}{amountInCents}{currency}{integrityKey}";
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(raw));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
    public class CardData
    {
        public string Number { get; set; }
        public string Cvc { get; set; }
        public string ExpMonth { get; set; }
        public string ExpYear { get; set; }
        public string CardHolder { get; set; }
    }

    // ✅ NUEVOS MODELOS para tipado fuerte
    public class WompiMerchantResponse
    {
        public MerchantData Data { get; set; }
    }

    public class MerchantData
    {
        public PresignedAcceptance PresignedAcceptance { get; set; }
    }

    public class PresignedAcceptance
    {
        public string AcceptanceToken { get; set; }
        public string Permalink { get; set; }
        public string Type { get; set; }
    }

    public class WompiAcceptanceTokenResponse
    {
        public AcceptanceData Data { get; set; }
    }

    public class AcceptanceData
    {
        public PresignedAcceptance PresignedAcceptance { get; set; }
    }
}