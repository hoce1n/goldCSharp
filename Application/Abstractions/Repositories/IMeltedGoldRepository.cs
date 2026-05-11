using Domain.Entities.Catalog;

namespace Application.Abstractions.Repositories
{
    public interface IMeltedGoldRepository
    {
        Task<MeltedGold> GetActiveOrCreateAsync(CancellationToken cancellationToken = default);
        void Update(MeltedGold meltedGold);

    }

}
