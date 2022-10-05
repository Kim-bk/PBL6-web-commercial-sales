using System;
using CommercialClothes.Models;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Services;
using CommercialClothes.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CommercialClothes.Services.Mapping;
using PBL6.pbl6_web_commercial_sales.CommercialClothes.Models.DAL.Repositories;

namespace ComercialClothes.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure DbContext with Scoped lifetime
            services.AddDbContext<ECommerceSellingClothesContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options.UseLazyLoadingProxies();
            }
            );

            services.AddScoped((Func<IServiceProvider, Func<ECommerceSellingClothesContext>>)((provider) => () => provider.GetService<ECommerceSellingClothesContext>()));
            // TODO : Test Transion
            services.AddScoped<DbFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IItemRepository,ItemRepository>()
                .AddScoped<ICategoryRepository,CategoryRepository>()
                .AddScoped<IImageRepository,ImageRepository>()
                .AddScoped<IOrderRepository,OrderRepository>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IUserService, UserService>()
                .AddScoped<Encryptor>()
                .AddScoped<IItemService,ItemService>()
                .AddScoped<ICategoryService, CategoryService>()
                .AddScoped<IMapperCustom, Mapper>()
                .AddScoped<IImageService,ImageService>()
                .AddScoped<IOrderService,OrderService>();
        }
    }
}
