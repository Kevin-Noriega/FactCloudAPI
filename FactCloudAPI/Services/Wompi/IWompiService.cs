using FactCloudAPI.Models.Wompi;
using System.Text.Json;

namespace FactCloudAPI.Services.Wompi
{
    public interface IWompiService
    {
        Task<JsonElement> GetFinancialInstitutionsAsync();
        Task<JsonElement> CreatePSETransactionAsync(object payload);
        Task<JsonElement> GetTransactionStatusAsync(string transactionId);
        Task<WompiAcceptanceTokenResponse> GetAcceptanceTokenAsync();
        Task<WompiTransactionResponse> GetTransactionAsync(string transactionId);
    }
}
