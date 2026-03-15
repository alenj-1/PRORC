using PRORC.Application.DTOs.Menus;
using PRORC.Application.Interfaces;
using PRORC.Domain.Entities.Menus;
using PRORC.Domain.Interfaces.Repositories;

namespace PRORC.Application.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IRestaurantRepository _restaurantRepository;

        public MenuService(IMenuRepository menuRepository, IRestaurantRepository restaurantRepository)
        {
            _menuRepository = menuRepository;
            _restaurantRepository = restaurantRepository;
        }

        public async Task<MenuDto?> GetByIdAsync(int id)
        {
            var menu = await _menuRepository.GetMenuWithItemsAsync(id);
            return menu is null ? null : Map(menu);
        }

        public async Task<List<MenuDto>> GetByRestaurantAsync(int restaurantId)
        {
            var menus = await _menuRepository.GetMenusByRestaurantAsync(restaurantId);
            return menus.Select(Map).ToList();
        }

        public async Task<MenuDto> CreateAsync(int restaurantId, string name)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
            if (restaurant is null)
                throw new InvalidOperationException("The restaurant does not exist.");

            if (await _menuRepository.ExistsMenuForRestaurantAsync(restaurantId, name))
                throw new InvalidOperationException("There is already a menu with that name for that restaurant.");

            var entity = new Menu
            {
                RestaurantId = restaurantId,
                Name = name
            };

            await _menuRepository.AddAsync(entity);
            return Map(entity);
        }

        public async Task<List<MenuItemDto>> GetAvailableItemsAsync(int menuId)
        {
            var items = await _menuRepository.GetAvailableMenuItemsAsync(menuId);
            return items.Select(MapItem).ToList();
        }

        private static MenuDto Map(Menu menu) => new()
        {
            Id = menu.Id,
            RestaurantId = menu.RestaurantId,
            Name = menu.Name,
            Items = menu.Items.Select(MapItem).ToList()
        };

        private static MenuItemDto MapItem(MenuItem item) => new()
        {
            Id = item.Id,
            MenuId = item.MenuId,
            Name = item.Name,
            Description = item.Description,
            Price = item.Price,
            IsAvailable = item.IsAvailable
        };
    }
}
