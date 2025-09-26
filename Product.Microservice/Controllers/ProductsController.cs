using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Microservice.Dtos;
using Product.Microservice.Dtos.Product;
using Product.Microservice.Services.Interfaces;
using Product.Microservice.Shared.Enums;
using StatusCodes = Product.Microservice.Shared.Enums.StatusCodes;

namespace Product.Microservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetAllProducts")]
        public async Task<ResponseDto<List<ProductDto>>> GetProducts()
        {
            ResponseDto<List<ProductDto>> response = new();
            try
            {
                var products = await _productService.GetAllProductsAsync();
                response.IsError = false;
                response.StatusCode = (int)StatusCodes.Status200OK;
                response.Result = products;
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

        [HttpGet("GetProductById")]
        public async Task<ResponseDto<ProductDto>> GetProduct(Guid id)
        {
            ResponseDto<ProductDto> response = new();
            var product = await _productService.GetProductAsync(id);
            if (product == null)
            {
                response.IsError = true;
                response.ErrorMessage = ResponseCodes.NotFound.GetDescription();
                response.StatusCode = (int)StatusCodes.Status404NotFound;
                return response;
            }

            response.IsError = false;
            response.StatusCode = (int)StatusCodes.Status200OK;
            response.Result = product;
            return response;
        }

        [HttpPost("CreateProduct")]
        public async Task<ResponseDto<ProductDto>> CreateProduct(CreateProductDto createDto)
        {
            ResponseDto<ProductDto> response = new();
            if (!ModelState.IsValid)
            {
                response.IsError = true;
                response.ErrorMessage = ResponseCodes.InvalidInput.GetDescription();
                response.StatusCode = (int)StatusCodes.Status400BadRequest;
                return response;
            }

            try
            {
                var createdProduct = await _productService.CreateProductAsync(createDto);
                response.IsError = false;
                response.StatusCode = (int)StatusCodes.Status200OK;
                response.Result = createdProduct;
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

        [HttpPut("UpdateProduct")]
        public async Task<ResponseDto<ProductDto>> UpdateProduct(Guid id, UpdateProductDto updateDto)
        {
            ResponseDto<ProductDto> response = new();

            if (!ModelState.IsValid)
            {
                response.IsError = true;
                response.ErrorMessage = ResponseCodes.InvalidInput.GetDescription();
                response.StatusCode = (int)StatusCodes.Status400BadRequest;
                return response;
            }

            try
            {
                var updatedProduct = await _productService.UpdateProductAsync(id, updateDto);
                response.IsError = false;
                response.StatusCode = (int)StatusCodes.Status200OK;
                response.Result = updatedProduct;
                return response;
            }
            catch (Exception ex)
            {
                response.IsError = true;
                response.ErrorMessage = $"Failed to update product: {ex.Message}";
                response.StatusCode = (int)StatusCodes.Status400BadRequest;
                return response;
            }
        }

        [HttpDelete("DeleteProduct")]
        public async Task<ResponseDto<bool>> DeleteProduct(Guid id)
        {
            ResponseDto<bool> response = new();
            try
            {
                await _productService.DeleteProductAsync(id);
                response.IsError = false;
                response.StatusCode = (int)StatusCodes.Status200OK;
                response.Result = true;
                return response;
            }
            catch (Exception ex)
            {
                response.IsError = true;
                response.ErrorMessage = $"Failed to delete product: {ex.Message}";
                response.StatusCode = (int)StatusCodes.Status400BadRequest;
                response.Result = false;
                return response;
            }
        }

        [HttpGet("GetProductsByCategory")]
        public async Task<ResponseDto<List<ProductDto>>> GetProductsByCategory(Guid categoryId)
        {
            ResponseDto<List<ProductDto>> response = new();

            try
            {
                var products = await _productService.GetProductsByCategoryAsync(categoryId);
                response.IsError = false;
                response.StatusCode = (int)StatusCodes.Status200OK;
                response.Result = products;
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

        [HttpGet("SearchProducts")]
        public async Task<ResponseDto<List<ProductDto>>> SearchProducts([FromQuery] string searchTerm)
        {
            ResponseDto<List<ProductDto>> response = new();

            try
            {
                var products = await _productService.SearchProductsAsync(searchTerm);
                response.IsError = false;
                response.StatusCode = (int)StatusCodes.Status200OK;
                response.Result = products;
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
    }
}
