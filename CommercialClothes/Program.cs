using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using ComercialClothes.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace ComercialClothes
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configurationBuilder = new ConfigurationBuilder()
                                        .SetBasePath(builder.Environment.ContentRootPath)
                                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                                        .AddEnvironmentVariables();

            builder.Configuration.AddConfiguration(configurationBuilder.Build());

            // Add services to the container.

            var defaultConnectionString = string.Empty;

            if (builder.Environment.EnvironmentName == "Development")
            {
                defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            }
            else
            {
                // Use connection string provided at runtime by Heroku.
                var connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                connectionUrl = connectionUrl.Replace("postgres://", string.Empty);
                var userPassSide = connectionUrl.Split("@")[0];
                var hostSide = connectionUrl.Split("@")[1];

                var user = userPassSide.Split(":")[0];
                var password = userPassSide.Split(":")[1];
                var host = hostSide.Split("/")[0];
                var database = hostSide.Split("/")[1].Split("?")[0];

                defaultConnectionString = $"Host={host};Database={database};Username={user};Password={password};SSL Mode=Require;Trust Server Certificate=true";
            }

            builder.Services.AddDbContext<ECommerceSellingClothesContext>(options =>
               options.UseNpgsql(defaultConnectionString));

            var serviceProvider = builder.Services.BuildServiceProvider();
            try
            {
                var dbContext = serviceProvider.GetRequiredService<ECommerceSellingClothesContext>();
                dbContext.Database.Migrate();
            }
            catch
            {
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                   

                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.Listen(IPAddress.Any, Convert.ToInt32(Environment.GetEnvironmentVariable("PORT")));
                    }).UseStartup<Startup>();
                });
    }
}
