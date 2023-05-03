using EStoreWebApi.Data;
using EStoreWebApi.Features.Catalogue.Requests;
using EStoreWebApi.Features.Catalogue.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EStoreWebApi.Features.Catalogue.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : Controller
{
    private readonly AppDbContext _db;

    public ProductsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<ProductResponse>))]
    public async Task<IActionResult> ListProducts([FromQuery] ListProductsRequest request)
    {
        var query = _db.Products
            .Include(p => p.ProductCategories)
            .Take(request.PerPage)
            .Skip(request.PerPage * (request.Page - 1))
            .AsQueryable();
        
        if (request.CategoriesIds.Any())
        {
            query = query.Where(product =>
                product.ProductCategories.Any(
                    category => request.CategoriesIds.Contains(category.Id)));
        }

        var products = await query
            .AsNoTracking()
            .ToListAsync();

        return Ok(products.Select(ProductResponse.FromProduct));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(200, Type = typeof(ProductResponse))]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _db.Products
            .Include(p => p.ProductCategories)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        return product is not null
            ? Ok(ProductResponse.FromProduct(product))
            : NotFound();
    }
}

