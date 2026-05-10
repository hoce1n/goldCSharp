using Domain.Entities.Catalog;

namespace Application.Abstractions.Repositories
{
    public interface IMarketPriceRepository
    {
        Task AddAsync(MarketPriceSnapshot snapshot, CancellationToken cancellationToken = default);
        Task<MarketPriceSnapshot?> GetLatestAsync(CancellationToken cancellationToken = default);
        Task<List<MarketPriceSnapshot>> GetRangeAsync(
            DateTime from,
            DateTime to,
            CancellationToken cancellationToken);
    }
}
