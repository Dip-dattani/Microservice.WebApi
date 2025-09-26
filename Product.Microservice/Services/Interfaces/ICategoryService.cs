using Product.Microservice.Dtos.Category;

namespace Product.Microservice.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDto?> GetCategoryAsync(Guid id);
        Task<List<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createDto);
        Task<CategoryDto> UpdateCategoryAsync(Guid id, UpdateCategoryDto updateDto);
        Task DeleteCategoryAsync(Guid id);
    }
}
