using Application.Abstractions.Repositories;
using Domain.Entities.Catalog;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class CoinRepository : ICoinRepository
    {
        private readonly AppDbContext _context;

        public CoinRepository(AppDbContext context)
        {
            _context = context;
        }

        // Commands
        public async Task<Coin> AddAsync(Coin coin, CancellationToken cancellationToken = default)
        {
            await _context.Coins.AddAsync(coin, cancellationToken);
            return coin;
        }

        public async Task UpdateAsync(Coin coin, CancellationToken cancellationToken = default)
        {
            _context.Coins.Update(coin);
        }

        public async Task DeleteAsync(Coin coin, CancellationToken cancellationToken = default)
        {
            _context.Coins.Remove(coin);
        }

        // Queries
        public async Task<Coin?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Coins
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Coin?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Coins
                .FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
        }

        public async Task<List<Coin>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Coins
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Coin>> GetActiveCoinsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Coins
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Coins
                .AnyAsync(c => c.Name == name, cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(string name, Guid excludeId, CancellationToken cancellationToken = default)
        {
            return await _context.Coins
                .AnyAsync(c => c.Name == name && c.Id != excludeId, cancellationToken);
        }

        public async Task<(List<Coin> Coins, int TotalCount)> GetPagedAsync(
            bool? isActive, 
            int pageNumber, 
            int pageSize, 
            CancellationToken cancellationToken = default)
        {
            var query = _context.Coins.AsQueryable();

            if (isActive.HasValue)
            {
                query = query.Where(c => c.IsActive == isActive.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var coins = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (coins, totalCount);
        }
    }
}
