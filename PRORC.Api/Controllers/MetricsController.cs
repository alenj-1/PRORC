using Microsoft.AspNetCore.Mvc;
using PRORC.Application.Interfaces;

namespace PRORC.Api.Controllers
{
    [ApiController]
    [Route("api/metrics")]

    public class MetricsController : ControllerBase
    {
        private readonly IMetricsService _metricsService;

        public MetricsController(IMetricsService metricsService)
        {
            _metricsService = metricsService;
        }


        // GET que permite obtener el resumen de métricas de un restaurante
        [HttpGet("restaurant/{restaurantId:int}")]
        public async Task<IActionResult> GetRestaurantSummary(int restaurantId)
        {
            var result = await _metricsService.GetRestaurantSummaryAsync(restaurantId);
            return Ok(result);
        }
    }
}