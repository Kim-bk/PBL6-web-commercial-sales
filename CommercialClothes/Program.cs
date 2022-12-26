using ComercialClothes;
using CommercialClothes.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:3000"
                              , "https://www.2clothy.tk", "https://2clothy.tk"
                              , "https://fatalmistake-hub.github.io"
                              , "https://www.sellercenter2clothy.software"
                              , "https://2clothy.vercel.app"
                              , "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"
                              , "https://commerce-2clothy.azurewebsites.net/api/payment"
                              , "https://api.sanbox.paypal.com"
                              , "https://2-clothy-admin.vercel.app")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});

var configurationBuilder = new ConfigurationBuilder()
                            .SetBasePath(builder.Environment.ContentRootPath)
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                            .AddEnvironmentVariables();

builder.Configuration.AddConfiguration(configurationBuilder.Build());

// Add services to the container.

var defaultConnectionString = string.Empty;
defaultConnectionString = builder.Configuration.GetConnectionString("LocalConnection");
builder.Services.AddDbContext<ECommerceSellingClothesContext>(
    options =>
    {
        options.UseSqlServer(defaultConnectionString);
        options.UseLazyLoadingProxies();
    }
);

var serviceProvider = builder.Services.BuildServiceProvider();

try
{
    var dbContext = serviceProvider.GetRequiredService<ECommerceSellingClothesContext>();
    dbContext.Database.Migrate();
}
catch
{
}

///----
var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app);

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors(MyAllowSpecificOrigins);

app.Run();