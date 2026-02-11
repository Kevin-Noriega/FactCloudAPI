using FactCloudAPI.Models.Wompi;
using FactCloudAPI.Services.Wompi;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        public IActionResult GetAcceptanceToken()
        {
            Console.WriteLine("🔵 GetAcceptanceToken llamado - devolviendo mock");

            // Devolver mock directamente (sin llamar a Wompi)
            return Ok(new
            {
                data = new
                {
                    presigned_acceptance = new
                    {
                        acceptance_token = "mock_token_dev_" + Guid.NewGuid().ToString("N")[..8],
                        permalink = "https://sandbox.wompi.co/terms",
                        type = "END_USER_POLICY"
                    }
                }
            });
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
