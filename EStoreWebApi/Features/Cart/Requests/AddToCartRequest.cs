using System.ComponentModel.DataAnnotations;

namespace EStoreWebApi.Features.Cart.Requests;

public class AddToCartRequest
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    public uint Quantity { get; set; }
}

