using Microsoft.EntityFrameworkCore;
using PRORC.Domain.Entities.Menus;
using PRORC.Domain.Interfaces.Repositories;
using PRORC.Persistence.Base;
using PRORC.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Persistence.Repositories.Menus;

public class MenuRepository : BaseRepository<Menu, int>, IMenuRepository
{
    public MenuRepository(PRORCContext context) : base(context)
    {
    }

    public async Task<Menu?> GetMenuWithItemsAsync(int id)
    {
        return await _dbSet
            .Include(m => m.Items)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<List<Menu>> GetMenusByRestaurantAsync(int restaurantId)
    {
        return await _dbSet
            .Where(m => m.RestaurantId == restaurantId)
            .Include(m => m.Items)
            .ToListAsync();
    }

    public async Task<Menu?> GetMenuByNameAsync(string name)
    {
        return await _dbSet
            .FirstOrDefaultAsync(m => m.Name == name);
    }

    public async Task<bool> ExistsMenuForRestaurantAsync(int restaurantId, string name)
    {
        return await _dbSet
            .AnyAsync(m => m.RestaurantId == restaurantId && m.Name == name);
    }

    public async Task<List<MenuItem>> GetAvailableMenuItemsAsync(int menuId)
    {
        return await _context.MenuItems
            .Where(i => i.MenuId == menuId && i.IsAvailable)
            .ToListAsync();
    }
}
