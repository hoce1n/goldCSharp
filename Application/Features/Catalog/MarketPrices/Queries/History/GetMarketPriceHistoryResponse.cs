namespace Application.Features.Catalog.MarketPrices.Queries.History
{
    public sealed class GetMarketPriceHistoryResponse
    {
        public IReadOnlyList<MarketPriceHistoryPoint> Points { get; init; } = [];
    }

    public sealed class MarketPriceHistoryPoint
    {
        public DateTime Timestamp { get; init; }
        public decimal Price { get; init; }
    }
}
