using Application.Abstractions.Messaging;
using Application.Common.Result;

namespace Application.Features.Catalog.MarketPrices.Queries.History
{
    public sealed record GetMarketPriceHistoryQuery(
        MarketPriceHistoryRange Range
    ) : IQuery<Result<GetMarketPriceHistoryResponse>>;

    public enum MarketPriceHistoryRange
    {
        Day,
        Week,
        Month
    }
}
