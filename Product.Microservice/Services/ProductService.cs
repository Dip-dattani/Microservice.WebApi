using Product.Microservice.Dtos.Product;
using Product.Microservice.Repository.Interfaces;
using Product.Microservice.Services.Interfaces;
using System.Security.Claims;

namespace Product.Microservice.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(IProductRepository productRepository, IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ProductDto?> GetProductAsync(Guid id)
        {
            var product = await _productRepository.GetByIdWithCategoryAsync(id);
            return product == null || product.IsDeleted ? null : MapToResponseDto(product);
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            var activeProducts = products.Where(c => !c.IsDeleted);
            return [.. activeProducts.Select(MapToResponseDto)];
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createDto)
        {
            var currentUserId = GetCurrentUserId();
            var product = new Models.Product
            {
                Name = createDto.Name,
                Description = createDto.Description,
                Price = createDto.Price,
                CategoryId = createDto.CategoryId,
                CreatedDate = DateTime.Now,
                CreatedBy = currentUserId,
            };

            var createdProduct = await _productRepository.AddAsync(product);
            var productWithCategory = await _productRepository.GetByIdWithCategoryAsync(createdProduct.Id);
            return MapToResponseDto(productWithCategory!);
        }

        public async Task<ProductDto> UpdateProductAsync(Guid id, UpdateProductDto updateDto)
        {
            var currentUserId = GetCurrentUserId();

            var product = await _productRepository.GetByIdAsync(id);
            if (product == null || product.IsDeleted)
                throw new KeyNotFoundException($"Product with ID {id} not found");

            product.Name = updateDto.Name;
            product.Description = updateDto.Description;
            product.Price = updateDto.Price;
            product.CategoryId = updateDto.CategoryId;
            product.LastModificationDate = DateTime.Now;
            product.LastModifiedBy = currentUserId;

            await _productRepository.UpdateAsync(product);
            var updatedProduct = await _productRepository.GetByIdWithCategoryAsync(id);
            return MapToResponseDto(updatedProduct!);
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var currentUserId = GetCurrentUserId();
            var product = await _productRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Product with ID {id} not found");
            product.IsDeleted = true;
            product.DeletionTime = DateTime.Now;
            product.DeletedBy = currentUserId;
            await _productRepository.UpdateAsync(product);
        }

        public async Task<List<ProductDto>> GetProductsByCategoryAsync(Guid categoryId)
        {
            var products = await _productRepository.GetByCategoryIdAsync(categoryId);
            var activeProducts = products.Where(t => !t.IsDeleted);
            return [.. activeProducts.Select(MapToResponseDto)];
        }

        public async Task<List<ProductDto>> SearchProductsAsync(string searchTerm)
        {
            var products = await _productRepository.SearchByNameAsync(searchTerm);
            return [.. products.Select(MapToResponseDto)];
        }
        private string? GetCurrentUserId()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User?.Identity?.IsAuthenticated == true)
            {
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? httpContext.User.FindFirst("sub")?.Value;
                return userId;
            }

            return null;
        }
        private static ProductDto MapToResponseDto(Product.Microservice.Models.Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? string.Empty,
                CreatedDate = product.CreatedDate,
                LastModificationDate = product.LastModificationDate,
                CreatedBy = product.CreatedBy,
                LastModifiedBy = product.LastModifiedBy,
            };
        }
    }
}
