using EStoreWebApi.Shared.Entities.Base;

namespace EStoreWebApi.Features.Orders.Entities;

public class OrderItem : BaseEntity
{
    public int ProductId { get; set; }
    
    public int ItemName { get; set; }

    public int QuantityOfItems { get; set; }
    
    public int ItemInitialPrice { get; set; }
    
    public int TotalPriceInCents { get; set; }
}
