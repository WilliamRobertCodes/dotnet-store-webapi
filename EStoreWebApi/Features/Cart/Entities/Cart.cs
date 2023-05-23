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

    public void UpdateProduct(Product product, uint newQuantity)
    {
        GetOrCreateLineItem(product)
            .UpdateQuantity(newQuantity);
    }

    public void AddProduct(Product product, uint quantity)
    {
        var item = GetOrCreateLineItem(product);
        
        item.UpdateQuantity(item.Quantity + quantity);
    }

    public void RemoveProduct(Product product)
    {
        var item = GetLineItem(product);
        
        if (item is null)
        {
            return;
        }

        CartLineItems.Remove(item);
    }

    public void RemoveAllProducts()
    {
        CartLineItems.RemoveAll(item => true);
    }

    private CartLineItem? GetLineItem(Product product)
    {
        return CartLineItems.Where(item => item.ProductId == product.Id).FirstOrDefault();
    }

    private CartLineItem GetOrCreateLineItem(Product product)
    {
        var lineItem = GetLineItem(product);

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
