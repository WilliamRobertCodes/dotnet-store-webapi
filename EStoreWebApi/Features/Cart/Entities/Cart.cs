using EStoreWebApi.Features.Accounts.Entities;
using EStoreWebApi.Features.Catalogue.Entities;
using EStoreWebApi.Shared.Entities.Base;

namespace EStoreWebApi.Features.Cart.Entities;

public class Cart : BaseEntity, IAggregateRoot
{
    public int UserId { get; set; }

    public User User { get; set; }

    public List<CartLineItem> CartLineItems { get; private set; }

    public uint TotalPriceInCents => (uint)CartLineItems.Sum(i => i.TotalPriceInCents);

    public void Update(Product product, uint newQuantity)
    {
        GetOrCreateLineItem(product)
            .UpdateQuantity(newQuantity);
    }

    private CartLineItem GetOrCreateLineItem(Product product)
    {
        var lineItem = CartLineItems.Where(item => item.ProductId == product.Id).FirstOrDefault();

        if (lineItem is null)
        {
            lineItem = new CartLineItem()
            {
                CartId = Id,
                ProductId = product.Id,
            };

            CartLineItems.Add(lineItem);
        }

        return lineItem;
    }
}
