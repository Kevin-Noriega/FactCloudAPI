using NubeeAPI.Models.Wompi;
using System.Text.Json;

namespace NubeeAPI.Services.Wompi
{
    public interface IWompiService
    {
        Task<WompiAcceptanceTokenResponse> GetAcceptanceTokenAsync();
        Task<string> TokenizeCardAsync(CardData cardData);
        Task<WompiTransactionResponse> CreateCardTransactionAsync(WompiTransactionRequest request);
        Task<WompiTransactionResponse> GetTransactionAsync(string transactionId);
        Task<JsonElement> GetFinancialInstitutionsAsync();
        Task<JsonElement> CreatePSETransactionAsync(object payload);
        Task<JsonElement> GetTransactionStatusAsync(string transactionId); // ? para PSEController
    }
}
