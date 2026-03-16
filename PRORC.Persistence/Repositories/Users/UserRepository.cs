using Microsoft.EntityFrameworkCore;
using PRORC.Domain.Entities.Users;
using PRORC.Domain.Enums;
using PRORC.Domain.Interfaces.Repositories;
using PRORC.Persistence.Base;
using PRORC.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Persistence.Repositories.Users
{
    public class UserRepository : BaseRepository<User, int>, IUserRepository
    {
        public UserRepository(PRORCContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet
                .AnyAsync(u => u.Email == email);
        }

        public async Task<List<User>> GetUsersByRoleAsync(UserRoleEnum role)
        {
            return await _dbSet
                .Where(u => u.Role == role)
                .OrderBy(u => u.FullName)
                .ToListAsync();
        }

        public async Task<List<User>> SearchUsersByNameAsync(string name)
        {
            return await _dbSet
                .Where(u => u.FullName.Contains(name))
                .OrderBy(u => u.FullName)
                .ToListAsync();
        }

        public async Task<bool> ExistsUserByIdAndEmailAsync(int userId, string email)
        {
            return await _dbSet
                .AnyAsync(u => u.Id == userId && u.Email == email);
        }
    }
}
