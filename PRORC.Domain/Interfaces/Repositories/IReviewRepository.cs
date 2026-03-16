using PRORC.Domain.Entities.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Domain.Interfaces.Repositories
{
    public interface IReviewRepository : IBaseRepository<Review, int>
    {
        Task<List<Review>> GetReviewsByRestaurantAsync(int restaurantId);
        Task<List<Review>> GetReviewsByUserAsync(int userId);
        Task<double> GetAverageRatingAsync(int restaurantId);
        Task<bool> ExistsReviewByUserForRestaurantAsync(int userId, int restaurantId);
        Task<List<Review>> GetLatestReviewsByRestaurantAsync(int restaurantId, int count);
        Task<List<Review>> GetReviewsByRatingAsync(int restaurantId, int rating);
        Task<int> CountReviewsByRestaurantAsync(int restaurantId);
    }
}
