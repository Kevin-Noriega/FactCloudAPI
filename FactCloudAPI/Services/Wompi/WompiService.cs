using FactCloudAPI.Models.Wompi;
using System.Text.Json;
using System.Text;
using System.Security.Cryptography;

namespace FactCloudAPI.Services.Wompi
{
    public class WompiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public WompiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;

            
        }

        // ✅ CORREGIDO: Obtener token de aceptación (ahora con tipado fuerte)
        public async Task<WompiAcceptanceTokenResponse> GetAcceptanceTokenAsync()
        {
            try
            {
                var publicKey = _config["Wompi:PublicKey"];

                // ✅ URL absoluta completa
                var url = $"https://sandbox.wompi.co/v1/merchants/{publicKey}";
                Console.WriteLine($"🔍 URL: {url}");

                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"🔍 StatusCode: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error: {content}");
                }

                var wompiResponse = JsonSerializer.Deserialize<WompiMerchantResponse>(
                    content,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                    }
                );

                Console.WriteLine($"✅ Token obtenido correctamente");

                return new WompiAcceptanceTokenResponse
                {
                    Data = new AcceptanceData
                    {
                        PresignedAcceptance = wompiResponse.Data.PresignedAcceptance
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                throw;
            }
        }



        // ✅ Tokenizar tarjeta (NO necesitas esto en backend, se hace desde frontend)
        public async Task<string> TokenizeCard(CardData cardData)
        {
            try
            {
                var publicKey = _config["Wompi:PublicKey"];

                var payload = new
                {
                    number = cardData.Number,
                    cvc = cardData.Cvc,
                    exp_month = cardData.ExpMonth,
                    exp_year = cardData.ExpYear,
                    card_holder = cardData.CardHolder
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(payload),
                    Encoding.UTF8,
                    "application/json"
                );

                // ✅ Limpiar headers antes de agregar Authorization
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {publicKey}");

                var response = await _httpClient.PostAsync("/tokens/cards", content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ Error tokenizando: {result}");
                    throw new Exception($"Error tokenizando tarjeta: {result}");
                }

                var tokenResponse = JsonSerializer.Deserialize<JsonElement>(result);
                return tokenResponse.GetProperty("data").GetProperty("id").GetString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en TokenizeCard: {ex.Message}");
                throw;
            }
        }

        // ✅ MEJORADO: Crear transacción
        public async Task<WompiTransactionResponse> CreateTransactionAsync(
    WompiTransactionRequest request)
        {
            try
            {
                var privateKey = _config["Wompi:PrivateKey"];
                var integrityKey = _config["Wompi:IntegrityKey"];

                Console.WriteLine($"🔵 Creando transacción - Reference: {request.Reference}");
                Console.WriteLine($"📋 Request completo: {JsonSerializer.Serialize(request)}");

                if (string.IsNullOrEmpty(privateKey))
                {
                    throw new Exception("Wompi:PrivateKey no configurada");
                }

                // ✅ Generar signature
                var signature = GenerateIntegritySignature(
                    request.Reference,
                    request.AmountInCents,
                    request.Currency,
                    integrityKey
                );

                // ✅ Construir payload con estructura correcta
                var payload = new
                {
                    acceptance_token = request.AcceptanceToken,
                    amount_in_cents = request.AmountInCents,
                    currency = request.Currency,
                    customer_email = request.CustomerEmail,
                    payment_method = new
                    {
                        type = request.PaymentMethod.Type,
                        token = request.PaymentMethod.Token,
                        installments = request.PaymentMethod.Installments
                    },
                    reference = request.Reference,
                    customer_data = new
                    {
                        full_name = request.CustomerData.FullName,
                        phone_number = request.CustomerData.PhoneNumber,
                        legal_id = request.CustomerData.LegalId.Number,
                        legal_id_type = request.CustomerData.LegalId.Type
                    },
                    signature = signature
                };

                var jsonPayload = JsonSerializer.Serialize(payload);
                Console.WriteLine($"📤 Payload a Wompi: {jsonPayload}");

                var content = new StringContent(
                    jsonPayload,
                    Encoding.UTF8,
                    "application/json"
                );

                // ✅ Limpiar headers y agregar Authorization
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {privateKey}");

                // ✅ URL completa con /v1
                var url = "https://sandbox.wompi.co/v1/transactions";
                Console.WriteLine($"🔍 URL: {url}");

                var response = await _httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"🔍 StatusCode: {response.StatusCode}");
                Console.WriteLine($"📄 Response: {responseContent}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ Error de Wompi: {responseContent}");
                    throw new Exception($"Error Wompi: {responseContent}");
                }

                var transactionResponse = JsonSerializer.Deserialize<WompiTransactionResponse>(
                    responseContent,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                    }
                );

                Console.WriteLine($"✅ Transacción creada - ID: {transactionResponse?.Data?.Id}");

                return transactionResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en CreateTransactionAsync: {ex.Message}");
                throw;
            }
        }


        // ✅ Generar firma de integridad (sin cambios, está bien)
        private string GenerateIntegritySignature(
            string reference,
            int amountInCents,
            string currency,
            string integrityKey)
        {
            var concatenatedString = $"{reference}{amountInCents}{currency}{integrityKey}";

            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(concatenatedString);
                var hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        // ✅ MEJORADO: Consultar transacción
        public async Task<WompiTransactionResponse> GetTransactionAsync(string transactionId)
        {
            try
            {
                Console.WriteLine($"🔍 Consultando transacción: {transactionId}");

                var response = await _httpClient.GetAsync($"/transactions/{transactionId}");
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ Error consultando transacción: {content}");
                    throw new Exception($"Error consultando transacción: {content}");
                }

                var transactionResponse = JsonSerializer.Deserialize<WompiTransactionResponse>(
                    content,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                    }
                );

                Console.WriteLine($"✅ Estado transacción: {transactionResponse?.Data?.Status}");

                return transactionResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetTransactionAsync: {ex.Message}");
                throw;
            }
        }
    }

    // ✅ CardData (sin cambios)
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
