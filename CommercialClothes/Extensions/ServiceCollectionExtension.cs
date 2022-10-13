using System;
using CommercialClothes.Models;
using CommercialClothes.Models.DAL;
using CommercialClothes.Services.Interfaces;
using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs.Settings;
using CommercialClothes.Services;
using CommercialClothes.Services.Interfaces;
using CommercialClothes.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PBL6.pbl6_web_commercial_sales.CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Services.TokenGenerators;
using CommercialClothes.Services.TokenValidators;

namespace ComercialClothes.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure DbContext with Scoped lifetime
            /*services.AddDbContext<ECommerceSellingClothesContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                options.UseLazyLoadingProxies();
            }
            );*/

            services.AddScoped((Func<IServiceProvider, Func<ECommerceSellingClothesContext>>)((provider) => () => provider.GetService<ECommerceSellingClothesContext>()));
            services.AddScoped<DbFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Send emmail
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<ICategoryRepository,CategoryRepository>()
                .AddScoped<IShopRepository, ShopRepository>()
                .AddScoped<IImageRepository,ImageRepository>()
                .AddScoped<IOrderRepository,OrderRepository>()
                .AddScoped<IItemRepository, ItemRepository>()
                .AddScoped<IRoleRepository, RoleRepository>()
                .AddScoped<IRefreshTokenRepository, RefreshTokenRepository>()
                .AddScoped<IUserGroupRepository, UserGroupRepository>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<Encryptor>()
                .AddScoped<AccessTokenGenerator>()
                .AddScoped<RefreshTokenGenerator>()
                .AddScoped<RefreshTokenValidator>()
                .AddScoped<TokenGenerator>()
                .AddScoped<IEmailSender, EmailSender>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IItemService, ItemService>()
                .AddScoped<ICategoryService, CategoryService>()
                .AddScoped<IMapperCustom, Mapper>()
                .AddScoped<IImageService, ImageService>()
                .AddScoped<IOrderService, OrderService>()
                .AddScoped<ISearchService, SearchService>()
                .AddScoped<IRoleService, RoleService>()
                .AddScoped<IShopService, ShopService>()
                .AddScoped<IAuthService, AuthService>();
        }
    }
}
