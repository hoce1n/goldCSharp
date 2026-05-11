using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Enums.Catalog;
using Domain.Enums.Quote;
using Domain.Services.Catalog;

namespace Application.Services.Catalog
{
    public class PricingService : IPricingService
    {
        private readonly IMarketPriceService _marketPriceService;
        private readonly IMeltedGoldRepository _meltedGoldRepository;
        private readonly ICoinRepository _coinRepository;

        public PricingService(
            IMarketPriceService marketPriceService,
            IMeltedGoldRepository meltedGoldRepository,
            ICoinRepository coinRepository)
        {
            _marketPriceService = marketPriceService;
            _meltedGoldRepository = meltedGoldRepository;
            _coinRepository = coinRepository;
        }

        public async Task<(long unitPrice, Guid snapshotId)> GetPriceAsync(
            Guid productId,
            ProductType productType,
            QuoteSide side,
            CancellationToken cancellationToken)
        {
            return productType switch
            {
                ProductType.Coin =>
                    await GetCoinPriceAsync(productId, cancellationToken),

                ProductType.MeltedGold =>
                    await GetMeltedGoldPriceAsync(side, cancellationToken),

                _ => throw new NotSupportedException("Unsupported product type")
            };
        }

        private async Task<(long unitPrice, Guid snapshotId)> GetCoinPriceAsync(
            Guid coinId,
            CancellationToken cancellationToken)
        {
            var coin = await _coinRepository.GetByIdAsync(coinId, cancellationToken);

            var snapshot = await _marketPriceService.GetLatestSnapshotAsync(cancellationToken);

            var price = PricingEngine.CalculateFinalPrice(
                coin.WeightInSoot,
                coin.MintingFee,
                snapshot.Price18PerGram);

            return (price, snapshot.Id);
        }

        private async Task<(long unitPrice, Guid snapshotId)> GetMeltedGoldPriceAsync(
            QuoteSide side,
            CancellationToken cancellationToken)
        {
            var snapshot = await _marketPriceService.GetLatestSnapshotAsync(cancellationToken);

            var meltedGold = await _meltedGoldRepository.GetActiveOrCreateAsync(cancellationToken);

            var basePrice = snapshot.Price18PerGram;

            long finalPrice = side == QuoteSide.Buy
                ? (long)Math.Round(basePrice + meltedGold.BuyFeePerGram)
                : (long)Math.Round(basePrice - meltedGold.SellFeePerGram);

            return (finalPrice, snapshot.Id);
        }
    }
}
