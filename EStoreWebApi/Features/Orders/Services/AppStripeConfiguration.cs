using System.ComponentModel.DataAnnotations;

namespace EStoreWebApi.Features.Orders.Services;

public class AppStripeConfiguration
{
    [Required] public string PublicKey { get; set; }

    [Required] public string SecretKey { get; set; }
    
    [Required] public string WebHookSecret { get; set; }
}
