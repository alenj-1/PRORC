using PRORC.Domain.Entities.Restaurants;

namespace PRORC.Domain.Interfaces.Repositories
{
    public interface IRestaurantRepository : IBaseRepository<Restaurant>
    {
        Task<IEnumerable<Restaurant>> GetByOwnerIdAsync(int ownerId);
        Task<IEnumerable<Restaurant>> SearchAsync(string? cuisineType, string? address, double? minimumRating);
        Task<IEnumerable<RestaurantAvailability>> GetAvailabilitiesByRestaurantIdAsync(int restaurantId);
        Task<RestaurantAvailability?> GetAvailabilitySlotAsync(int restaurantId, DateTime availableDate, TimeSpan reservationTime);
        Task<RestaurantAvailability> AddAvailabilityAsync(RestaurantAvailability availability);
        Task UpdateAvailabilityAsync(RestaurantAvailability availability);
    }
}