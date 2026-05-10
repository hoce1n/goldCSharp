using Domain.Entities.Catalog;

namespace Application.Abstractions.Services
{
    public interface ICoinPriceService
    {
        Task<long> CalculateCoinPriceAsync(Coin coin, CancellationToken cancellationToken);
    }
}
