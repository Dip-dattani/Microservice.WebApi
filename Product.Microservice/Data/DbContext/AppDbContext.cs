using Microsoft.EntityFrameworkCore;
using Product.Microservice.Models;

namespace Product.Microservice.Data.DbContext
{
    public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Models.Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}
