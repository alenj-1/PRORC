using Microsoft.EntityFrameworkCore;
using PRORC.Domain.Entities.Reviews;
using PRORC.Domain.Interfaces.Repositories;
using PRORC.Persistence.Base;
using PRORC.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Persistence.Repositories.Reviews
{
    public class ReviewRepository : BaseRepository<Review, int>, IReviewRepository
    {
        public ReviewRepository(PRORCContext context) : base(context) { }

        public async Task<List<Review>> GetReviewsByRestaurantAsync(int restaurantId)
        {
            return await _dbSet
                .Where(r => r.RestaurantId == restaurantId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Review>> GetReviewsByUserAsync(int userId)
        {
            return await _dbSet
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingAsync(int restaurantId)
        {
            var hasReviews = await _dbSet.AnyAsync(r => r.RestaurantId == restaurantId);

            if (!hasReviews)
                return 0;

            return await _dbSet
                .Where(r => r.RestaurantId == restaurantId)
                .AverageAsync(r => r.Rating);
        }

        public async Task<bool> ExistsReviewByUserForRestaurantAsync(int userId, int restaurantId)
        {
            return await _dbSet
                .AnyAsync(r => r.UserId == userId && r.RestaurantId == restaurantId);
        }

        public async Task<List<Review>> GetLatestReviewsByRestaurantAsync(int restaurantId, int count)
        {
            return await _dbSet
                .Where(r => r.RestaurantId == restaurantId)
                .OrderByDescending(r => r.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Review>> GetReviewsByRatingAsync(int restaurantId, int rating)
        {
            return await _dbSet
                .Where(r => r.RestaurantId == restaurantId && r.Rating == rating)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> CountReviewsByRestaurantAsync(int restaurantId)
        {
            return await _dbSet
                .CountAsync(r => r.RestaurantId == restaurantId);
        }
    }
}
