using Ardalis.Specification;
using Domain.Entities.Identity;
using Domain.ValueObjects;

namespace Application.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<User?> GetByPhoneNumberAsync(PhoneNumber phoneNumber);
        Task<User?> GetByIdWithRefreshTokensAsync(Guid userId, CancellationToken cancellationToken);
        Task AddAsync(User user, CancellationToken cancellationToken = default);
        Task UpdateAsync(User user, CancellationToken cancellationToken = default);
        //Task DeleteAsync(User user, CancellationToken cancellationToken = default);

    }
}