using BanNoiThat.Application.Interfaces.IRepository;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Infrastructure.SqlServer.DataContext;
using BanNoiThat.Infrastructure.SqlServer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BanNoiThat.API.Extensions
{
    public static class DatabaseSQLExtension
    {
        public static void AddDbContextSQL(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(
                option =>
                {
                    option.UseSqlServer(configuration.GetConnectionString("DefaultSQLConnection"));
                }
            );
        }

        public static void RegisterDIRepository(this IServiceCollection service)
        {
            service.AddScoped<IUnitOfWork, UnitOfWork>();

            service.AddScoped<ICategoriesRepository, CategoriesRepository>();
            service.AddScoped<IProductRepository, ProductRepository>();
            service.AddScoped<IBrandsRepository, BrandRepository>();
            service.AddScoped<ICartRepository, CartRepository>();
            service.AddScoped<IOrdersRepository, OrderRepository>();
            service.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
