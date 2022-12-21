using System;
using CommercialClothes.Models;
using CommercialClothes.Models.DAL;
using CommercialClothes.Services.Interfaces;
using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs.Settings;
using CommercialClothes.Services;
using CommercialClothes.Services.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PBL6.pbl6_web_commercial_sales.CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Services.TokenGenerators;
using CommercialClothes.Services.TokenValidators;
using CommercialClothes.Commons.VNPay;
using Microsoft.EntityFrameworkCore.Migrations;
using Model.DAL.Interfaces;
using Model.DAL.Repositories;

namespace ComercialClothes.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped((Func<IServiceProvider, Func<ECommerceSellingClothesContext>>)((provider) => () => provider.GetService<ECommerceSellingClothesContext>()));
            services.AddScoped<DbFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.Configure<VNPaySettings>(configuration.GetSection("VNPaySettings"));

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddScoped<IUserRepository, userRepo>()
                .AddScoped<ICategoryRepository, CategoryRepository>()
                .AddScoped<IShopRepository, ShopRepository>()
                .AddScoped<IImageRepository, ImageRepository>()
                .AddScoped<IOrderRepository, OrderRepository>()
                .AddScoped<IItemRepository, ItemRepository>()
                .AddScoped<IRoleRepository, RoleRepository>()
                .AddScoped<IRefreshTokenRepository, refreshTokenRepo>()
                .AddScoped<IUserGroupRepository, UserGroupRepository>()
                .AddScoped<IOrderDetailRepository, OrderDetailRepository>()
                .AddScoped<ICredentialRepository, CredentialRepository>()
                .AddScoped<IStatisticalRepository, StatisticalRepository>()
                .AddScoped<IBankRepository, BankRepository>()
                .AddScoped<IBankTypeRepository, BankTypeRepository>()
                .AddScoped<IHistoryTransactionRepository, HistoryTransactionRepository>();
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
                .AddScoped<IPermissionService, PermissionService>()
                .AddScoped<IShopService, ShopService>()
                .AddScoped<IRoleService, RoleService>()
                .AddScoped<IUserGroupService, UserGroupService>()
                .AddScoped<IAuthService, AuthService>()
                .AddScoped<ICartService, CartService>()
                .AddScoped<IStatisticalService, StatisticalService>()
                .AddScoped<IBankService, BankService>()
                .AddScoped<IPaymentService, PaymentService>()
                .AddScoped<IAdminService, AdminService>();
        }
    }
}