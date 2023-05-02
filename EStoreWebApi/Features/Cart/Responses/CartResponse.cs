using EStoreWebApi.Features.Cart.Entities;
using EStoreWebApi.Features.Catalogue.Entities;

namespace EStoreWebApi.Features.Cart.Responses;

public class CartResponse
{
    public List<CartLineItemResponse> CartLineItems { get; set; }

    public uint TotalPriceInCents { get; set; }

    public static CartResponse FromCart(Entities.Cart cart)
    {
        return new CartResponse()
        {
            CartLineItems = cart.CartLineItems
                .Select(CartLineItemResponse.FromLineItem)
                .ToList(),
            TotalPriceInCents = cart.TotalPriceInCents,
        };
    }
}

public class CartLineItemResponse
{
    public uint Quantity { get; set; }

    public CartProductResponse Product { get; set; }

    public static CartLineItemResponse FromLineItem(CartLineItem lineItem)
    {
        return new CartLineItemResponse()
        {
            Quantity = lineItem.Quantity,
            Product = CartProductResponse.FromProduct(lineItem.Product),
        };
    }
}

public class CartProductResponse
{
    public string Name { get; set; }

    public string Description { get; set; }

    public uint Price { get; set; }

    public static CartProductResponse FromProduct(Product product)
    {
        return new CartProductResponse()
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
        };
    }
}