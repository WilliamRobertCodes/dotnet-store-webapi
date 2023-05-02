using EStoreWebApi.Shared.Requests.Base;
using Microsoft.AspNetCore.Mvc;

namespace EStoreWebApi.Features.Catalogue.Requests;

public class ListProductsRequest : PaginatedRequest
{
    [FromQuery(Name = "productCategories")]
    public List<int> ProductCategories { get; set; } = new();
}
