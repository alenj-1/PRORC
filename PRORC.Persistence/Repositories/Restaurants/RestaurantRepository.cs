using PRORC.Domain.Entities.Restaurants;
using Microsoft.EntityFrameworkCore;
using PRORC.Domain.Interfaces.Repositories;
using PRORC.Persistence.Base;
using PRORC.Persistence.Context;

namespace PRORC.Persistence.Repositories.Restaurants
{
    public class RestaurantRepository : BaseRepository<Restaurant, int>, IRestaurantRepository
    {
        public RestaurantRepository(PRORCContext context) : base(context) { }

        public async Task<List<Restaurant>> GetActiveRestaurantsAsync()
        {
            return await _dbSet
                .Where(r => r.IsActive)
                .OrderBy(r => r.Name)
                .ToListAsync();
        }

        public async Task<List<Restaurant>> GetInactiveRestaurantsAsync()
        {
            return await _dbSet
                .Where(r => !r.IsActive)
                .OrderBy(r => r.Name)
                .ToListAsync();
        }

        public async Task<Restaurant?> GetRestaurantWithAvailabilityAsync(int id)
        {
            return await _dbSet
                .Include(r => r.Availabilities)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Restaurant?> GetRestaurantByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(r => r.Name == name);
        }

        public async Task<bool> ExistsRestaurantByNameAsync(string name)
        {
            return await _dbSet
                .AnyAsync(r => r.Name == name);
        }

        public async Task<List<Restaurant>> SearchRestaurantsAsync(string search)
        {
            return await _dbSet
                .Where(r => r.Name.Contains(search) || r.CuisineType.Contains(search))
                .OrderBy(r => r.Name)
                .ToListAsync();
        }

        public async Task<List<Restaurant>> GetRestaurantsByCuisineAsync(string cuisineType)
        {
            return await _dbSet
                .Where(r => r.CuisineType == cuisineType && r.IsActive)
                .OrderBy(r => r.Name)
                .ToListAsync();
        }

        public async Task<List<Restaurant>> GetActiveRestaurantsWithAvailabilityAsync()
        {
            return await _dbSet
                .Where(r => r.IsActive)
                .Include(r => r.Availabilities)
                .OrderBy(r => r.Name)
                .ToListAsync();
        }
    }
}

