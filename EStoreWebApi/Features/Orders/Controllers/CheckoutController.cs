using System.ComponentModel.DataAnnotations;
using EStoreWebApi.Data;
using EStoreWebApi.Features.Cart.Queries;
using EStoreWebApi.Features.Orders.Entities;
using EStoreWebApi.Features.Orders.Services;
using EStoreWebApi.Shared.Responses;
using EStoreWebApi.Shared.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace EStoreWebApi.Features.Orders.Controllers;

[ApiController]
[Route("api/checkout")]
public class CheckoutController : Controller
{
    private readonly AppDbContext _db;
    private readonly AuthService _auth;
    private readonly StripeService _stripe;


    public CheckoutController(AppDbContext db, AuthService auth, StripeService stripe)
    {
        _db = db;
        _auth = auth;
        _stripe = stripe;
    }
    
    public class CheckoutRequest
    {
        [Required]
        public int AddressId { get; set; }
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(CheckoutRequest request)
    {
        var user = await _auth.RequiredUserAsync();
        var address = await _db.UserAddresses
            .Include(address => address.Country)
            .Where(address => address.Id == request.AddressId)
            .Where(address => address.UserId == user.Id)
            .FirstOrDefaultAsync();

        if (address is null)
            return HandleNullAddress(request);

        var cart = await CartAggregateQuery.ForUser(_db, user.Id)
            .FirstOrDefaultAsync();
        
        if (cart is null)
            return HandleEmptyCart();

        var order = new Order()
        {
            UserId = user.Id,
            Status = OrderStatus.PaymentPending,
            AddressFirstName = address.FirstName,
            AddressLastName = address.LastName,
            AddressCompanyName = address.CompanyName,
            AddressStreet1 = address.Street1,
            AddressStreet2 = address.Street2,
            AddressCity = address.City,
            AddressZipCode = address.ZipCode,
            AddressCountryName = address.Country.Name,
            OrderItems = cart.CartLineItems.Select(item => new OrderItem()
            {
                ItemName = item.Product.Name,
                Quantity = item.Quantity,
                TotalPriceInCents = (int) item.TotalPriceInCents,
                ProductId = item.ProductId,
            }).ToList(),
        };

        return Ok(order);
    }

    private IActionResult HandleEmptyCart()
    {
        ModelState.AddModelError("Cart", "User's cart is empty.");
        return BadRequest(ErrorResponse.Make(ModelState));
    }

    private IActionResult HandleNullAddress(CheckoutRequest request)
    {
        var message = $"Address with id {request.AddressId} does not exist, or does not belong the authenticated user.";
        
        ModelState.AddModelError("AddressId", message);
        return UnprocessableEntity(ErrorResponse.Make(ModelState));
    }
}
