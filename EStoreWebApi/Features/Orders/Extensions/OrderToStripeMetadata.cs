using EStoreWebApi.Features.Orders.Entities;

namespace EStoreWebApi.Features.Orders.Extensions;

public static class OrderToStripeMetadata
{
    public static Dictionary<string, string> ToStripeMetadata(this Order order)
    {
        var dict = new Dictionary<string, string>();

        dict.Add("order__id", order.Id.ToString());

        foreach (var item in order.OrderItems)
        {
            var keyPrefix = $"order__item_{item.Id.ToString()}";

            dict.Add($"{keyPrefix}__name", item.ItemName);
            dict.Add($"{keyPrefix}__quantity", item.Quantity.ToString());
            dict.Add($"{keyPrefix}__product_id", item.ProductId.ToString());
        }

        dict.Add("order__address__first_name", order.AddressFirstName);
        dict.Add("order__address__last_name", order.AddressLastName);
        dict.Add("order__address__company_name", order.AddressCompanyName ?? "null");
        dict.Add("order__address__street_1", order.AddressStreet1);
        dict.Add("order__address__street_2", order.AddressStreet2 ?? "null");
        dict.Add("order__address__city", order.AddressCity);
        dict.Add("order__address__zip_code", order.AddressZipCode);
        dict.Add("order__address__country_name", order.AddressCountryName);

        return dict;
    }
}