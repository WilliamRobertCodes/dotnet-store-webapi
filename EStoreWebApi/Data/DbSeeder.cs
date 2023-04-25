using System;
using System.Linq;
using Bogus;
using EStoreWebApi.Entities;
using EStoreWebApi.Extensions;

namespace EStoreWebApi.Data;

public class DbSeeder
{
	private readonly AppDbContext _db;

    public DbSeeder(AppDbContext db)
    {
        _db = db;
    }

    public void Seed()
    {
        if (DbIsPopulated())
            return;

        var faker = new Faker();

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
                    Price = (uint) new Random().Next(500, 50_000),
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
    }

    private bool DbIsPopulated()
    {
        return _db.Products.Any()
            || _db.ProductCategories.Any();
    }
}
