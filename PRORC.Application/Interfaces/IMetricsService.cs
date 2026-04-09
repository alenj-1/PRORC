using PRORC.Application.DTOs.Metrics;

namespace PRORC.Application.Interfaces
{
    public interface IMetricsService
    {
        Task<MetricsSummaryDto> GetRestaurantSummaryAsync(int restaurantId);
    }
}