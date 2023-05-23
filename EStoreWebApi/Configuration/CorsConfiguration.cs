namespace EStoreWebApi.Configuration;

public static class CorsConfiguration
{
    public static void ConfigureCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policyBuilder =>
            {
                policyBuilder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(new[]
                    {
                        "https://localhost:3000",
                        "https://localhost:5173",
                    })
                    .AllowCredentials();
            });
        });
    }
}