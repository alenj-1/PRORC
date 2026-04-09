using Microsoft.Extensions.Logging;
using PRORC.Application.DTOs.Users;
using PRORC.Application.Interfaces;
using PRORC.Domain.Entities.Users;
using PRORC.Domain.Enums;
using PRORC.Domain.Interfaces.Logging;
using PRORC.Domain.Interfaces.Repositories;

namespace PRORC.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuditLogger _auditLogger;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IAuditLogger auditLogger, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _auditLogger = auditLogger;
            _logger = logger;
        }

        public async Task<UserDto> CreateAsync(CreateUserRequest request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request));

                if (await _userRepository.ExistsByEmailAsync(request.Email))
                    throw new InvalidOperationException("There is already a user with that email.");

                var password = request.Password;

                var user = User.Create(request.Name, request.Email, password, request.Role);

                var createdUser = await _userRepository.AddAsync(user);

                await TryWriteAuditAsync(createdUser.Id, "CreateUser", "User", createdUser.Id, "User created.");

                return MapUser(createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user with email {Email}.", request?.Email);
                throw;
            }
        }

        public async Task<UserDto?> GetByIdAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                return user == null ? null : MapUser(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user {UserId}.", userId);
                throw;
            }
        }

        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);
                return user == null ? null : MapUser(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by email {Email}.", email);
                throw;
            }
        }

        public async Task<IEnumerable<UserDto>> GetByRoleAsync(UserRole role)
        {
            try
            {
                var users = await _userRepository.GetByRoleAsync(role);
                return users.Select(MapUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users by role {Role}.", role);
                throw;
            }
        }

        public async Task ActivateAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId)
                    ?? throw new KeyNotFoundException("User not found.");

                user.Activate();

                await _userRepository.UpdateAsync(user);

                await TryWriteAuditAsync(user.Id, "ActivateUser", "User", user.Id, "User activated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating user {UserId}.", userId);
                throw;
            }
        }

        public async Task DeactivateAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId)
                    ?? throw new KeyNotFoundException("User not found.");

                user.Deactivate();

                await _userRepository.UpdateAsync(user);

                await TryWriteAuditAsync(user.Id, "DeactivateUser", "User", user.Id, "User deactivated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating user {UserId}.", userId);
                throw;
            }
        }

        public async Task ChangeRoleAsync(int userId, UserRole newRole)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId)
                    ?? throw new KeyNotFoundException("User not found.");

                user.ChangeRole(newRole);

                await _userRepository.UpdateAsync(user);

                await TryWriteAuditAsync(user.Id, "ChangeRole", "User", user.Id, $"Role changed to {newRole}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing role for user {UserId}.", userId);
                throw;
            }
        }

        private UserDto MapUser(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive
            };
        }

        private async Task TryWriteAuditAsync(int? userId, string action, string entityName, int entityId, string details)
        {
            try
            {
                await _auditLogger.LogAsync(userId, action, entityName, entityId, details);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Audit logging failed in UserService.");
            }
        }
    }
}