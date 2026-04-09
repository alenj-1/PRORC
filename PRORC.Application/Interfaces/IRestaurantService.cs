using PRORC.Application.DTOs.Restaurants;

namespace PRORC.Application.Interfaces
{
    public interface IRestaurantService
    {
        Task<RestaurantDto> CreateRestaurantAsync(CreateRestaurantRequest request);
        Task<RestaurantDto?> GetByIdAsync(int restaurantId);
        Task<IEnumerable<RestaurantDto>> GetByOwnerIdAsync(int ownerId);
        Task<IEnumerable<RestaurantDto>> SearchAsync(string? cuisineType, string? address, double? minimumRating);
        Task ActivateRestaurantAsync(int restaurantId);
        Task DeactivateRestaurantAsync(int restaurantId);
        Task SetAvailabilityAsync(int restaurantId, DateTime availableDate, TimeSpan startTime, TimeSpan endTime, int availableTables);
    }
}