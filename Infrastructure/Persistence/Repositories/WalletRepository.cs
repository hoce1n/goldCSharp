using Application.Abstractions.Repositories;
using Domain.Entities.Wallet;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly AppDbContext _context;

        public WalletRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Wallet wallet, CancellationToken cancellationToken)
        {
            await _context.AddAsync(wallet, cancellationToken);
        }

        public async Task<Wallet?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Wallets
                .Include(x => x.Ledgers)
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        }

        public void Update(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
        }
    }
}
