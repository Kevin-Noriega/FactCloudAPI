using FactCloudAPI.Models.Wompi;
using FactCloudAPI.Services.Wompi;
using Microsoft.AspNetCore.Mvc;

namespace FactCloudAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly WompiService _wompiService;

        public PaymentController(WompiService wompiService)
        {
            _wompiService = wompiService;
        }

        [HttpGet("acceptance-token")]
        public async Task<IActionResult> GetAcceptanceToken()
        {
            try
            {
                var result = await _wompiService.GetAcceptanceTokenAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("create-transaction")]
        public async Task<IActionResult> CreateTransaction(
            [FromBody] WompiTransactionRequest request)
        {
            try
            {
                var result = await _wompiService.CreateTransactionAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("transaction/{id}")]
        public async Task<IActionResult> GetTransaction(string id)
        {
            try
            {
                var result = await _wompiService.GetTransactionAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Webhook para recibir notificaciones de Wompi
        [HttpPost("webhook")]
        public async Task<IActionResult> WebhookHandler([FromBody] dynamic payload)
        {
            // Validar firma y procesar webhook
            // Guardar estado de transacción en tu BD
            return Ok();
        }
    }
}
