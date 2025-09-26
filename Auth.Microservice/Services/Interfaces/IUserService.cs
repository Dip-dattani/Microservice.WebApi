using Auth.Microservice.Dtos.User;
using Auth.Microservice.Dtos;

namespace Auth.Microservice.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        Task<UserDto?> GetUserByIdAsync(Guid userId);
    }
}
