using FactCloudAPI.Models.Wompi;
using Newtonsoft.Json;
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

            var baseUrl = _config["Wompi:BaseUrl"];
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        // Obtener token de aceptación
        public async Task<object> GetAcceptanceTokenAsync()
        {
            try
            {
                var publicKey = _config["Wompi:PublicKey"];
                Console.WriteLine($"🔍 PublicKey: {publicKey}");

                if (string.IsNullOrEmpty(publicKey))
                {
                    return new { error = "Wompi:PublicKey no configurada" };
                }

                Console.WriteLine($"🔍 URL: {_httpClient.BaseAddress}/merchants/{publicKey}");

                var response = await _httpClient.GetAsync($"/merchants/{publicKey}");

                Console.WriteLine($"🔍 StatusCode: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"✅ Response: {content}");
                    return JsonConvert.DeserializeObject<dynamic>(content);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ Error: {errorContent}");

                // ✅ NO lanzar excepción, devolver objeto con error
                return new
                {
                    error = true,
                    statusCode = (int)response.StatusCode,
                    message = errorContent
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception: {ex.Message}");
                // ✅ NO lanzar excepción, devolver objeto con error
                return new
                {
                    error = true,
                    message = ex.Message
                };
            }
        }




        // Tokenizar tarjeta (llamar desde frontend)
        public async Task<string> TokenizeCard(CardData cardData)
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
                JsonConvert.SerializeObject(payload),
                Encoding.UTF8,
                "application/json"
            );

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {publicKey}");
            var response = await _httpClient.PostAsync("/tokens/cards", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject(result);
                return data.data.id;
            }

            throw new Exception("Error tokenizando tarjeta");
        }

        // Crear transacción
        public async Task<WompiTransactionResponse> CreateTransactionAsync(
            WompiTransactionRequest request)
        {
            var privateKey = _config["Wompi:PrivateKey"];
            var integrityKey = _config["Wompi:IntegrityKey"];

            // Generar signature
            var signature = GenerateIntegritySignature(
                request.Reference,
                request.AmountInCents,
                request.Currency,
                integrityKey
            );

            var payload = new
            {
                acceptance_token = request.AcceptanceToken,
                amount_in_cents = request.AmountInCents,
                currency = request.Currency,
                customer_email = request.CustomerEmail,
                payment_method = request.PaymentMethod,
                reference = request.Reference,
                customer_data = request.CustomerData,
                signature = signature
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(payload),
                Encoding.UTF8,
                "application/json"
            );

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {privateKey}");

            var response = await _httpClient.PostAsync("/transactions", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<WompiTransactionResponse>(responseContent);
            }

            throw new Exception($"Error creando transacción: {responseContent}");
        }

        // Generar firma de integridad
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

        // Consultar transacción
        public async Task<WompiTransactionResponse> GetTransactionAsync(string transactionId)
        {
            var response = await _httpClient.GetAsync($"/transactions/{transactionId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<WompiTransactionResponse>(content);
            }

            throw new Exception("Error consultando transacción");
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
}
