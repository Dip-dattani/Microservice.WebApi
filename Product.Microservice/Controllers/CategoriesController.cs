using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Microservice.Dtos;
using Product.Microservice.Dtos.Category;
using Product.Microservice.Services.Interfaces;
using Product.Microservice.Shared.Enums;
using System.Security.Claims;
using StatusCodes = Product.Microservice.Shared.Enums.StatusCodes;

namespace Product.Microservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IAuthService _authService;
        public CategoriesController(ICategoryService categoryService, IAuthService authService)
        {
            _categoryService = categoryService;
            _authService = authService;
        }

        [HttpPost("CreateCategory")]
        public async Task<ResponseDto<CategoryDto>> CreateCategory(CreateCategoryDto createDto)
        {
            ResponseDto<CategoryDto> response = new();

            if (!ModelState.IsValid)
            {
                response.IsError = true;
                response.ErrorMessage = ResponseCodes.InvalidInput.GetDescription();
                response.StatusCode = (int)StatusCodes.Status400BadRequest;
                return response;
            }

            try
            {
                var currentUserId = GetCurrentUserId();
                if (currentUserId == Guid.Empty)
                {
                    response.IsError = true;
                    response.ErrorMessage = "User not authenticated";
                    response.StatusCode = (int)StatusCodes.Status401Unauthorized;
                    return response;
                }


                var createdCategory = await _categoryService.CreateCategoryAsync(createDto);

                await EnrichCategoryWithUserData(createdCategory);               

                response.IsError = false;
                response.StatusCode = (int)StatusCodes.Status200OK;
                response.Result = createdCategory;
                return response;
            }
            catch (Exception ex)
            {
                response.IsError = true;
                response.ErrorMessage = $"{ResponseCodes.FailedToCreate.GetDescription()}: {ex.Message}";
                response.StatusCode = (int)StatusCodes.Status400BadRequest;
                return response;
            }
        }

        [HttpGet("GetCategoryById")]
        public async Task<ResponseDto<CategoryDto>> GetCategory(Guid id)
        {
            ResponseDto<CategoryDto> response = new();
            var category = await _categoryService.GetCategoryAsync(id);
            if (category == null)
            {
                response.IsError = true;
                response.ErrorMessage = ResponseCodes.NotFound.GetDescription();
                response.StatusCode = (int)StatusCodes.Status404NotFound;
                return response;
            }

            await EnrichCategoryWithUserData(category);           


            response.IsError = false;
            response.StatusCode = (int)StatusCodes.Status200OK;
            response.Result = category;
            return response;
        }

        [HttpGet("GetAllCategories")]
        public async Task<ResponseDto<List<CategoryDto>>> GetAllCategories()
        {
            ResponseDto<List<CategoryDto>> response = new();

            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                response.IsError = false;
                response.StatusCode = (int)StatusCodes.Status200OK;
                response.Result = categories;
                return response;
            }
            catch (Exception ex)
            {
                response.IsError = true;
                response.ErrorMessage = ex.Message;
                response.StatusCode = (int)StatusCodes.Status400BadRequest;
                return response;
            }
        }


        [HttpPut("UpdateCategory")]
        public async Task<ResponseDto<CategoryDto>> UpdateCategory(Guid id, UpdateCategoryDto updateDto)
        {
            ResponseDto<CategoryDto> response = new();

            if (!ModelState.IsValid)
            {
                response.IsError = true;
                response.ErrorMessage = ResponseCodes.InvalidInput.GetDescription();
                response.StatusCode = (int)StatusCodes.Status400BadRequest;
                return response;
            }


            try
            {
                var updatedCategory = await _categoryService.UpdateCategoryAsync(id, updateDto);
                response.IsError = false;
                response.StatusCode = (int)StatusCodes.Status200OK;
                response.Result = updatedCategory;
                return response;
            }
            catch (Exception ex)
            {
                response.IsError = true;
                response.ErrorMessage = $"Failed to update category: {ex.Message}";
                response.StatusCode = (int)StatusCodes.Status400BadRequest;
                return response;
            }
        }


        [HttpDelete("DeleteCategory")]
        public async Task<ResponseDto<bool>> DeleteCategory(Guid id)
        {
            ResponseDto<bool> response = new();
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                response.IsError = false;
                response.StatusCode = (int)StatusCodes.Status200OK;
                response.Result = true;
                return response;
            }
            catch (Exception ex)
            {
                response.IsError = true;
                response.ErrorMessage = $"Failed to delete category: {ex.Message}";
                response.StatusCode = (int)StatusCodes.Status400BadRequest;
                response.Result = false;
                return response;
            }
        }


        private Guid GetCurrentUserId()
        {
            var userIdClaim = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub")?.Value;
            return Guid.TryParse(userIdClaim, out Guid userId) ? userId : Guid.Empty;
        }

        private string GetAuthorizationToken()
        {
            var authHeader = Request.Headers.Authorization.ToString();
            return authHeader.StartsWith("Bearer ") ? authHeader.Substring(7) : authHeader;
        }

        private async Task EnrichCategoryWithUserData(CategoryDto category)
        {
            if (category == null) return;

            var token = GetAuthorizationToken();
            if (string.IsNullOrEmpty(token)) return;


            Guid createdByUserId = Guid.Empty;
            Guid modifiedByUserId = Guid.Empty;

            // Get creator information
            if (!string.IsNullOrEmpty(category.CreatedBy) && Guid.TryParse(category.CreatedBy, out createdByUserId))
            {
                var userResponse = await _authService.GetUserByIdAsync(createdByUserId, token);
                if (!userResponse.IsError && userResponse.Result != null)
                {
                    category.CreatedByName = $"{userResponse.Result.FirstName} {userResponse.Result.LastName}";
                }
            }

            // Get last modified by information
            if (category.LastModificationDate.HasValue &&
                !string.IsNullOrEmpty(category.LastModifiedBy) &&
                Guid.TryParse(category.LastModifiedBy, out modifiedByUserId) &&
                modifiedByUserId != createdByUserId)
            {
                var modifierResponse = await _authService.GetUserByIdAsync(modifiedByUserId, token);
                if (!modifierResponse.IsError && modifierResponse.Result != null)
                {
                    category.LastModifiedByName = $"{modifierResponse.Result.FirstName} {modifierResponse.Result.LastName}";
                }
            }
            else if (category.LastModificationDate.HasValue && modifiedByUserId == createdByUserId)
            {
                // Same user modified, use the creator name
                category.LastModifiedByName = category.CreatedByName;
            }
            }
        }
    
}
