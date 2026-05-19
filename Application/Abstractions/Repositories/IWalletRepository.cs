using Domain.Entities.Wallet;

namespace Application.Abstractions.Repositories
{
    public interface IWalletRepository
    {
        Task<Wallet?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task AddAsync(Wallet wallet, CancellationToken cancellationToken);
        void Update(Wallet wallet);
    }
}
