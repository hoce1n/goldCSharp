using Domain.Enums.Catalog;
using Domain.Enums.Quote;

namespace Application.Abstractions.Services
{
    public interface IPricingService
    {
        Task<(long unitPrice, Guid snapshotId)> GetPriceAsync(
            Guid productId,
            ProductType productType,
            QuoteSide side,
            CancellationToken cancellationToken);
    }
}
