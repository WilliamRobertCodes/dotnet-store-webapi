using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EStoreWebApi.Shared.Responses;

public class ErrorResponse
{
    [JsonInclude]
    public Dictionary<string, List<string>> Errors;

    public static ErrorResponse Make(ModelStateDictionary modelState)
    {
        var errors = new Dictionary<string, List<string>>();

        foreach (var (key, values) in modelState)
        {
            errors[key] = values.Errors
                .Select(e => e.ErrorMessage)
                .ToList();
        }
        
        return new ErrorResponse()
        {
            Errors = errors,
        };
    }
}