using PRORC.Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> GetByIdAsync(int id);
        Task<List<UserDto>> GetAllAsync();
        Task<List<UserDto>> SearchByNameAsync(string name);
        Task<UserDto> CreateAsync(CreateUserRequest request);
    }
}
