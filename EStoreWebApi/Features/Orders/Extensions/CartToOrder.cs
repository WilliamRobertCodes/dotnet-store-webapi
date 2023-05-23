using EStoreWebApi.Features.Accounts.Entities;
using EStoreWebApi.Features.Orders.Entities;

namespace EStoreWebApi.Features.Orders.Extensions;

public static class CartToOrder
{
    public static Order ToOrder(this Cart.Entities.Cart cart, User user, UserAddress address)
    {
        return new Order
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
            OrderItems = cart.CartLineItems.Select(item => new OrderItem
            {
                ItemName = item.Product.Name,
                Quantity = item.Quantity,
                TotalPriceInCents = (int)item.TotalPriceInCents,
                ProductId = item.ProductId
            }).ToList()
        };
    }
}