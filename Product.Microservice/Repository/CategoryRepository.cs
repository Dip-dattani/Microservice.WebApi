using Product.Microservice.Data.DbContext;
using Product.Microservice.Helper;
using Product.Microservice.Models;
using Product.Microservice.Repository.Interfaces;

namespace Product.Microservice.Repository
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
