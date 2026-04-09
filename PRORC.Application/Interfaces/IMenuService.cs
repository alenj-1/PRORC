using PRORC.Application.DTOs.Menus;

namespace PRORC.Application.Interfaces
{
    public interface IMenuService
    {
        Task<MenuDto> CreateMenuAsync(int restaurantId, string name);
        Task<IEnumerable<MenuDto>> GetByRestaurantIdAsync(int restaurantId);
        Task<MenuDto?> GetActiveByRestaurantIdAsync(int restaurantId);
        Task UpdateMenuNameAsync(int menuId, string newName);
        Task ActivateMenuAsync(int menuId);
        Task DeactivateMenuAsync(int menuId);
        Task<MenuItemDto> AddMenuItemAsync(int menuId, string name, string description, decimal price);
        Task UpdateMenuItemPriceAsync(int menuItemId, decimal newPrice);
        Task MarkMenuItemAsAvailableAsync(int menuItemId);
        Task MarkMenuItemAsNotAvailableAsync(int menuItemId);
        Task<IEnumerable<MenuItemDto>> GetItemsByMenuIdAsync(int menuId);
    }
}