using Auth.Microservice.Dtos;
using Auth.Microservice.Dtos.User;
using Auth.Microservice.Services.Interfaces;
using Auth.Microservice.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StatusCodes = Auth.Microservice.Shared.Enums.StatusCodes;

namespace Auth.Microservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("Register")]
        public async Task<ResponseDto<UserDto>> Register(RegisterDto registerDto)
        {
            ResponseDto<UserDto> response = new();

            if (!ModelState.IsValid)
            {
                response.IsError = true;
                response.ErrorMessage = "Invalid input provided";
                response.StatusCode = 400;
                return response;
            }

            try
            {
                var result = await _userService.RegisterAsync(registerDto);
                response.IsError = false;
                response.StatusCode = 200;
                response.Result = result;
                return response;
            }
            catch (Exception ex)
            {
                response.IsError = true;
                response.ErrorMessage = $"Registration failed: {ex.Message}";
                response.StatusCode = 500;
                return response;
            }
        }

        [HttpPost("Login")]
        public async Task<ResponseDto<LoginResponseDto>> Login(LoginDto loginDto)
        {
            ResponseDto<LoginResponseDto> response = new();

            if (!ModelState.IsValid)
            {
                response.IsError = true;
                response.ErrorMessage = "Invalid input provided";
                response.StatusCode = 400;
                return response;
            }

            try
            {
                var result = await _userService.LoginAsync(loginDto);
                response.IsError = false;
                response.StatusCode = 200;
                response.Result = result;
                return response;
            }
            catch (Exception ex)
            {
                response.IsError = true;
                response.ErrorMessage = $"Login failed: {ex.Message}";
                response.StatusCode = 400;
                return response;
            }
        }

        [Authorize]
        [HttpGet("GetUser/{userId}")]
        public async Task<ResponseDto<UserDto>> GetUser(Guid userId)
        {
            ResponseDto<UserDto> response = new();

            var userIdClaim = User?.Claims?.FirstOrDefault(c => c.Type == "sub")?.Value;

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                response.IsError = true;
                response.ErrorMessage = ResponseCodes.NotFound.GetDescription();
                response.StatusCode = (int)StatusCodes.Status404NotFound;
                return response;
            }

            response.IsError = false;
            response.StatusCode = (int)StatusCodes.Status200OK;
            response.Result = user;
            return response;
        }
    }
}
