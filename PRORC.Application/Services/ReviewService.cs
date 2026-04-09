using Microsoft.Extensions.Logging;
using PRORC.Application.DTOs.Reviews;
using PRORC.Application.Interfaces;
using PRORC.Domain.Entities.Reviews;
using PRORC.Domain.Enums;
using PRORC.Domain.Interfaces.Logging;
using PRORC.Domain.Interfaces.Repositories;

namespace PRORC.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IAuditLogger _auditLogger;
        private readonly ILogger<ReviewService> _logger;

        public ReviewService(
            IReviewRepository reviewRepository,
            IReservationRepository reservationRepository,
            IRestaurantRepository restaurantRepository,
            IAuditLogger auditLogger,
            ILogger<ReviewService> logger)
        {
            _reviewRepository = reviewRepository;
            _reservationRepository = reservationRepository;
            _restaurantRepository = restaurantRepository;
            _auditLogger = auditLogger;
            _logger = logger;
        }

        public async Task<ReviewDto> CreateReviewAsync(CreateReviewRequest request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request));

                var reservation = await _reservationRepository.GetByIdAsync(request.ReservationId)
                    ?? throw new KeyNotFoundException("Reservation not found.");

                if (reservation.UserId != request.UserId)
                    throw new InvalidOperationException("That reservation does not belong to the user.");

                if (reservation.RestaurantId != request.RestaurantId)
                    throw new InvalidOperationException("The reservation does not belong to that restaurant.");

                if (reservation.Status != ReservationStatus.Completed)
                    throw new InvalidOperationException("Only completed reservations can leave reviews.");

                var existingReview = await _reviewRepository.GetByReservationIdAsync(request.ReservationId);
                if (existingReview != null)
                    throw new InvalidOperationException("That reservation already has a review.");

                var review = Review.Create(
                    request.UserId,
                    request.RestaurantId,
                    request.ReservationId,
                    request.Rating,
                    request.Comment,
                    true);

                var createdReview = await _reviewRepository.AddAsync(review);

                await TryWriteAuditAsync(
                    createdReview.UserId,
                    "CreateReview",
                    "Review",
                    createdReview.Id,
                    $"Review created for restaurant {createdReview.RestaurantId}.");

                // Recalculamos el rating del restaurante.
                // Si esto falla, no rompemos la creación de la reseña.
                // La reseña es la operación principal; el rating se puede recalcular luego.
                try
                {
                    var averageRating = await _reviewRepository.GetAverageRatingByRestaurantIdAsync(request.RestaurantId);

                    var restaurant = await _restaurantRepository.GetByIdAsync(request.RestaurantId);
                    if (restaurant != null)
                    {
                        // Aquí seguimos usando UpdateRating, pero solo con el promedio calculado.
                        // O sea: no se cambia manualmente, se recalcula.
                        restaurant.SetCalculatedRating(averageRating);
                        await _restaurantRepository.UpdateAsync(restaurant);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "The restaurant rating could not be recalculated after creating review {ReviewId}.", createdReview.Id);
                }

                return MapReview(createdReview);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating review for reservation {ReservationId}.", request?.ReservationId);
                throw;
            }
        }

        public async Task<IEnumerable<ReviewDto>> GetByRestaurantIdAsync(int restaurantId)
        {
            try
            {
                var reviews = await _reviewRepository.GetByRestaurantIdAsync(restaurantId);
                return reviews.Select(MapReview);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reviews for restaurant {RestaurantId}.", restaurantId);
                throw;
            }
        }

        public async Task<IEnumerable<ReviewDto>> GetByUserIdAsync(int userId)
        {
            try
            {
                var reviews = await _reviewRepository.GetByUserIdAsync(userId);
                return reviews.Select(MapReview);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reviews for user {UserId}.", userId);
                throw;
            }
        }

        private ReviewDto MapReview(Review review)
        {
            return new ReviewDto
            {
                Id = review.Id,
                UserId = review.UserId,
                RestaurantId = review.RestaurantId,
                ReservationId = review.ReservationId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }

        private async Task TryWriteAuditAsync(int? userId, string action, string entityName, int entityId, string details)
        {
            try
            {
                await _auditLogger.LogAsync(userId, action, entityName, entityId, details);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Audit logging failed in ReviewService.");
            }
        }
    }
}