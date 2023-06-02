using EStoreWebApi.Features.Accounts.Entities;
using Stripe;

namespace EStoreWebApi.Features.Orders.Services.Options;

public class AppCustomerCreateOptions : CustomerCreateOptions
{
    public AppCustomerCreateOptions(User user)
    {
        Email = user.Email;
        Metadata = new Dictionary<string, string>
        {
            { "eStoreApiId", user.Id.ToString() }
        };
    }
}