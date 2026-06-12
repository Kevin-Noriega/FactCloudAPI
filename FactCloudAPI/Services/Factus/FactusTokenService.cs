using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using NubeeAPI.Configuration;

namespace NubeeAPI.Services.Factus
{
 
    // ── Respuesta del endpoint /oauth/token ───────────────────────────
    public class FactusTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════
    //  IFactusTokenService
    // ══════════════════════════════════════════════════════════════════
    public interface IFactusTokenService
    {
        /// <summary>
        /// Devuelve un access_token válido.
        /// Si el token en caché no expiró lo reutiliza.
        /// Si expiró intenta refresh; si falla hace login completo.
        /// </summary>
        Task<string> ObtenerTokenAsync();

        /// <summary>Invalida el token en caché (útil tras un 401).</summary>
        void InvalidarToken();
    }

    // ══════════════════════════════════════════════════════════════════
    //  FactusTokenService — implementación
    // ══════════════════════════════════════════════════════════════════
    public class FactusTokenService : IFactusTokenService
    {
        private const string CACHE_KEY_TOKEN = "factus_access_token";
        private const string CACHE_KEY_REFRESH = "factus_refresh_token";

        private readonly HttpClient _http;
        private readonly FactusOptions _opts;
        private readonly IMemoryCache _cache;
        private readonly ILogger<FactusTokenService> _logger;

        public FactusTokenService(
            IHttpClientFactory httpFactory,
            IOptions<FactusOptions> opts,
            IMemoryCache cache,
            ILogger<FactusTokenService> logger)
        {
            _http = httpFactory.CreateClient("Factus");
            _opts = opts.Value;
            _cache = cache;
            _logger = logger;
        }

        public async Task<string> ObtenerTokenAsync()
        {
            // 1. Token en caché y vigente → reutilizar
            if (_cache.TryGetValue(CACHE_KEY_TOKEN, out string? cachedToken) &&
                !string.IsNullOrEmpty(cachedToken))
            {
                return cachedToken;
            }

            // 2. Hay refresh_token → intentar renovar
            if (_cache.TryGetValue(CACHE_KEY_REFRESH, out string? refreshToken) &&
                !string.IsNullOrEmpty(refreshToken))
            {
                try
                {
                    return await RefrescarTokenAsync(refreshToken);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Refresh token falló, haciendo login completo.");
                }
            }

            // 3. Login completo con credenciales
            return await LoginAsync();
        }

        public void InvalidarToken()
        {
            _cache.Remove(CACHE_KEY_TOKEN);
            _logger.LogInformation("Token Factus invalidado manualmente.");
        }

        // ── Login completo (password grant) ──────────────────────────
        private async Task<string> LoginAsync()
        {
            _logger.LogInformation("Solicitando nuevo token Factus (login).");

            var body = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "password",
                ["client_id"] = _opts.ClientId,
                ["client_secret"] = _opts.ClientSecret,
                ["username"] = _opts.Username,
                ["password"] = _opts.Password,
            });

            var response = await _http.PostAsync("/oauth/token", body);
            return await ProcesarRespuestaTokenAsync(response, "login");
        }

        // ── Refresh de token ──────────────────────────────────────────
        private async Task<string> RefrescarTokenAsync(string refreshToken)
        {
            _logger.LogInformation("Renovando token Factus con refresh_token.");

            var body = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["client_id"] = _opts.ClientId,
                ["client_secret"] = _opts.ClientSecret,
                ["refresh_token"] = refreshToken,
            });

            var response = await _http.PostAsync("/oauth/token", body);
            return await ProcesarRespuestaTokenAsync(response, "refresh");
        }

        // ── Procesamiento común de la respuesta ───────────────────────
        private async Task<string> ProcesarRespuestaTokenAsync(
            HttpResponseMessage response, string operacion)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Factus {Operacion} falló. Status: {Status}. Body: {Body}",
                    operacion, response.StatusCode, content);
                throw new FactusAuthException(
                    $"No se pudo obtener el token de Factus ({operacion}). Status: {response.StatusCode}");
            }

            var token = JsonSerializer.Deserialize<FactusTokenResponse>(content)
                ?? throw new FactusAuthException("Respuesta de token inválida.");

            // Guardar en caché con 60s de margen antes de la expiración real
            var expiry = TimeSpan.FromSeconds(Math.Max(token.ExpiresIn - 60, 30));
            _cache.Set(CACHE_KEY_TOKEN, token.AccessToken, expiry);

            if (!string.IsNullOrEmpty(token.RefreshToken))
                _cache.Set(CACHE_KEY_REFRESH, token.RefreshToken,
                    TimeSpan.FromDays(30)); // refresh_token dura 30 días típicamente

            _logger.LogInformation(
                "Token Factus obtenido ({Operacion}). Expira en {Expiry}s.", operacion, token.ExpiresIn);

            return token.AccessToken;
        }
    }

    // ══════════════════════════════════════════════════════════════════
    //  Excepciones personalizadas de Factus
    // ══════════════════════════════════════════════════════════════════
    public class FactusAuthException : Exception
    {
        public FactusAuthException(string message) : base(message) { }
    }

    public class FactusApiException : Exception
    {
        public int StatusCode { get; }
        public string? ResponseBody { get; }

        public FactusApiException(string message, int statusCode, string? body = null)
            : base(message)
        {
            StatusCode = statusCode;
            ResponseBody = body;
        }
    }
}
