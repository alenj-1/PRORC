using Microsoft.Extensions.Logging;
using PRORC.Application.DTOs.Auth;
using PRORC.Application.DTOs.Users;
using PRORC.Application.Interfaces;
using PRORC.Domain.Entities.Users;
using PRORC.Domain.Interfaces.Logging;
using PRORC.Domain.Interfaces.Repositories;

namespace PRORC.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuditLogger _auditLogger;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserRepository userRepository, IAuditLogger auditLogger, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _auditLogger = auditLogger;
            _logger = logger;
        }

        public async Task<UserDto> RegisterAsync(RegisterRequest request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request));

                if (await _userRepository.ExistsByEmailAsync(request.Email))
                    throw new InvalidOperationException("There is already a user registered with that email.");

                var user = User.Create(request.Name, request.Email, request.Password, request.Role);

                var createdUser = await _userRepository.AddAsync(user);

                _logger.LogInformation("User {Email} registered successfully.", createdUser.Email);

                await TryWriteAuditAsync(
                    createdUser.Id,
                    "Register",
                    "User",
                    createdUser.Id,
                    $"User {createdUser.Email} was registered.");

                return MapUser(createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while registering user with email {Email}.", request?.Email);
                throw;
            }
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request));

                var user = await _userRepository.GetByEmailAsync(request.Email);

                if (user == null)
                {
                    _logger.LogWarning("Login failed. User with email {Email} was not found.", request.Email);

                    return new LoginResponse
                    {
                        IsAuthenticated = false,
                        Message = "Invalid credentials."
                    };
                }

                if (!user.IsActive)
                {
                    _logger.LogWarning("Login failed. User {Email} is inactive.", request.Email);

                    return new LoginResponse
                    {
                        IsAuthenticated = false,
                        Message = "User is inactive."
                    };
                }

                if (user.Password != request.Password)
                {
                    _logger.LogWarning("Login failed. Invalid password for user {Email}.", request.Email);

                    await TryWriteAuditAsync(
                        user.Id,
                        "LoginFailed",
                        "User",
                        user.Id,
                        $"Failed login for user {user.Email}.");

                    return new LoginResponse
                    {
                        IsAuthenticated = false,
                        Message = "Invalid credentials."
                    };
                }

                _logger.LogInformation("User {Email} logged in successfully.", user.Email);

                await TryWriteAuditAsync(
                    user.Id,
                    "Login",
                    "User",
                    user.Id,
                    $"Successful login for user {user.Email}.");

                return new LoginResponse
                {
                    IsAuthenticated = true,
                    Message = "Login successful.",
                    UserId = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role.ToString()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while logging in user with email {Email}.", request?.Email);
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

        // Si la auditoría falla, no rompe el flujo principal, solo se deja evidencia en los logs
        private async Task TryWriteAuditAsync(int? userId, string action, string entityName, int entityId, string details)
        {
            try
            {
                await _auditLogger.LogAsync(userId, action, entityName, entityId, details);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Audit logging failed in AuthService.");
            }
        }
    }
}