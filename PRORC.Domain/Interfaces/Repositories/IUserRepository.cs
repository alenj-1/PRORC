using PRORC.Domain.Entities.Users;
using PRORC.Domain.Enums;

namespace PRORC.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email);
        Task<IEnumerable<User>> GetByRoleAsync(UserRole role);
    }
}