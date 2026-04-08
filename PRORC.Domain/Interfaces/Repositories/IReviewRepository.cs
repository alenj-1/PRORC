using PRORC.Domain.Entities.Reviews;

namespace PRORC.Domain.Interfaces.Repositories
{
    public interface IReviewRepository : IBaseRepository<Review>
    {
        Task<IEnumerable<Review>> GetByRestaurantIdAsync(int restaurantId);
        Task<IEnumerable<Review>> GetByUserIdAsync(int userId);
        Task<Review?> GetByReservationIdAsync(int reservationId);
        Task<double> GetAverageRatingByRestaurantIdAsync(int restaurantId);
    }
}