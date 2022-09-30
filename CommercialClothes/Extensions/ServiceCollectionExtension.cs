using System;
using ComercialClothes.Models;
using ComercialClothes.Models.DAL;
using ComercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs.Settings;
using CommercialClothes.Services;
using CommercialClothes.Services.Interfaces;
using CommercialClothes.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ComercialClothes.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure DbContext with Scoped lifetime
            services.AddDbContext<ECommerceSellingClothesContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                options.UseLazyLoadingProxies();
            }
            );

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
                .AddScoped<IItemRepository, ItemRepository>()
                .AddScoped<IRoleRepository, RoleRepository>()
                .AddScoped<IUserGroupRepository, UserGroupRepository>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IEmailSender, EmailSender>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<ISearchService, SearchService>()
                .AddScoped<IRoleService, RoleService>()
                .AddScoped<IMapperCustom, Mapper>()
                .AddScoped<Encryptor>();
        }
    }
}
