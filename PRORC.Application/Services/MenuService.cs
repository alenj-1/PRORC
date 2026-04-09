using Microsoft.Extensions.Logging;
using PRORC.Application.DTOs.Menus;
using PRORC.Application.Interfaces;
using PRORC.Domain.Entities.Menus;
using PRORC.Domain.Interfaces.Logging;
using PRORC.Domain.Interfaces.Repositories;

namespace PRORC.Application.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IAuditLogger _auditLogger;
        private readonly ILogger<MenuService> _logger;

        public MenuService(IMenuRepository menuRepository, IAuditLogger auditLogger, ILogger<MenuService> logger)
        {
            _menuRepository = menuRepository;
            _auditLogger = auditLogger;
            _logger = logger;
        }

        public async Task<MenuDto> CreateMenuAsync(int restaurantId, string name)
        {
            try
            {
                var menu = Menu.Create(restaurantId, name);

                var createdMenu = await _menuRepository.AddAsync(menu);

                _logger.LogInformation("Menu created successfully for restaurant {RestaurantId}.", restaurantId);

                await TryWriteAuditAsync(
                    null,
                    "CreateMenu",
                    "Menu",
                    createdMenu.Id,
                    $"Menu {createdMenu.Name} created for restaurant {createdMenu.RestaurantId}.");

                return MapMenu(createdMenu);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating menu for restaurant {RestaurantId}.", restaurantId);
                throw;
            }
        }

        public async Task<IEnumerable<MenuDto>> GetByRestaurantIdAsync(int restaurantId)
        {
            try
            {
                var menus = await _menuRepository.GetByRestaurantIdAsync(restaurantId);
                return menus.Select(MapMenu);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting menus for restaurant {RestaurantId}.", restaurantId);
                throw;
            }
        }

        public async Task<MenuDto?> GetActiveByRestaurantIdAsync(int restaurantId)
        {
            try
            {
                var menu = await _menuRepository.GetActiveByRestaurantIdAsync(restaurantId);
                return menu == null ? null : MapMenu(menu);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active menu for restaurant {RestaurantId}.", restaurantId);
                throw;
            }
        }

        public async Task UpdateMenuNameAsync(int menuId, string newName)
        {
            try
            {
                var menu = await _menuRepository.GetByIdAsync(menuId)
                    ?? throw new KeyNotFoundException("Menu not found.");

                menu.UpdateName(newName);

                await _menuRepository.UpdateAsync(menu);

                _logger.LogInformation("Menu {MenuId} updated successfully.", menuId);

                await TryWriteAuditAsync(null, "UpdateMenu", "Menu", menu.Id, $"Menu name changed to {menu.Name}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating menu {MenuId}.", menuId);
                throw;
            }
        }

        public async Task ActivateMenuAsync(int menuId)
        {
            try
            {
                var menu = await _menuRepository.GetByIdAsync(menuId)
                    ?? throw new KeyNotFoundException("Menu not found.");

                menu.Activate();
                await _menuRepository.UpdateAsync(menu);

                await TryWriteAuditAsync(null, "ActivateMenu", "Menu", menu.Id, "Menu activated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating menu {MenuId}.", menuId);
                throw;
            }
        }

        public async Task DeactivateMenuAsync(int menuId)
        {
            try
            {
                var menu = await _menuRepository.GetByIdAsync(menuId)
                    ?? throw new KeyNotFoundException("Menu not found.");

                menu.Deactivate();
                await _menuRepository.UpdateAsync(menu);

                await TryWriteAuditAsync(null, "DeactivateMenu", "Menu", menu.Id, "Menu deactivated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating menu {MenuId}.", menuId);
                throw;
            }
        }

        public async Task<MenuItemDto> AddMenuItemAsync(int menuId, string name, string description, decimal price)
        {
            try
            {
                var menu = await _menuRepository.GetByIdAsync(menuId)
                    ?? throw new KeyNotFoundException("Menu not found.");

                if (!menu.IsActive)
                    throw new InvalidOperationException("You cannot add items to an inactive menu.");

                var item = MenuItem.Create(menuId, name, description, price);

                var createdItem = await _menuRepository.AddMenuItemAsync(item);

                _logger.LogInformation("Menu item {ItemName} added successfully.", createdItem.Name);

                await TryWriteAuditAsync(null, "AddMenuItem", "MenuItem", createdItem.Id, $"Item {createdItem.Name} added.");

                return MapMenuItem(createdItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to menu {MenuId}.", menuId);
                throw;
            }
        }

        public async Task UpdateMenuItemPriceAsync(int menuItemId, decimal newPrice)
        {
            try
            {
                var item = await _menuRepository.GetMenuItemByIdAsync(menuItemId)
                    ?? throw new KeyNotFoundException("Menu item not found.");

                item.UpdatePrice(newPrice);

                await _menuRepository.UpdateMenuItemAsync(item);

                await TryWriteAuditAsync(null, "UpdateMenuItemPrice", "MenuItem", item.Id, $"New price: {item.Price}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating price for menu item {MenuItemId}.", menuItemId);
                throw;
            }
        }

        public async Task MarkMenuItemAsAvailableAsync(int menuItemId)
        {
            try
            {
                var item = await _menuRepository.GetMenuItemByIdAsync(menuItemId)
                    ?? throw new KeyNotFoundException("Menu item not found.");

                item.MarkAsAvailable();

                await _menuRepository.UpdateMenuItemAsync(item);

                await TryWriteAuditAsync(null, "MarkMenuItemAvailable", "MenuItem", item.Id, "Item marked as available.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking menu item {MenuItemId} as available.", menuItemId);
                throw;
            }
        }

        public async Task MarkMenuItemAsNotAvailableAsync(int menuItemId)
        {
            try
            {
                var item = await _menuRepository.GetMenuItemByIdAsync(menuItemId)
                    ?? throw new KeyNotFoundException("Menu item not found.");

                item.MarkAsNotAvailable();

                await _menuRepository.UpdateMenuItemAsync(item);

                await TryWriteAuditAsync(null, "MarkMenuItemNotAvailable", "MenuItem", item.Id, "Item marked as not available.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking menu item {MenuItemId} as not available.", menuItemId);
                throw;
            }
        }

        public async Task<IEnumerable<MenuItemDto>> GetItemsByMenuIdAsync(int menuId)
        {
            try
            {
                var items = await _menuRepository.GetItemsByMenuIdAsync(menuId);
                return items.Select(MapMenuItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting menu items for menu {MenuId}.", menuId);
                throw;
            }
        }

        private MenuDto MapMenu(Menu menu)
        {
            return new MenuDto
            {
                Id = menu.Id,
                RestaurantId = menu.RestaurantId,
                Name = menu.Name,
                IsActive = menu.IsActive
            };
        }

        private MenuItemDto MapMenuItem(MenuItem item)
        {
            return new MenuItemDto
            {
                Id = item.Id,
                MenuId = item.MenuId,
                MenuItemId = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                IsAvailable = item.IsAvailable
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
                _logger.LogWarning(ex, "Audit logging failed in MenuService.");
            }
        }
    }
}