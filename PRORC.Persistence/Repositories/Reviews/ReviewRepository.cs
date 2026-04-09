using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PRORC.Domain.Entities.Reviews;
using PRORC.Domain.Interfaces.Repositories;
using PRORC.Persistence.Base;
using PRORC.Persistence.Context;

namespace PRORC.Persistence.Repositories.Reviews
{
    public class ReviewRepository : BaseRepository<Review>, IReviewRepository
    {
        public ReviewRepository(PRORCContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory) { }

        public async Task<IEnumerable<Review>> GetByRestaurantIdAsync(int restaurantId)
        {
            try
            {
                return await _context.Reviews
                    .AsNoTracking()
                    .Where(r => r.RestaurantId == restaurantId)
                    .OrderByDescending(r => r.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reviews by RestaurantId {RestaurantId}.", restaurantId);
                throw;
            }
        }

        public async Task<IEnumerable<Review>> GetByUserIdAsync(int userId)
        {
            try
            {
                return await _context.Reviews
                    .AsNoTracking()
                    .Where(r => r.UserId == userId)
                    .OrderByDescending(r => r.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reviews by UserId {UserId}.", userId);
                throw;
            }
        }

        public async Task<Review?> GetByReservationIdAsync(int reservationId)
        {
            try
            {
                return await _context.Reviews
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.ReservationId == reservationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting review by ReservationId {ReservationId}.", reservationId);
                throw;
            }
        }

        public async Task<double> GetAverageRatingByRestaurantIdAsync(int restaurantId)
        {
            try
            {
                // Busca si el restaurante tiene reseñas
                var hasReviews = await _context.Reviews
                    .AnyAsync(r => r.RestaurantId == restaurantId);

                // Si no tiene reseñas, devuelve 0
                if (!hasReviews)
                {
                    return 0;
                }

                // Calcula el promedio de la columna Rating
                var average = await _context.Reviews
                    .Where(r => r.RestaurantId == restaurantId)
                    .AverageAsync(r => r.Rating);

                // Redondea a 2 decimales para que quede más limpio
                return Math.Round(average, 2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating average rating for RestaurantId {RestaurantId}.", restaurantId);
                throw;
            }
        }
    }
}