using Product.Microservice.Repository;
using Product.Microservice.Repository.Interfaces;
using Product.Microservice.Services;
using Product.Microservice.Services.Interfaces;

namespace Product.Microservice.Helper
{
    public class DependencyInjectionHelper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            // Register Services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            //services.AddScoped<AuthService>();
        }
    }
}
