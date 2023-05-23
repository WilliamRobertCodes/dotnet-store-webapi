using EStoreWebApi.Data;
using EStoreWebApi.Features.Orders.Entities;
using EStoreWebApi.Features.Orders.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace EStoreWebApi.Features.ThirdParty.Stripe.Controllers;

[ApiController]
[Route("stripe/webhooks")]
public class WebHooksController : Controller
{
    private readonly string _webhookSecret;
    private readonly AppDbContext _db;

    public WebHooksController(IOptions<AppStripeConfiguration> stripeConfig, AppDbContext db)
    {
        _webhookSecret = stripeConfig.Value.WebHookSecret;
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> ProcessWebHook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var signature = Request.Headers["Stripe-Signature"];

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json, signature, _webhookSecret);

            // Handle the event
            if (stripeEvent.Type == Events.PaymentIntentSucceeded)
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                var orderId = paymentIntent.Metadata.First(item => item.Key == "order__id");

                var order = await _db.Orders.FindAsync(int.Parse(orderId.Value));

                order.Status = OrderStatus.Paid;

                await _db.SaveChangesAsync();
            }
            else
            {
                // Unexpected event type
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
            }
            return Ok();
        }
        catch (StripeException e)
        {
            return BadRequest();
        }
    }
}