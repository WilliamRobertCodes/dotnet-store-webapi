using EStoreWebApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EStoreWebApi.Features.Catalogue.Controllers;

[ApiController]
[Route("product-categories")]
public class ProductCategoriesController : Controller
{
	private readonly AppDbContext _db;

    public ProductCategoriesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> ListProductCategories()
    {
        var categories = await _db.ProductCategories.ToListAsync();

        return Ok(categories.Select(category => new
        {
            Id = category.Id,
            Name = category.Name,
        }));
    }
}

