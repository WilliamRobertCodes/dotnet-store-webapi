using System.ComponentModel.DataAnnotations;

namespace EStoreWebApi.Features.Orders.Requests;

public class CheckoutRequest
{
    [Required] public int OrderId { get; set; }
}