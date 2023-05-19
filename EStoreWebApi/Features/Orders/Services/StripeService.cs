using EStoreWebApi.Features.Accounts.Entities;
using EStoreWebApi.Features.Orders.Entities;
using Stripe;
using Stripe.Checkout;

namespace EStoreWebApi.Features.Orders.Services;

public class StripeService
{
    public StripeService(string apiKey)
    {
        StripeConfiguration.ApiKey = apiKey;
    }
    
    public async Task<Session> CreateCheckoutSession(User user, Order order)
    {
        var customer = await GetStripeCustomer(user);

        var options = new PriceCreateOptions
        {
            Currency = "usd",
            UnitAmount = 1000,
            Product = "{{PRODUCT_ID}}",
        };
        var service = new PriceService();
        service.Create(options);
    }

    private async Task<Customer> GetStripeCustomer(User user)
    {
        var customerService = new CustomerService();

        if (user.StripeCustomerId is null)
        {
            return await customerService.CreateAsync(new CustomerCreateOptions()
            {
                Email = user.Email,
                Metadata = new()
                {
                    { "eStoreApiId", user.Id.ToString() }
                },
            });
        }

        return await customerService.GetAsync(user.StripeCustomerId);
    }
}