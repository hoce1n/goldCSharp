using Domain.Entities.Catalog;

namespace Application.Abstractions.Services
{
    public interface IMarketPriceService
    {
        Task<MarketPriceSnapshot> GetLatestSnapshotAsync(
            CancellationToken cancellationToken);

        Task RefreshFromExternalApiAsync(
            CancellationToken cancellationToken);
    }
}
