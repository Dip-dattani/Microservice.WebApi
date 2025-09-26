using Auth.Microservice.Models;

namespace Auth.Microservice.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
