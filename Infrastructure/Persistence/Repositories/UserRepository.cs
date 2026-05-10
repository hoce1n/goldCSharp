using Application.Abstractions.Repositories;
using Ardalis.Specification;
using Domain.Entities.Identity;
using Domain.ValueObjects;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            return _context.AddAsync(user, cancellationToken).AsTask();
        }

        public Task UpdateAsync(User user, CancellationToken cancellationToken)
        {
            _context.Users.Update(user);
            return Task.CompletedTask;
        }

        //public async Task<int> DeleteAsync(User user, CancellationToken cancellationToken = default)
        //{
        //    _context.Set<User>().Remove(user);
        //    return await _context.SaveChangesAsync(cancellationToken);
        //}

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<User?> GetByIdWithRefreshTokensAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }

        public async Task<User?> GetByPhoneNumberAsync(PhoneNumber phoneNumber)
        {
            if (phoneNumber == null || string.IsNullOrWhiteSpace(phoneNumber.Value))
                return null;

            return await _context.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }
    }
}
