using Microsoft.AspNetCore.Mvc;

namespace EStoreWebApi.Shared.Requests.Base;

public class PaginatedRequest
{
    [FromQuery(Name = "perPage")]
    public int PerPage { get; set; } = 10;

    [FromQuery(Name = "page")]
    public int Page { get; set; } = 1;
}

