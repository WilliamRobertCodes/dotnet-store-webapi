using EStoreWebApi.Features.Orders.Entities;
using Stripe;

namespace EStoreWebApi.Features.Orders.Response;

public class CreatePaymentIntentResponse
{
    public CreatePaymentIntentResponse(Order order, PaymentIntent paymentIntent)
    {
        Order = OrderResponse.FromOrder(order);
        PaymentIntentClientSecret = paymentIntent.ClientSecret;
    }

    public OrderResponse Order { get; set; }

    public string PaymentIntentClientSecret { get; set; }
}

public class OrderResponse
{
    public OrderStatus Status { get; set; }

    public string AddressFirstName { get; set; }

    public string AddressLastName { get; set; }

    public string? AddressCompanyName { get; set; }

    public string AddressStreet1 { get; set; }

    public string? AddressStreet2 { get; set; }

    public string AddressCity { get; set; }

    public string AddressZipCode { get; set; }

    public string AddressCountryName { get; set; }

    public List<OrderItemResponse> OrderItems { get; set; } = new();

    public int TotalPriceInCents { get; set; }

    public static OrderResponse FromOrder(Order order)
    {
        return new OrderResponse
        {
            Status = order.Status,
            AddressFirstName = order.AddressFirstName,
            AddressLastName = order.AddressLastName,
            AddressCompanyName = order.AddressCompanyName,
            AddressStreet1 = order.AddressStreet1,
            AddressStreet2 = order.AddressStreet2,
            AddressCity = order.AddressCity,
            AddressZipCode = order.AddressZipCode,
            AddressCountryName = order.AddressCountryName,
            TotalPriceInCents = order.TotalPriceInCents,
            OrderItems = order.OrderItems.Select(OrderItemResponse.FromOrderItem).ToList()
        };
    }
}

public class OrderItemResponse
{
    public string ItemName { get; set; }

    public uint Quantity { get; set; }

    public int TotalPriceInCents { get; set; }

    public int UnitPriceInCents { get; set; }

    public int ProductId { get; set; }

    public static OrderItemResponse FromOrderItem(OrderItem item)
    {
        return new OrderItemResponse
        {
            ItemName = item.ItemName,
            Quantity = item.Quantity,
            TotalPriceInCents = item.TotalPriceInCents,
            UnitPriceInCents = item.UnitPriceInCents,
            ProductId = item.ProductId
        };
    }
}