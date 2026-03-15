using PRORC.Domain.Entities.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Domain.Interfaces.Repositories
{
    public interface IMenuRepository : IBaseRepository<Menu, int>
    {
        Task<Menu?> GetMenuWithItemsAsync(int id);
        Task<List<Menu>> GetMenusByRestaurantAsync(int restaurantId);
        Task<Menu?> GetMenuByNameAsync(string name);
        Task<bool> ExistsMenuForRestaurantAsync(int restaurantId, string name);
        Task<List<MenuItem>> GetAvailableMenuItemsAsync(int menuId);
    }
}
