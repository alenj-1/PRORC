using PRORC.Domain.Entities.Restaurants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Domain.Interfaces.Repositories
{
    public interface IRestaurantRepository : IBaseRepository<Restaurant, int>
    {
        Task<List<Restaurant>> GetActiveRestaurantsAsync();
        Task<List<Restaurant>> GetInactiveRestaurantsAsync();
        Task<Restaurant?> GetRestaurantWithAvailabilityAsync(int id);
        Task<Restaurant?> GetRestaurantByNameAsync(string name);
        Task<bool> ExistsRestaurantByNameAsync(string name);
        Task<List<Restaurant>> SearchRestaurantsAsync(string search);
        Task<List<Restaurant>> GetRestaurantsByCuisineAsync(string cuisineType);
        Task<List<Restaurant>> GetActiveRestaurantsWithAvailabilityAsync();
    }
}
