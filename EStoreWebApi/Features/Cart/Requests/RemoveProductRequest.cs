using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EStoreWebApi.Features.Cart.Requests;

public class RemoveProductRequest
{
    [Required]
    [JsonPropertyName("productId")]
    public int ProductId { get; set; }
}


