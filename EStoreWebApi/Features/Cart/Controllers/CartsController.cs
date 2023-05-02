using EStoreWebApi.Data;
using EStoreWebApi.Features.Cart.Queries;
using EStoreWebApi.Features.Cart.Requests;
using EStoreWebApi.Features.Cart.Responses;
using EStoreWebApi.Shared.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EStoreWebApi.Features.Cart.Controllers;

[ApiController]
[Route("cart")]
public class CartsController : Controller
{
    private readonly AppDbContext _db;
    private readonly AuthService _auth;

    public CartsController(AppDbContext db, AuthService auth)
    {
        _db = db;
        _auth = auth;
    }

    [Authorize]
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(CartResponse))]
    public async Task<IActionResult> GetCart()
    {
        var cart = await GetCartAsync();

        return Ok(CartResponse.FromCart(cart));
    }

    [Authorize]
    [HttpPost("add")]
    [ProducesResponseType(200, Type = typeof(CartResponse))]
    public async Task<IActionResult> AddToCart(AddToCartRequest request)
    {
        var product = await _db.Products.FindAsync(request.ProductId);

        if (product is null)
            return NotFound();

        var cart = await GetCartAsync();

        cart.Update(product, request.Quantity);

        await _db.SaveChangesAsync();

        return Ok(CartResponse.FromCart(cart));
    }

    private async Task<Entities.Cart> GetCartAsync()
    {
        var user = await _auth.UserAsync();

        return await CartAggregateQuery
            .ForUser(_db, user.Id)
            .FirstAsync();
    }
}

