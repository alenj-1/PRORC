using Microsoft.Extensions.Logging;
using PRORC.Application.DTOs.Metrics;
using PRORC.Application.Interfaces;
using PRORC.Domain.Enums;
using PRORC.Domain.Interfaces.Repositories;

namespace PRORC.Application.Services
{
    public class MetricsService : IMetricsService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<MetricsService> _logger;

        public MetricsService(
            IReservationRepository reservationRepository,
            IOrderRepository orderRepository,
            IReviewRepository reviewRepository,
            ILogger<MetricsService> logger)
        {
            _reservationRepository = reservationRepository;
            _orderRepository = orderRepository;
            _reviewRepository = reviewRepository;
            _logger = logger;
        }

        public async Task<MetricsSummaryDto> GetRestaurantSummaryAsync(int restaurantId)
        {
            try
            {
                var reservations = (await _reservationRepository.GetByRestaurantIdAsync(restaurantId)).ToList();
                var reviews = (await _reviewRepository.GetByRestaurantIdAsync(restaurantId)).ToList();

                var totalOrders = 0;
                var completedOrders = 0;
                decimal totalRevenue = 0;

                // Recorre las reservas para encontrar las órdenes asociadas
                foreach (var reservation in reservations)
                {
                    var order = await _orderRepository.GetByReservationIdAsync(reservation.Id);

                    if (order != null)
                    {
                        totalOrders++;

                        if (order.Status == OrderStatus.Completed)
                        {
                            completedOrders++;
                            totalRevenue += order.TotalAmount;
                        }
                    }
                }

                var averageRating = await _reviewRepository.GetAverageRatingByRestaurantIdAsync(restaurantId);

                return new MetricsSummaryDto
                {
                    RestaurantId = restaurantId,
                    TotalReservations = reservations.Count,
                    CompletedReservations = reservations.Count(r => r.Status == ReservationStatus.Completed),
                    TotalOrders = totalOrders,
                    CompletedOrders = completedOrders,
                    TotalRevenue = totalRevenue,
                    TotalReviews = reviews.Count,
                    AverageRating = averageRating
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting metrics summary for restaurant {RestaurantId}.", restaurantId);
                throw;
            }
        }
    }
}