using EStoreWebApi.Data;
using EStoreWebApi.Services.PasswordHashing;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddAuthentication()
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);
builder.Services.AddAuthorization();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IPasswordhasher, BCryptPasswordHasher>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connection = "host=127.0.0.1;user=root;password=;database=dotnet_estore_dev";
    var serverVersion = new MySqlServerVersion("8.0.27");

    options
        .UseMySql(connection, serverVersion)
        .EnableDetailedErrors()
        .EnableSensitiveDataLogging();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.InjectStylesheet("/swagger/themes/theme-muted.css");
    });

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        new DbSeeder(db).Seed();
    }
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
