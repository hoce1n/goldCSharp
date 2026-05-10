namespace Application.Features.Catalog.MarketPrices.Queries.GetLatest
{
    public sealed class GetLatestMarketPriceResponse
    {
        public decimal PricePerGram { get; init; }
        public DateTime PriceTimestamp { get; init; }
    }

}
