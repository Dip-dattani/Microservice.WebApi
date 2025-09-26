using Auth.Microservice.Data.DbContext;
using Auth.Microservice.Helper;
using Auth.Microservice.Models;
using Auth.Microservice.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Auth.Microservice.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email && !u.IsDeleted);
        }
    }
}
