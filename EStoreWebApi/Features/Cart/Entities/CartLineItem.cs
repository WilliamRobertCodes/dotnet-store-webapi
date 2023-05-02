using EStoreWebApi.Features.Catalogue.Entities;
using EStoreWebApi.Shared.Entities.Base;

namespace EStoreWebApi.Features.Cart.Entities;

public class CartLineItem : TimestampedEntity
{
    public static uint MaxQuantity { get; } = 20;

    public int CartId { get; set; }

    public Cart Cart { get; set; }

    public int ProductId { get; set; }

    public Product Product { get; set; }

    public uint Quantity { get; private set; } = 1;

    public void UpdateQuantity(uint newQuantity)
    {
        Quantity = uint.Min(newQuantity, MaxQuantity);
    }

    public uint TotalPriceInCents => Product.Price * Quantity;
}
