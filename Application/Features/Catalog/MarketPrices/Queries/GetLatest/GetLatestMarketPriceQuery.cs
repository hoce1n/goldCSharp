using Application.Abstractions.Messaging;
using Application.Common.Result;

namespace Application.Features.Catalog.MarketPrices.Queries.GetLatest
{
    public sealed record GetLatestMarketPriceQuery
    : IQuery<Result<GetLatestMarketPriceResponse>>;
}
