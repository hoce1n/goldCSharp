using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Common.Result;
using Domain.Common.Errors;

namespace Application.Features.Catalog.MarketPrices.Queries.History
{
    public sealed class GetMarketPriceHistoryQueryHandler
    : IQueryHandler<GetMarketPriceHistoryQuery, Result<GetMarketPriceHistoryResponse>>
    {
        private readonly IMarketPriceRepository _marketPriceRepository;

        public GetMarketPriceHistoryQueryHandler(IMarketPriceRepository marketPriceRepository)
        {
            _marketPriceRepository = marketPriceRepository;
        }

        public async Task<Result<GetMarketPriceHistoryResponse>> Handle(
            GetMarketPriceHistoryQuery request,
            CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            TimeSpan range = request.Range switch
            {
                MarketPriceHistoryRange.Day => TimeSpan.FromDays(1),
                MarketPriceHistoryRange.Week => TimeSpan.FromDays(7),
                MarketPriceHistoryRange.Month => TimeSpan.FromDays(30),
                _ => TimeSpan.FromDays(1)
            };

            TimeSpan bucket = request.Range switch
            {
                MarketPriceHistoryRange.Day => TimeSpan.FromMinutes(5),
                MarketPriceHistoryRange.Week => TimeSpan.FromHours(1),
                MarketPriceHistoryRange.Month => TimeSpan.FromHours(3),
                _ => TimeSpan.FromMinutes(5)
            };

            var from = now - range;

            // 1) Read raw snapshots
            var snapshots = await _marketPriceRepository.GetRangeAsync(from, now, cancellationToken);

            if (snapshots.Count == 0)
            {
                return Result<GetMarketPriceHistoryResponse>.Failure(
                    Error.Failure(ErrorCodes.General.NotFound, "دیتایی از قیمت طلا موجود نیست."));
            }

            // 2) Aggregate into buckets
            var aggregated = snapshots
                .GroupBy(x => new DateTime(
                    (x.CreatedAt.Ticks / bucket.Ticks) * bucket.Ticks,
                    DateTimeKind.Utc))
                .Select(g => new MarketPriceHistoryPoint
                {
                    Timestamp = g.Key,
                    Price = (decimal)g.Average(x => x.Price18PerGram)
                })
                .OrderBy(x => x.Timestamp)
                .ToList();

            var response = new GetMarketPriceHistoryResponse
            {
                Points = aggregated
            };

            return response;
        }
    }

}
