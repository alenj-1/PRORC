using PRORC.Domain.Entities.Users;
using PRORC.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<User, int>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<List<User>> GetUsersByRoleAsync(UserRoleEnum role);
        Task<List<User>> SearchUsersByNameAsync(string name);
        Task<bool> ExistsUserByIdAndEmailAsync(int userId, string email);
    }
}
