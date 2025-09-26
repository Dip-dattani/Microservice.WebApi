using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Microservice.Dtos;
using Product.Microservice.Dtos.Category;
using Product.Microservice.Services;
using Product.Microservice.Services.Interfaces;
using Product.Microservice.Shared.Enums;
using StatusCodes = Product.Microservice.Shared.Enums.StatusCodes;


namespace Product.Microservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        //private readonly AuthService _authService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            //_authService = authService;
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
                var createdCategory = await _categoryService.CreateCategoryAsync(createDto);


                if (createdCategory.CreatedBy != null && !string.IsNullOrEmpty(createdCategory.CreatedBy) && Guid.TryParse(createdCategory.CreatedBy, out Guid userId))
                {
                    var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                    //var userResponse = await _authService.GetUserByIdAsync(userId, token);
                    //if (!userResponse.IsError && userResponse.Result != null)
                    //{
                    //    createdCategory.CreatedByName = $"{userResponse.Result.FirstName} {userResponse.Result.LastName}";
                    //    if (createdCategory.LastModificationDate.HasValue)
                    //    {
                    //        if (createdCategory.CreatedBy.Equals(createdCategory.LastModificationDate.Value))
                    //        {
                    //            createdCategory.LastModifiedByName = $"{userResponse.Result.FirstName} {userResponse.Result.LastName}";
                    //        }
                    //        else
                    //        {
                    //            var updateUserId = Guid.Parse(createdCategory.LastModifiedBy.Trim());
                    //            var updateUserResponse = await _authService.GetUserByIdAsync(userId, token);
                    //            if (!updateUserResponse.IsError && updateUserResponse.Result != null)
                    //            {
                    //                createdCategory.LastModifiedByName = $"{updateUserResponse.Result.FirstName} {updateUserResponse.Result.LastName}";
                    //            }
                    //        }
                    //    }
                    //}
                }

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

            if (category.CreatedBy != null && !string.IsNullOrEmpty(category.CreatedBy) && Guid.TryParse(category.CreatedBy, out Guid userId))
            {
                var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                //var userResponse = await _authService.GetUserByIdAsync(userId, token);
                //if (!userResponse.IsError && userResponse.Result != null)
                //{
                //    category.CreatedByName = $"{userResponse.Result.FirstName} {userResponse.Result.LastName}";
                //    if (category.LastModificationDate.HasValue)
                //    {
                //        if (category.CreatedBy.Equals(category.LastModificationDate.Value))
                //        {
                //            category.LastModifiedByName = $"{userResponse.Result.FirstName} {userResponse.Result.LastName}";
                //        }
                //        else
                //        {
                //            var updateUserId = Guid.Parse(category.LastModifiedBy.Trim());
                //            var updateUserResponse = await _authService.GetUserByIdAsync(userId, token);
                //            if (!updateUserResponse.IsError && updateUserResponse.Result != null)
                //            {
                //                category.LastModifiedByName = $"{updateUserResponse.Result.FirstName} {updateUserResponse.Result.LastName}";
                //            }
                //        }
                //    }
                //}
            }


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

    }
}
