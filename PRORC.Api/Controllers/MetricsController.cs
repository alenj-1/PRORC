using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRORC.Api.Security;
using PRORC.Application.DTOs.Metrics;
using PRORC.Application.Interfaces;

namespace PRORC.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = AuthPolicies.RestaurantOrSystemAdmin)]

    public class MetricsController(IMetricsService metricsService) : ControllerBase
    {
        private readonly IMetricsService _metricsService = metricsService;

        // GET que permite devolver el resumen de métricas que está definido en MetricsSummaryDto
        [HttpGet("summary")]
        public async Task<ActionResult<MetricsSummaryDto>> GetSummary()
        {
            var summary = await _metricsService.GetSummaryDTOAsync();
            return Ok(summary);
        }
    }
}
