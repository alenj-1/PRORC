using PRORC.Application.DTOs.Auth;
using PRORC.Application.DTOs.Users;

namespace PRORC.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto> RegisterAsync(RegisterRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}