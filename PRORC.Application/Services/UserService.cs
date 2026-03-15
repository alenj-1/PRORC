using PRORC.Application.DTOs.Users;
using PRORC.Application.Interfaces;
using PRORC.Domain.Entities.Users;
using PRORC.Domain.Interfaces.Repositories;

namespace PRORC.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user is null ? null : Map(user);
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(Map).ToList();
        }

        public async Task<List<UserDto>> SearchByNameAsync(string name)
        {
            var users = await _userRepository.SearchUsersByNameAsync(name);
            return users.Select(Map).ToList();
        }

        public async Task<UserDto> CreateAsync(CreateUserRequest request)
        {
            if (await _userRepository.EmailExistsAsync(request.Email))
                throw new InvalidOperationException("A user with that email already exists.");

            var entity = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = request.Password,
                Role = request.Role,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(entity);
            return Map(entity);
        }

        private static UserDto Map(User user) => new()
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt
        };
    }
}
