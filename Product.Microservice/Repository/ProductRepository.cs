using Microsoft.EntityFrameworkCore;
using Product.Microservice.Data.DbContext;
using Product.Microservice.Helper;
using Product.Microservice.Repository.Interfaces;

namespace Product.Microservice.Repository
{
    public class ProductRepository : BaseRepository<Models.Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<Models.Product?> GetByIdWithCategoryAsync(Guid id)
        {
            return await _dbSet
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task<IEnumerable<Models.Product>> GetByCategoryIdAsync(Guid categoryId)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Models.Product>> SearchByNameAsync(string name)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.Name.Contains(name) && !p.IsDeleted)
                .ToListAsync();
        }
    }
}
