using Product.Microservice.Dtos;
using Product.Microservice.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Product.Microservice.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly JsonSerializerOptions _jsonOptions;

        public AuthService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            _httpClient.BaseAddress = new Uri("https://localhost:7182/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ResponseDto<UserDto>> GetUserByIdAsync(Guid userId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Make request through gateway
            var response = await _httpClient.GetAsync($"auth/user/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ResponseDto<UserDto>>(content, _jsonOptions);
                return result ?? new ResponseDto<UserDto> { IsError = true, ErrorMessage = "Failed to deserialize response" };
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return new ResponseDto<UserDto>
                {
                    IsError = true,
                    ErrorMessage = $"Failed to get user: {response.StatusCode} - {errorContent}",
                    StatusCode = (int)response.StatusCode
                };
            }
        }
    }
}
