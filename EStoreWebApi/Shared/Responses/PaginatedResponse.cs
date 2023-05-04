using EStoreWebApi.Shared.Requests.Base;

namespace EStoreWebApi.Shared.Responses;

public class PaginatedResponse<T>
{
    public List<T> Data { get; set; }

    public PaginationResponse Pagination { get; set; }
}

public class PaginationResponse
{
    public int Page { get; set; }

    public int PerPage { get; set; }

    public int TotalCount { get; set; }

    public int? PreviousPage => Page == 1 ? null : Page - 1;

    public int? NextPage => Page + 1 > NumberOfPages ? null : Page + 1;

    public double NumberOfPages => Math.Floor(TotalCount / (float)PerPage);

    public static PaginationResponse FromRequest(PaginatedRequest request, int count)
    {
        return new PaginationResponse()
        {
            Page = request.Page,
            PerPage = request.PerPage,
            TotalCount = count,
        };
    }
}
