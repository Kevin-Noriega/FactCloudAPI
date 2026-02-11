using FactCloudAPI.Data;
using FactCloudAPI.DTOs.Planes;
using FactCloudAPI.DTOs.Planes.PlanesFacturacion;
using FactCloudAPI.Models.Planes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace FactCloudAPI.Controllers.Public
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlanesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlanes()
        {
            var planes = await _context.PlanesFacturacion
                .Include(p => p.Features)
                .Where(p => p.Activo)
                .Select(p => new PlanDto
                {
                    Id = p.Id,
                    Name = p.Nombre,
                    Description = p.Descripcion,

                    AnnualPrice = p.PrecioAnualFinal,
                    MonthlyPrice = p.PrecioMensualFinal,

                    HasDiscount = p.DescuentoActivo,
                    DiscountPercentage = p.DescuentoActivo ? p.DescuentoPorcentaje : null,
                    OriginalAnnualPrice = p.DescuentoActivo ? p.PrecioAnual : null,

                    Featured = p.Destacado,
                    Tag = p.Destacado ? "Más vendido" : null,

                    Features = p.Features.Select(f => new PlanFeatureDto
                    {
                        Text = f.Texto,
                        Tooltip = f.Tooltip
                    }).ToList()
                })
                .ToListAsync();

            return Ok(planes);
        }
    }
}
