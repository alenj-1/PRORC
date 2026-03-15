using PRORC.Application.DTOs.Auth;
using PRORC.Application.DTOs.Users;
using PRORC.Application.Interfaces;
using PRORC.Domain.Entities.Users;
using PRORC.Domain.Interfaces.Repositories;

namespace PRORC.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user is null)
            {
                return new LoginResponse
                {
                    IsAuthenticated = false,
                    Message = "User not found."
                };
            }

            if (user.PasswordHash != request.Password)
            {
                return new LoginResponse
                {
                    IsAuthenticated = false,
                    Message = "Invalid Password."
                };
            }

            return new LoginResponse
            {
                IsAuthenticated = true,
                Message = "Successful login.",
                Token = string.Empty,
                User = MapUser(user)
            };
        }

        public async Task<UserDto> RegisterAsync(RegisterRequest request)
        {
            var exists = await _userRepository.EmailExistsAsync(request.Email);
            if (exists)
                throw new InvalidOperationException("There is already a user with that email address.");

            var entity = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = request.Password,
                Role = request.Role,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(entity);

            return MapUser(entity);
        }

        private static UserDto MapUser(User user) => new()
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt
        };
    }
}