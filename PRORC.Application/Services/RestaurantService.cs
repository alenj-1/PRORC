using PRORC.Application.DTOs.Restaurants;
using PRORC.Application.Interfaces;
using PRORC.Domain.Entities.Restaurants;
using PRORC.Domain.Interfaces.Repositories;

namespace PRORC.Application.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;

        public RestaurantService(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        public async Task<RestaurantDto?> GetByIdAsync(int id)
        {
            var entity = await _restaurantRepository.GetByIdAsync(id);
            return entity is null ? null : Map(entity);
        }

        public async Task<List<RestaurantDto>> GetActiveAsync()
        {
            var restaurants = await _restaurantRepository.GetActiveRestaurantsAsync();
            return restaurants.Select(Map).ToList();
        }

        public async Task<List<RestaurantDto>> SearchAsync(string search)
        {
            var restaurants = await _restaurantRepository.SearchRestaurantsAsync(search);
            return restaurants.Select(Map).ToList();
        }

        public async Task<RestaurantDto> CreateAsync(CreateRestaurantRequest request)
        {
            if (await _restaurantRepository.ExistsRestaurantByNameAsync(request.Name))
                throw new InvalidOperationException("There is already a restaurant with that name.");

            var entity = new Restaurant
            {
                Name = request.Name,
                Address = request.Address,
                CuisineType = request.CuisineType,
                IsActive = request.IsActive
            };

            await _restaurantRepository.AddAsync(entity);
            return Map(entity);
        }

        private static RestaurantDto Map(Restaurant restaurant) => new()
        {
            Id = restaurant.Id,
            Name = restaurant.Name,
            Address = restaurant.Address,
            CuisineType = restaurant.CuisineType,
            IsActive = restaurant.IsActive
        };
    }
}
