using Product.Microservice.Dtos;

namespace Product.Microservice.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDto<UserDto>> GetUserByIdAsync(Guid userId, string token);
    }
}
