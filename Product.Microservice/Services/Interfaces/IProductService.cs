using Product.Microservice.Dtos.Product;

namespace Product.Microservice.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto?> GetProductAsync(Guid id);
        Task<List<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> CreateProductAsync(CreateProductDto createDto);
        Task<ProductDto> UpdateProductAsync(Guid id, UpdateProductDto updateDto);
        Task DeleteProductAsync(Guid id);
        Task<List<ProductDto>> GetProductsByCategoryAsync(Guid categoryId);
        Task<List<ProductDto>> SearchProductsAsync(string searchTerm);
    }
}
