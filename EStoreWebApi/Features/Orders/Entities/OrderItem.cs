using EStoreWebApi.Shared.Entities.Base;

namespace EStoreWebApi.Features.Orders.Entities;

public class OrderItem : BaseEntity
{
    public int OrderId { get; set; }

    public Order Order { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public uint Quantity { get; set; }

    public int TotalPriceInCents { get; set; }

    public int UnitPriceInCents { get; set; }

    public int ProductId { get; set; }
}