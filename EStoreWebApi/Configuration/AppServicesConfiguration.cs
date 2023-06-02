using EStoreWebApi.Data;
using EStoreWebApi.Features.Orders.Services;
using EStoreWebApi.Shared.Services.Auth;
using EStoreWebApi.Shared.Services.PasswordHashing;

namespace EStoreWebApi.Configuration;

public static class AppServicesConfiguration
{
    public static void ConfigureAppServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<IPasswordhasher, BCryptPasswordHasher>();
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddDbContext<AppDbContext>();
        builder.Services.Configure<AppStripeConfiguration>(builder.Configuration.GetSection("Stripe"));
        builder.Services.AddScoped<StripeService>();
    }
}