using Application.Abstractions.Repositories;
using Domain.Entities.Wallet;
using Infrastructure.Persistence.Data;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class WalletLedgerRepository : IWalletLedgerRepository
    {
        private readonly AppDbContext _context;

        public WalletLedgerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(WalletLedger walletLedger, CancellationToken cancellationToken)
        {
            await _context.WalletLedgers.AddAsync(walletLedger, cancellationToken);
        }
    }
}
