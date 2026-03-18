using Microsoft.AspNetCore.Mvc;
using PRORC.Application.Interfaces;

namespace PRORC.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class MetricsController : ControllerBase
    {
        private readonly IMetricsService _metricsService;

        public MetricsController(IMetricsService metricsService)
        {
            _metricsService = metricsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSummaryDTO()
        {
            var result = await _metricsService.GetSummaryDTOAsync();
            return Ok(result);
        }
    }
}
