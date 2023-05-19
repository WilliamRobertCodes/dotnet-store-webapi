using EStoreWebApi.Shared.Entities.Base;

namespace EStoreWebApi.Features.Orders.Entities;

public enum OrderStatus
{
    PaymentPending,
    Paid,
    Cancelled,
    Shipped,
    Received,
    Returning,
    Returned,
}

public class Order : TimestampedEntity
{
    public int UserId { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.PaymentPending;

    public string AddressFirstName { get; set; } = null!;

    public string AddressLastName { get; set; } = null!;

    public string? AddressCompanyName { get; set; }

    public string AddressStreet1 { get; set; } = null!;

    public string? AddressStreet2 { get; set; }

    public string AddressCity { get; set; } = null!;

    public string AddressZipCode { get; set; } = null!;

    public string AddressCountryName { get; set; } = null!;

    public List<OrderItem> OrderItems { get; set; } = new();
}
