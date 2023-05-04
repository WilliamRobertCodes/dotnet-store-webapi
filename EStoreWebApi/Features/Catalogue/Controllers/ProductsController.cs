using EStoreWebApi.Data;
using EStoreWebApi.Features.Catalogue.Requests;
using EStoreWebApi.Features.Catalogue.Responses;
using EStoreWebApi.Shared.Responses;
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
    [ProducesResponseType(200, Type = typeof(PaginatedProductResponse))]
    public async Task<IActionResult> ListProducts([FromQuery] ListProductsRequest request)
    {
        var countQuery = _db.Products
            .AsQueryable();
            
        var query = _db.Products
            .Include(p => p.ProductCategories)
            .AsQueryable();
        
        if (request.CategoriesIds.Any())
        {
            query = query.Where(product =>
                product.ProductCategories.Any(
                    category => request.CategoriesIds.Contains(category.Id)));
            
            countQuery = countQuery
                .Where(product =>
                    product.ProductCategories.Any(
                        category => request.CategoriesIds.Contains(category.Id)));
        }

        var products = await query
            .Skip(request.PerPage * (request.Page - 1))
            .Take(request.PerPage)
            .AsNoTracking()
            .ToListAsync();

        var count = countQuery.Count();
        
        return Ok(new PaginatedProductResponse()
        {
            Data = products.Select(ProductResponse.FromProduct).ToList(),
            Pagination = PaginationResponse.FromRequest(request, count),
        });
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

