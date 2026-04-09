using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PRORC.Domain.Entities.Menus;
using PRORC.Domain.Interfaces.Repositories;
using PRORC.Persistence.Base;
using PRORC.Persistence.Context;

namespace PRORC.Persistence.Repositories.Menus
{
    public class MenuRepository : BaseRepository<Menu>, IMenuRepository
    {
        public MenuRepository(PRORCContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory) { }

        public async Task<IEnumerable<Menu>> GetByRestaurantIdAsync(int restaurantId)
        {
            try
            {
                return await _context.Menus
                    .AsNoTracking()
                    .Where(m => m.RestaurantId == restaurantId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting menus by RestaurantId {RestaurantId}.", restaurantId);
                throw;
            }
        }

        public async Task<Menu?> GetActiveByRestaurantIdAsync(int restaurantId)
        {
            try
            {
                return await _context.Menus
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.RestaurantId == restaurantId && m.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active menu by RestaurantId {RestaurantId}.", restaurantId);
                throw;
            }
        }

        public async Task<IEnumerable<MenuItem>> GetItemsByMenuIdAsync(int menuId)
        {
            try
            {
                return await _context.MenuItems
                    .AsNoTracking()
                    .Where(mi => mi.MenuId == menuId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting menu items by MenuId {MenuId}.", menuId);
                throw;
            }
        }

        public async Task<MenuItem?> GetMenuItemByIdAsync(int menuItemId)
        {
            try
            {
                return await _context.MenuItems
                    .AsNoTracking()
                    .FirstOrDefaultAsync(mi => mi.Id == menuItemId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting MenuItem by Id {MenuItemId}.", menuItemId);
                throw;
            }
        }
    }
}