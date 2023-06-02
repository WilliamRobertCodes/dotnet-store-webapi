using EStoreWebApi.Data;
using EStoreWebApi.Features.Orders.Requests;
using EStoreWebApi.Features.Orders.Response;
using EStoreWebApi.Features.Orders.Services;
using EStoreWebApi.Shared.Responses;
using EStoreWebApi.Shared.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EStoreWebApi.Features.Orders.Controllers;

[ApiController]
[Authorize]
[Route("api/checkout")]
public class CheckoutController : Controller
{
    private readonly AuthService _auth;
    private readonly AppDbContext _db;
    private readonly StripeService _stripe;

    public CheckoutController(AppDbContext db, AuthService auth, StripeService stripe)
    {
        _db = db;
        _auth = auth;
        _stripe = stripe;
    }

    [HttpPost("create-payment-intent")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreatePaymentIntentResponse))]
    public async Task<IActionResult> CreatePaymentIntent(CheckoutRequest request)
    {
        var user = await _auth.RequiredUserAsync();
        var order = await _db.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.UserId == user.Id)
            .Where(o => o.Id == request.OrderId)
            .FirstOrDefaultAsync();

        if (order is null)
            return HandleOrderIsNull(order.Id);

        var paymentIntent = await _stripe.CreatePaymentIntentAsync(user, order);
        
        await _db.Orders.AddAsync(order);
        await _db.SaveChangesAsync();

        return Ok(new CreatePaymentIntentResponse(order, paymentIntent));
    }

    private IActionResult HandleOrderIsNull(int orderId)
    {
        var message = $"Order with id {orderId} does not exist, or does not belong the authenticated user.";

        ModelState.AddModelError("OrderId", message);
        return UnprocessableEntity(ErrorResponse.Make(ModelState));
    }
}