using Application.Abstractions.Repositories;
using Domain.Entities.Catalog;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class MeltedGoldRepository : IMeltedGoldRepository
    {
        private readonly AppDbContext _context;

        public MeltedGoldRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MeltedGold> GetActiveOrCreateAsync(CancellationToken cancellationToken = default)
        {
            var meltedGold = await _context.MeltedGolds
                .FirstOrDefaultAsync(mg => mg.IsActive, cancellationToken);

            if (meltedGold is null)
            {
                meltedGold = new MeltedGold(
                    buyFeePerGram: 25000,
                    sellFeePerGram: 20000,
                    minTradeAmount: 0.05m
                );

                _context.MeltedGolds.Add(meltedGold);
            }

            return meltedGold;
        }

        public void Update(MeltedGold meltedGold)
        {
            _context.MeltedGolds.Update(meltedGold);
        }
    }

}
