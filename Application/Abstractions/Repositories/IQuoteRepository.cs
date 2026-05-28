using Domain.Entities.Quote;

namespace Application.Abstractions.Repositories
{
    public interface IQuoteRepository
    {
        Task AddAsync(Quote quote, CancellationToken cancellationToken);
        Task<Quote?> GetByIdAsync(Guid? id, CancellationToken cancellationToken);
    }
}
