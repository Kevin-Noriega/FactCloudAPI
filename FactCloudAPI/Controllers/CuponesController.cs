using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Cupones;
using FactCloudAPI.Models.Planes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;



namespace FactCloudAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuponesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CuponesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("validate")]
        public async Task<ActionResult<CuponValidateResponseDto>> Validate([FromBody] CuponValidateDto dto)
        {
            var now = DateTime.UtcNow;
            var plan = await _context.Set<PlanFacturacion>().FindAsync(dto.PlanId);
            if (plan == null) return BadRequest("Plan no encontrado");

            var cupon = await _context.Cupones
                .FirstOrDefaultAsync(c =>
                    c.Codigo == dto.Code.ToUpper() &&
                    c.IsActive &&
                    (!c.Expiracion.HasValue || c.Expiracion >= now) &&
                    (c.PlanId == null || c.PlanId == dto.PlanId) &&
                    (c.MaxUsos == null || c.UsosCodigo < c.MaxUsos)
                );

            if (cupon == null)
            {
                return Ok(new CuponValidateResponseDto
                {
                    IsValid = false,
                    Message = "Código inválido, expirado o no aplica a este plan"
                });
            }

            var precioFinalPlan = plan.PrecioAnualFinal;
            var descuentoCupón = precioFinalPlan * cupon.DescuentoPorcentaje / 100m;
            var precioFinalConCupón = decimal.Round(precioFinalPlan - descuentoCupón, 0);

            return Ok(new CuponValidateResponseDto
            {
                IsValid = true,
                Code = cupon.Codigo,
                DiscountPercentage = cupon.DescuentoPorcentaje,
                Message = "Cupón aplicado correctamente",
                PriceAfterDiscount = precioFinalConCupón
            });
        }
    }

}
