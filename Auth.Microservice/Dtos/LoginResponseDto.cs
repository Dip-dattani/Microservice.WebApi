using Auth.Microservice.Dtos.User;

namespace Auth.Microservice.Dtos
{
    public class LoginResponseDto
    {
        public UserDto User { get; set; } = default!;
        public string Token { get; set; } = string.Empty;
    }
}
