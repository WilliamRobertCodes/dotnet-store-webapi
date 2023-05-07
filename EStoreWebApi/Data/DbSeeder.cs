using Bogus;
using EStoreWebApi.Extensions;
using EStoreWebApi.Features.Accounts.Entities;
using EStoreWebApi.Features.Catalogue.Entities;
using EStoreWebApi.Shared.Entities;
using EStoreWebApi.Shared.Services.PasswordHashing;

namespace EStoreWebApi.Data;

public class DbSeeder
{
    private readonly AppDbContext _db;
    private readonly IPasswordhasher _hasher;

    public DbSeeder(AppDbContext db, IPasswordhasher hasher)
    {
        _db = db;
        _hasher = hasher;
    }

    public void Seed()
    {
        if (DbIsPopulated())
            return;

        var faker = new Faker();

        var user = new User()
        {
            Email = "wrobert@vous.lu",
            PasswordHash = _hasher.HashPassword("secret123"),
        };

        _db.Users.Add(user);
        _db.SaveChanges();

        var categories = faker.Commerce.Categories(200)
            .Select(name => new ProductCategory()
            {
                Name = name
            })
            .ToList();

        _db.ProductCategories.AddRange(categories);
        _db.SaveChanges();

        var products = Enumerable.Range(1, 50)
            .Select(_ =>
            {
                var creationDate = faker.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now);
                var updateDate = faker.Date.Between(creationDate, DateTime.Now);

                return new Product()
                {
                    Name = faker.Commerce.ProductName(),
                    Description = faker.Commerce.ProductDescription(),
                    Price = (uint)new Random().Next(500, 50_000),
                    ProductCategories = new List<ProductCategory>()
                    {
                        categories.GetRandomElement(),
                        categories.GetRandomElement(),
                    },
                    CreatedAt = creationDate,
                    UpdatedAt = updateDate,
                };
            })
            .ToList();

        _db.Products.AddRange(products);
        _db.SaveChanges();
        
        /*
        var countries = new List<Country>()
        {
            new() { Name = "Belgium", Code = "BE" },
            new() { Name = "Luxembourg", Code = "LU" },
            new() { Name = "Germany", Code = "DE" },
        };

        _db.Countries.AddRange(countries);
        _db.SaveChanges();
        */
    }

    private bool DbIsPopulated()
        => _db.Products.Any() && _db.Users.Any();
}
