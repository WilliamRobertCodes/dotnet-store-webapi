using System.ComponentModel.DataAnnotations;

namespace EStoreWebApi.Features.Orders.Requests;

public class CreateOrderRequest
{
    [Required] public int AddressId { get; set; }
}