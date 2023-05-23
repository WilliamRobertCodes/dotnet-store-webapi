using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace EStoreWebApi.Configuration;

public static class MvcConfiguration
{
    public static void ConfigureMvc(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddControllers()
            .ConfigureApiBehaviorOptions(options =>
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetailsFactory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                    var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(context.HttpContext, context.ModelState, 422);

                    return new ObjectResult(problemDetails)
                    {
                        StatusCode = 422
                    };
                }
            );

        builder.Services.Configure<MvcOptions>(options =>
        {
            options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
        });
    }
}