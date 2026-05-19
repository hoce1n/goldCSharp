using Domain.Entities.Wallet;

namespace Application.Abstractions.Repositories
{
    public interface IWalletLedgerRepository
    {
        Task AddAsync(WalletLedger walletLedger, CancellationToken cancellationToken); 
    }
}
