using EStoreWebApi.Features.Orders.Entities;
using EStoreWebApi.Features.Orders.Extensions;
using Stripe;

namespace EStoreWebApi.Features.Orders.Services.Options;

public class AppPaymentIntentCreateOptions : PaymentIntentCreateOptions
{
    public AppPaymentIntentCreateOptions(Order order, Customer customer) : base()
    {
        Amount = order.TotalPriceInCents;
        Currency = "eur";
        PaymentMethodTypes = new() { "card" };
        Metadata = order.ToStripeMetadata();
        Customer = customer.Id;
    }
}
