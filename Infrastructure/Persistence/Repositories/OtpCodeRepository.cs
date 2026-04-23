using Application.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Identity;
using Domain.ValueObjects;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class OtpCodeRepository : IOtpCodeRepository
    {
        private readonly AppDbContext _context;

        public OtpCodeRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task AddAsync(OtpCode otp)
        {
            await _context.OtpCodes.AddAsync(otp);
        }

        public async Task<OtpCode?> GetLatestAsync(PhoneNumber phoneNumber)
        {
            return await _context.OtpCodes
                .Where(o => o.PhoneNumber.Value == phoneNumber.Value)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<OtpCode?> GetActiveCodeAsync(
            PhoneNumber phoneNumber,
            CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            return await _context.OtpCodes
                .Where(x =>
                    x.PhoneNumber.Value == phoneNumber.Value &&
                    x.ExpiresAt > now &&
                    x.UsedAt == null)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<int> CountRecentAsync(
            PhoneNumber phoneNumber,
            DateTime since)
        {
            return await _context.OtpCodes
                .Where(x =>
                    x.PhoneNumber.Value == phoneNumber.Value &&
                    x.CreatedAt >= since)
                .CountAsync();
        }

        public Task UpdateAsync(OtpCode otp)
        {
            _context.OtpCodes.Update(otp);
            return Task.CompletedTask;
        }
    }
}
