using PRORC.Application.DTOs.Reviews;
using PRORC.Application.Interfaces;
using PRORC.Domain.Entities.Reviews;
using PRORC.Domain.Enums;
using PRORC.Domain.Interfaces.Repositories;

namespace PRORC.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IReservationRepository _reservationRepository;

        public ReviewService(
            IReviewRepository reviewRepository,
            IUserRepository userRepository,
            IRestaurantRepository restaurantRepository,
            IOrderRepository orderRepository,
            IReservationRepository reservationRepository)
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
            _restaurantRepository = restaurantRepository;
            _orderRepository = orderRepository;
            _reservationRepository = reservationRepository;
        }

        public async Task<ReviewDto> CreateAsync(CreateReviewRequest request)
        {
            if (request.Rating < 1 || request.Rating > 5)
                throw new InvalidOperationException("Rating must be between 1 and 5 stars.");

            if (await _userRepository.GetByIdAsync(request.UserId) is null)
                throw new InvalidOperationException("The user does not exist.");

            if (await _restaurantRepository.GetByIdAsync(request.RestaurantId) is null)
                throw new InvalidOperationException("The restaurant does not exist");

            var hasCompletedOrder = (await _orderRepository.GetOrdersByUserAsync(request.UserId))
                .Any(o => o.RestaurantId == request.RestaurantId && o.Status == OrderStatusEnum.Completed);

            var hasCompletedReservation = (await _reservationRepository.GetReservationsByUserAsync(request.UserId))
                .Any(r => r.RestaurantId == request.RestaurantId && r.Status == ReservationStatusEnum.Completed);

            if (!hasCompletedOrder && !hasCompletedReservation)
                throw new InvalidOperationException("Reviews can only be created after the service has been completed.");

            var entity = new Review
            {
                UserId = request.UserId,
                RestaurantId = request.RestaurantId,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _reviewRepository.AddAsync(entity);
            return Map(entity);
        }

        public async Task<List<ReviewDto>> GetByRestaurantAsync(int restaurantId)
        {
            var reviews = await _reviewRepository.GetReviewsByRestaurantAsync(restaurantId);
            return reviews.Select(Map).ToList();
        }

        public async Task<List<ReviewDto>> GetByUserAsync(int userId)
        {
            var reviews = await _reviewRepository.GetReviewsByUserAsync(userId);
            return reviews.Select(Map).ToList();
        }

        public Task<double> GetAverageRatingAsync(int restaurantId)
        {
            return _reviewRepository.GetAverageRatingAsync(restaurantId);
        }

        private static ReviewDto Map(Review review) => new()
        {
            Id = review.Id,
            UserId = review.UserId,
            RestaurantId = review.RestaurantId,
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt
        };
    }
}