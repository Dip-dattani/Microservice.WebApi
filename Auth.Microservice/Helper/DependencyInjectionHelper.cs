using Auth.Microservice.Repositories;
using Auth.Microservice.Repositories.Interfaces;
using Auth.Microservice.Services;
using Auth.Microservice.Services.Interfaces;

namespace Auth.Microservice.Helper
{
    public class DependencyInjectionHelper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Register Repositories
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();

            // Register Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IJwtService, JwtService>();
        }
    }
}
