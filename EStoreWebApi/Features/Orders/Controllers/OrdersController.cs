using EStoreWebApi.Data;
using EStoreWebApi.Features.Cart.Queries;
using EStoreWebApi.Features.Orders.Extensions;
using EStoreWebApi.Features.Orders.Requests;
using EStoreWebApi.Features.Orders.Response;
using EStoreWebApi.Shared.Responses;
using EStoreWebApi.Shared.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EStoreWebApi.Features.Orders.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : Controller
{
    private readonly AuthService _auth;
    private readonly AppDbContext _db;

    public OrdersController(AppDbContext db, AuthService auth)
    {
        _db = db;
        _auth = auth;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
    {
        var user = await _auth.RequiredUserAsync();
        var address = await _db.UserAddresses
            .Include(address => address.Country)
            .Where(address => address.Id == request.AddressId)
            .Where(address => address.UserId == user.Id)
            .FirstOrDefaultAsync();

        if (address is null)
            return HandleNullAddress(address.Id);

        var cart = await CartAggregateQuery.ForUser(_db, user.Id)
            .FirstOrDefaultAsync();

        if (cart is null)
            return HandleEmptyCart();

        var order = cart.ToOrder(user, address);
        
        cart.RemoveAllProducts();

        await _db.Orders.AddAsync(order);
        await _db.SaveChangesAsync();

        return Ok(OrderResponse.FromOrder(order));
    }
    
    private IActionResult HandleEmptyCart()
    {
        ModelState.AddModelError("Cart", "User's cart is empty.");
        return BadRequest(ErrorResponse.Make(ModelState));
    }

    private IActionResult HandleNullAddress(int addressId)
    {
        var message = $"Address with id {addressId} does not exist, or does not belong the authenticated user.";

        ModelState.AddModelError("AddressId", message);
        return UnprocessableEntity(ErrorResponse.Make(ModelState));
    }
}