using Product.Microservice.Helper;

namespace Product.Microservice.Repository.Interfaces
{
    public interface IProductRepository : IBaseRepository<Models.Product>
    {
        Task<Models.Product?> GetByIdWithCategoryAsync(Guid id);
        Task<IEnumerable<Models.Product>> GetByCategoryIdAsync(Guid categoryId);
        Task<IEnumerable<Models.Product>> SearchByNameAsync(string name);
    }
}
