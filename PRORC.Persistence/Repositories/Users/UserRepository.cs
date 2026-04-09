using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PRORC.Domain.Entities.Users;
using PRORC.Domain.Enums;
using PRORC.Domain.Interfaces.Repositories;
using PRORC.Persistence.Base;
using PRORC.Persistence.Context;

namespace PRORC.Persistence.Repositories.Users
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(PRORCContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory) { }

        public async Task<User?> GetByEmailAsync(string email)
        {
            try
            {
                var normalizedEmail = email.Trim().ToLower();

                return await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == normalizedEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by Email {Email}.", email);
                throw;
            }
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            try
            {
                var normalizedEmail = email.Trim().ToLower();

                return await _context.Users
                    .AnyAsync(u => u.Email == normalizedEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if email exists: {Email}.", email);
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetByRoleAsync(UserRole role)
        {
            try
            {
                return await _context.Users
                    .AsNoTracking()
                    .Where(u => u.Role == role)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users by Role {Role}.", role);
                throw;
            }
        }
    }
}