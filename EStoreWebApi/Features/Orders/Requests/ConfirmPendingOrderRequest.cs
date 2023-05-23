using System.ComponentModel.DataAnnotations;

namespace EStoreWebApi.Features.Orders.Requests;

public class ConfirmPendingOrderRequest
{
    [Required] public string PaymentIntentId { get; set; }
}