using Auth.Microservice.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Microservice.Data.DbContext
{
    public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
    }
}
