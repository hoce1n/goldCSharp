using Application.Abstractions.Repositories;
using Domain.Entities.Quote;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class QuoteRepository : IQuoteRepository
    {
        private readonly AppDbContext _context;
        public QuoteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Quote quote, CancellationToken cancellationToken)
        {
            await _context.Quotes.AddAsync(quote, cancellationToken);
        }

        public async Task<Quote?> GetByIdAsync(Guid? id, CancellationToken cancellationToken)
        {
            return await _context.Quotes
                        .FirstOrDefaultAsync(q => q.Id == id);
        }
    }
}
