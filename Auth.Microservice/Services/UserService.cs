using Auth.Microservice.Dtos.User;
using Auth.Microservice.Dtos;
using Auth.Microservice.Models;
using Auth.Microservice.Repositories.Interfaces;
using Auth.Microservice.Services.Interfaces;

namespace Auth.Microservice.Services
{
    public class UserService : IUserService
    {
        private readonly IPasswordService _passwordService;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        public UserService(IPasswordService passwordService, IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }
        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user == null ? null : MapToUserDto(user);
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);

            if (user == null || !user.IsActive)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            //Verify password
            if (!_passwordService.VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            user.LastLoginDate = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            var token = _jwtService.GenerateToken(user);
            return new LoginResponseDto
            {
                User = MapToUserDto(user),
                Token = token
            };
        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await _userRepository.EmailExistsAsync(registerDto.Email))
            {
                throw new InvalidOperationException("User with this email already exists");
            }
            var hashedPassword = _passwordService.HashPassword(registerDto.Password);

            var user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                Phone = registerDto.Phone,
                PasswordHash = hashedPassword,
                UserRole = registerDto.UserRole,
            };

            var createdUser = await _userRepository.AddAsync(user);


            return MapToUserDto(createdUser);
        }

        private static UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                UserRole = user.UserRole,
                IsActive = user.IsActive,
                CreatedDate = user.CreatedDate
            };
        }
    }
}
