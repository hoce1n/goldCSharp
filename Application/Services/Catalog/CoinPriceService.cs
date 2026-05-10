using Application.Abstractions.Services;
using Domain.Entities.Catalog;
using Domain.Services.Catalog;

namespace Application.Services.Catalog
{
    public class CoinPriceService : ICoinPriceService
    {
        private readonly MarketPriceService _marketPriceServcie;

        public CoinPriceService(MarketPriceService marketPriceServcie)
        {
            _marketPriceServcie = marketPriceServcie;
        }

        public async Task<long> CalculateCoinPriceAsync(
            Coin coin,
            CancellationToken cancellationToken)
        {
            var snapshot = await _marketPriceServcie.GetLatestSnapshotAsync(cancellationToken);

            return PricingEngine.CalculateFinalPrice(
                coin.WeightInSoot,
                coin.MintingFee,
                snapshot.Price18PerGram);
        }
    }
}
