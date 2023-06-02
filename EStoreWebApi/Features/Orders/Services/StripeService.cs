using EStoreWebApi.Data;
using EStoreWebApi.Features.Accounts.Entities;
using EStoreWebApi.Features.Orders.Entities;
using EStoreWebApi.Features.Orders.Services.Options;
using Microsoft.Extensions.Options;
using Stripe;

namespace EStoreWebApi.Features.Orders.Services;

public class StripeService
{
    private readonly AppDbContext _db;
    private readonly CustomerService _customerService;
    private readonly PaymentIntentService _paymentIntentService;

    public StripeService(IOptions<AppStripeConfiguration> config, AppDbContext db)
    {
        StripeConfiguration.ApiKey = config.Value.SecretKey;

        _db = db;
        _customerService = new CustomerService();
        _paymentIntentService = new PaymentIntentService();
    }

    public async Task<PaymentIntent> CreatePaymentIntentAsync(User user, Order order)
    {
        var customer = await GetStripeCustomer(user);

        var options = new AppPaymentIntentCreateOptions(order, customer);

        return await _paymentIntentService.CreateAsync(options);
    }

    private async Task<Customer> GetStripeCustomer(User user)
    {
        if (user.HasStripeCustomer)
        {
            return await CreateStripeCustomerForUser(user);
        }

        return await _customerService.GetAsync(user.StripeCustomerId);
    }

    private async Task<Customer> CreateStripeCustomerForUser(User user)
    {
        var options = new AppCustomerCreateOptions(user);

        var customer = await _customerService.CreateAsync(options);

        await using (var transaction = await _db.Database.BeginTransactionAsync())
        {
            user.StripeCustomerId = customer.Id;

            await transaction.CommitAsync();
        }

        return customer;
    }
}