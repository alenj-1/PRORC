using PRORC.Domain.Entities.Menus;

namespace PRORC.Domain.Interfaces.Repositories
{
    public interface IMenuRepository : IBaseRepository<Menu>
    {
        Task<IEnumerable<Menu>> GetByRestaurantIdAsync(int restaurantId);
        Task<Menu?> GetActiveByRestaurantIdAsync(int restaurantId);
        Task<IEnumerable<MenuItem>> GetItemsByMenuIdAsync(int menuId);
        Task<MenuItem?> GetMenuItemByIdAsync(int menuItemId);
        Task<MenuItem> AddMenuItemAsync(MenuItem item);
        Task UpdateMenuItemAsync(MenuItem item);
    }
}