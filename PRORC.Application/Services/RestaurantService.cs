using Microsoft.Extensions.Logging;
using PRORC.Application.DTOs.Restaurants;
using PRORC.Application.Interfaces;
using PRORC.Domain.Entities.Restaurants;
using PRORC.Domain.Enums;
using PRORC.Domain.Interfaces.Logging;
using PRORC.Domain.Interfaces.Repositories;

namespace PRORC.Application.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuditLogger _auditLogger;
        private readonly ILogger<RestaurantService> _logger;

        public RestaurantService(
            IRestaurantRepository restaurantRepository,
            IUserRepository userRepository,
            IAuditLogger auditLogger,
            ILogger<RestaurantService> logger)
        {
            _restaurantRepository = restaurantRepository;
            _userRepository = userRepository;
            _auditLogger = auditLogger;
            _logger = logger;
        }

        public async Task<RestaurantDto> CreateRestaurantAsync(CreateRestaurantRequest request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request));

                var owner = await _userRepository.GetByIdAsync(request.OwnerId)
                    ?? throw new KeyNotFoundException("Owner user not found.");

                if (owner.Role != UserRole.RestaurantAdmin)
                    throw new InvalidOperationException("Only a RestaurantAdmin user can own a restaurant.");

                var restaurant = Restaurant.Create(
                    request.OwnerId,
                    request.Name,
                    request.CuisineType,
                    request.Address,
                    request.Description);

                var createdRestaurant = await _restaurantRepository.AddAsync(restaurant);

                _logger.LogInformation("Restaurant {RestaurantId} created successfully.", createdRestaurant.Id);

                await TryWriteAuditAsync(
                    owner.Id,
                    "CreateRestaurant",
                    "Restaurant",
                    createdRestaurant.Id,
                    $"Restaurant {createdRestaurant.Name} was created.");

                return MapRestaurant(createdRestaurant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating restaurant for owner {OwnerId}.", request?.OwnerId);
                throw;
            }
        }

        public async Task<RestaurantDto?> GetByIdAsync(int restaurantId)
        {
            try
            {
                var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
                return restaurant == null ? null : MapRestaurant(restaurant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting restaurant {RestaurantId}.", restaurantId);
                throw;
            }
        }

        public async Task<IEnumerable<RestaurantDto>> GetByOwnerIdAsync(int ownerId)
        {
            try
            {
                var restaurants = await _restaurantRepository.GetByOwnerIdAsync(ownerId);
                return restaurants.Select(MapRestaurant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting restaurants for owner {OwnerId}.", ownerId);
                throw;
            }
        }

        public async Task<IEnumerable<RestaurantDto>> SearchAsync(string? cuisineType, string? address, double? minimumRating)
        {
            try
            {
                var restaurants = await _restaurantRepository.SearchAsync(cuisineType, address, minimumRating);
                return restaurants.Select(MapRestaurant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching restaurants.");
                throw;
            }
        }

        public async Task ActivateRestaurantAsync(int restaurantId)
        {
            try
            {
                var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId)
                    ?? throw new KeyNotFoundException("Restaurant not found.");

                restaurant.Activate();

                await _restaurantRepository.UpdateAsync(restaurant);

                await TryWriteAuditAsync(restaurant.OwnerId, "ActivateRestaurant", "Restaurant", restaurant.Id, "Restaurant activated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating restaurant {RestaurantId}.", restaurantId);
                throw;
            }
        }

        public async Task DeactivateRestaurantAsync(int restaurantId)
        {
            try
            {
                var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId)
                    ?? throw new KeyNotFoundException("Restaurant not found.");

                restaurant.Deactivate();

                await _restaurantRepository.UpdateAsync(restaurant);

                await TryWriteAuditAsync(restaurant.OwnerId, "DeactivateRestaurant", "Restaurant", restaurant.Id, "Restaurant deactivated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating restaurant {RestaurantId}.", restaurantId);
                throw;
            }
        }

        public async Task SetAvailabilityAsync(int restaurantId, DateTime availableDate, TimeSpan startTime, TimeSpan endTime, int availableTables)
        {
            try
            {
                var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId)
                    ?? throw new KeyNotFoundException("Restaurant not found.");

                if (!restaurant.IsActive)
                    throw new InvalidOperationException("You cannot configure availability for an inactive restaurant.");

                var availability = RestaurantAvailability.Create(
                    restaurantId,
                    availableDate,
                    startTime,
                    endTime,
                    availableTables);

                await _restaurantRepository.AddAvailabilityAsync(availability);

                await TryWriteAuditAsync(restaurant.OwnerId, "SetAvailability", "RestaurantAvailability", availability.Id, "Availability registered.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting availability for restaurant {RestaurantId}.", restaurantId);
                throw;
            }
        }

        private RestaurantDto MapRestaurant(Restaurant restaurant)
        {
            return new RestaurantDto
            {
                Id = restaurant.Id,
                OwnerId = restaurant.OwnerId,
                Name = restaurant.Name,
                CuisineType = restaurant.CuisineType,
                Address = restaurant.Address,
                Description = restaurant.Description,
                Rating = restaurant.Rating,
                IsActive = restaurant.IsActive
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
                _logger.LogWarning(ex, "Audit logging failed in RestaurantService.");
            }
        }
    }
}