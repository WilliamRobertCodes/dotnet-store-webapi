using Bogus.DataSets;
using EStoreWebApi.Data;
using EStoreWebApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EStoreWebApi.Controllers;

[ApiController]
[Route("/products")]
public class ProductsController : Controller
{
    private readonly AppDbContext _db;

    public ProductsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> ListProducts()
    {
        var products = await _db.Products
            .Include(p => p.ProductCategories)
            .ToListAsync();

        return Ok(products.Select(MapProduct));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _db.Products
            .Include(p => p.ProductCategories)
            .FirstOrDefaultAsync(p => p.Id == id);

        return product is Product
            ? Ok(MapProduct(product))
            : NotFound();
    }

    private object MapProduct(Product product) => new
    {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description,
        Price = product.Price,
        ProductCategories = product.ProductCategories.Select(category => new
        {
            Id = category.Id,
            Name = category.Name,
        }),
        CreatedAt = product.CreatedAt,
        UpdatedAt = product.UpdatedAt,
    };
}

