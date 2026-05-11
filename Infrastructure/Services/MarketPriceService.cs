using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Common.Interfaces;
using Domain.Entities.Catalog;

namespace Infrastructure.Services
{
    public class MarketPriceService : IMarketPriceService
    {
        private readonly IMarketPriceRepository _marketPriceRepository;
        private readonly IGoldPriceApiClient _goldApi;
        private readonly IUnitOfWork _unitOfWork;

        public MarketPriceService(
            IMarketPriceRepository marketPriceRepository,
            IGoldPriceApiClient goldApi,
            IUnitOfWork unitOfWork)
        {
            _goldApi = goldApi;
            _marketPriceRepository = marketPriceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<MarketPriceSnapshot> GetLatestSnapshotAsync(
        CancellationToken cancellationToken)
        {
            var latest = await _marketPriceRepository.GetLatestAsync(cancellationToken);
            if (latest is null)
                throw new InvalidOperationException("هیچ snapshot قیمتی در سیستم وجود ندارد.");

            return latest;
        }

        public async Task RefreshFromExternalApiAsync(
            CancellationToken cancellationToken)
        {
            var price = await _goldApi.GetLatestPriceAsync(cancellationToken);

            var snapshot = new MarketPriceSnapshot(price);

            await _marketPriceRepository.AddAsync(snapshot, cancellationToken);
            await _unitOfWork.SaveChangeAsync();
        }
    }
}
