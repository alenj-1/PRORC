using PRORC.Application.DTOs.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Application.Interfaces
{
    public interface IMenuService
    {
        Task<MenuDto?> GetByIdAsync(int id);
        Task<List<MenuDto>> GetByRestaurantAsync(int restaurantId);
        Task<MenuDto> CreateAsync(int restaurantId, string name);
        Task<List<MenuItemDto>> GetAvailableItemsAsync(int menuId);
    }
}
