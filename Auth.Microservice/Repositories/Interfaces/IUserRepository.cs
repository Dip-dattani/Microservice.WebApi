using Auth.Microservice.Helper;
using Auth.Microservice.Models;

namespace Auth.Microservice.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
    }
}
