using EStoreWebApi.Data;
using EStoreWebApi.Shared.Services.Auth;
using EStoreWebApi.Shared.Services.PasswordHashing;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddAuthentication()
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.Events.OnRedirectToLogin = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };

        options.Events.OnRedirectToAccessDenied = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IPasswordhasher, BCryptPasswordHasher>();
builder.Services.AddTransient<AuthService>();
builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordhasher>();

        new DbSeeder(db, hasher).Seed();
    }
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
