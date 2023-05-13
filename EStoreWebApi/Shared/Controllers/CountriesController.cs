using EStoreWebApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EStoreWebApi.Shared.Controllers;

[ApiController]
[Route("api/countries")]
public class CountriesController : Controller
{
    private readonly AppDbContext _db;

    public CountriesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> ListCountries()
    {
        var countries = await _db.Countries.ToListAsync();

        return Ok(countries);
    }
}