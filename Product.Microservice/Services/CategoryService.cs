using Product.Microservice.Dtos.Category;
using Product.Microservice.Models;
using Product.Microservice.Repository.Interfaces;
using Product.Microservice.Services.Interfaces;
using System.Security.Claims;

namespace Product.Microservice.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CategoryService(ICategoryRepository categoryRepository, IHttpContextAccessor httpContextAccessor)
        {
            _categoryRepository = categoryRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createDto)
        {
            var currentUserId = GetCurrentUserId();
            var category = new Category
            {
                Name = createDto.Name,
                Description = createDto.Description,
                CreatedBy = currentUserId,
                CreatedDate = DateTime.Now
            };

            var createdCategory = await _categoryRepository.AddAsync(category);
            return MapToResponseDto(createdCategory!);
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var currentUserId = GetCurrentUserId();
            var category = await _categoryRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Category with ID {id} not found");
            category.IsDeleted = true;
            category.DeletionTime = DateTime.Now;
            category.DeletedBy = currentUserId;

            await _categoryRepository.UpdateAsync(category);
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var activCategories = categories.Where(c => !c.IsDeleted);
            return [.. activCategories.Select(MapToResponseDto)];
        }

        public async Task<CategoryDto?> GetCategoryAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category == null || category.IsDeleted ? null : MapToResponseDto(category);
        }

        public async Task<CategoryDto> UpdateCategoryAsync(Guid id, UpdateCategoryDto updateDto)
        {
            var currentUserId = GetCurrentUserId();

            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null || category.IsDeleted)
                throw new KeyNotFoundException($"Category with ID {id} not found");

            category.Name = updateDto.Name;
            category.Description = updateDto.Description;
            category.LastModificationDate = DateTime.Now;
            category.LastModifiedBy = currentUserId;

            await _categoryRepository.UpdateAsync(category);
            var updatedCategory = await _categoryRepository.GetByIdAsync(id);
            return MapToResponseDto(updatedCategory!);
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
        private static CategoryDto MapToResponseDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedDate = category.CreatedDate,
                LastModificationDate = category.LastModificationDate,
                CreatedBy = category.CreatedBy,
                LastModifiedBy = category.LastModifiedBy
            };
        }
    }
}
