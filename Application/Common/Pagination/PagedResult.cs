namespace Application.Common.Pagination
{
    public sealed class PagedResult<T>
    {
        public IReadOnlyCollection<T> Items { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalCount { get; init; }

        public PagedResult(
            IReadOnlyCollection<T> items,
            int page,
            int pageSize,
            int totalCount)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
        }
    }

}
