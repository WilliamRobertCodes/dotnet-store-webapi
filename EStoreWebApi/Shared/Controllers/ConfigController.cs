using EStoreWebApi.Features.Orders.Services;
using EStoreWebApi.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EStoreWebApi.Shared.Controllers;

[ApiController]
[Route("api/config")]
public class ConfigController : Controller
{
    private readonly AppStripeConfiguration _appStripeConfig;

    public ConfigController(IOptions<AppStripeConfiguration> stripeConfig)
    {
        _appStripeConfig = stripeConfig.Value;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ConfigResponse))]
    public async Task<IActionResult> GetConfig()
    {
        var res = new ConfigResponse()
        {
            StripePublicKey = _appStripeConfig.PublicKey,
        };

        return Ok(res);
    }
}
