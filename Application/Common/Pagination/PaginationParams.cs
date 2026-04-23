
namespace Application.Common.Pagination
{
    public sealed class PaginationParams
    {
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 20;
    }

}
