using Application.Abstractions.Repositories;
using Domain.Entities.Catalog;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class MarketPriceRepository : IMarketPriceRepository
    {
        private readonly AppDbContext _context;

        public MarketPriceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            MarketPriceSnapshot snapshot,
            CancellationToken cancellationToken)
        {
            await _context.MarketPriceSnapshots.AddAsync(snapshot, cancellationToken);
        }

        public async Task<MarketPriceSnapshot?> GetLatestAsync(
            CancellationToken cancellationToken)
        {
            return await _context.MarketPriceSnapshots
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<MarketPriceSnapshot>> GetRangeAsync(
            DateTime from,
            DateTime to,
            CancellationToken cancellationToken)
        {
            return await _context.MarketPriceSnapshots
                .Where(x => x.CreatedAt >= from && x.CreatedAt <= to)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

    }
}