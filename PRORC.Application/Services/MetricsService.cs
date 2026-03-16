using PRORC.Application.DTOs.Metrics;
using PRORC.Application.Interfaces;
using PRORC.Domain.Enums;
using PRORC.Domain.Interfaces.Repositories;

namespace PRORC.Application.Services
{
    public class MetricsService : IMetricsService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IPaymentRepository _paymentRepository;

        public MetricsService(
            IUserRepository userRepository,
            IRestaurantRepository restaurantRepository,
            IMenuRepository menuRepository,
            IOrderRepository orderRepository,
            IReservationRepository reservationRepository,
            IReviewRepository reviewRepository,
            IPaymentRepository paymentRepository)
        {
            _userRepository = userRepository;
            _restaurantRepository = restaurantRepository;
            _menuRepository = menuRepository;
            _orderRepository = orderRepository;
            _reservationRepository = reservationRepository;
            _reviewRepository = reviewRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<MetricsSummaryDto> GetSummaryDTOAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var restaurants = await _restaurantRepository.GetAllAsync();
            var activeRestaurants = await _restaurantRepository.GetActiveRestaurantsAsync();
            var menus = await _menuRepository.GetAllAsync();
            var orders = await _orderRepository.GetAllAsync();
            var pendingOrders = await _orderRepository.GetOrdersByStatusAsync(OrderStatusEnum.Pending);
            var reservations = await _reservationRepository.GetAllAsync();
            var pendingReservations = await _reservationRepository.GetReservationsByStatusAsync(ReservationStatusEnum.Pending);
            var reviews = await _reviewRepository.GetAllAsync();
            var authorizedPayments = await _paymentRepository.GetPaymentsByStatusAsync(PaymentStatusEnum.Authorized);

            return new MetricsSummaryDto
            {
                TotalUsers = users.Count,
                TotalRestaurants = restaurants.Count,
                ActiveRestaurants = activeRestaurants.Count,
                TotalMenus = menus.Count,
                TotalOrders = orders.Count,
                PendingOrders = pendingOrders.Count,
                TotalReservations = reservations.Count,
                PendingReservations = pendingReservations.Count,
                TotalReviews = reviews.Count,
                AuthorizedPayments = authorizedPayments.Count
            };
        }
    }
}