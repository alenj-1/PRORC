using PRORC.Application.DTOs.Users;
using PRORC.Domain.Enums;

namespace PRORC.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateAsync(CreateUserRequest request);
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto?> GetByEmailAsync(string email);
        Task<IEnumerable<UserDto>> GetByRoleAsync(UserRole role);
        Task ActivateAsync(int userId);
        Task DeactivateAsync(int userId);
        Task ChangeRoleAsync(int userId, UserRole newRole);
    }
}