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

        // Obtiene todos los menús que pertenecen a un restaurante
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

        // Obtiene el menú activo de un restaurante
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

        // Obtiene todos los items de un menú específico
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

        // Busca un item del menú por su Id
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

        // Agrega un nuevo item a un menú
        public async Task<MenuItem> AddMenuItemAsync(MenuItem item)
        {
            try
            {
                await _context.MenuItems.AddAsync(item);
                await _context.SaveChangesAsync();

                _logger.LogInformation("MenuItem added successfully.");

                return item;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while adding MenuItem.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while adding MenuItem.");
                throw;
            }
        }

        // Actualiza un item del menú existente
        public async Task UpdateMenuItemAsync(MenuItem item)
        {
            try
            {
                _context.MenuItems.Update(item);
                await _context.SaveChangesAsync();

                _logger.LogInformation("MenuItem updated successfully.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while updating MenuItem.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating MenuItem.");
                throw;
            }
        }
    }
}