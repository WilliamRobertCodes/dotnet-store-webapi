using EStoreWebApi.Features.Accounts.Entities;
using EStoreWebApi.Features.Orders.Entities;
using EStoreWebApi.Features.Orders.Extensions;
using Microsoft.Extensions.Options;
using Stripe;

namespace EStoreWebApi.Features.Orders.Services;

public class StripeService
{
    private readonly List<string> _allowedPaymentMethods = new() { "card" };
    private readonly CustomerService _customerService;
    private readonly PaymentIntentService _paymentIntentService;

    public StripeService(IOptions<AppStripeConfiguration> config)
    {
        Stripe.StripeConfiguration.ApiKey = config.Value.SecretKey;

        _customerService = new CustomerService();
        _paymentIntentService = new PaymentIntentService();
    }

    public async Task<PaymentIntent> CreatePaymentIntentAsync(User user, Order order)
    {
        var customer = await GetStripeCustomer(user);

        var options = new PaymentIntentCreateOptions
        {
            Amount = order.TotalPriceInCents,
            Currency = "eur",
            PaymentMethodTypes = _allowedPaymentMethods,
            Metadata = order.ToStripeMetadata(),
            Customer = customer.Id
        };

        return await _paymentIntentService.CreateAsync(options);
    }

    private async Task<Customer> GetStripeCustomer(User user)
    {
        if (user.StripeCustomerId is null)
        {
            var options = new CustomerCreateOptions
            {
                Email = user.Email,
                Metadata = new Dictionary<string, string>
                {
                    { "eStoreApiId", user.Id.ToString() }
                }
            };

            return await _customerService.CreateAsync(options);
        }

        return await _customerService.GetAsync(user.StripeCustomerId);
    }
}