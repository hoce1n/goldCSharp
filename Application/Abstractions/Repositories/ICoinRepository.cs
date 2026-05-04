using Domain.Entities.Catalog;

namespace Application.Abstractions.Repositories
{
    public interface ICoinRepository
    {
        Task<Coin> AddAsync(Coin coin, CancellationToken cancellationToken = default);
        Task UpdateAsync(Coin coin, CancellationToken cancellationToken = default);
        Task DeleteAsync(Coin coin, CancellationToken cancellationToken = default);

        Task<Coin?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Coin?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<List<Coin>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<List<Coin>> GetActiveCoinsAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<bool> ExistsByNameAsync(string name, Guid excludeId, CancellationToken cancellationToken = default);
        Task<(List<Coin> Coins, int TotalCount)> GetPagedAsync(
            bool? isActive,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default
         );
    }

}
