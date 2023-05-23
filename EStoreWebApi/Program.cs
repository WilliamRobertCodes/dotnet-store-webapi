using EStoreWebApi.Configuration;
using EStoreWebApi.Data;
using EStoreWebApi.Shared.Services.PasswordHashing;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureCors();
builder.ConfigureAuthentication();
builder.ConfigureAuthorization();
builder.ConfigureMvc();
builder.ConfigureSwagger();
builder.ConfigureAppServices();

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

        var seeder = new DbSeeder(db, hasher);
        
        seeder.Seed();
    }
}

app.UseCors();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
