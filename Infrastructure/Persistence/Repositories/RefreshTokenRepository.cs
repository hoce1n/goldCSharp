using Domain.Entities.Identity;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;

        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RefreshToken token)
        {
            await _context.AddAsync(token);
        }

        public async Task<RefreshToken?> GetByHashAsync(string hash)
        {
            return await _context.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.TokenHash == hash);
        }


        public Task UpdateAsync(RefreshToken token)
        {
            _context.RefreshTokens.Update(token);
            return Task.CompletedTask;
        }
    }
}
