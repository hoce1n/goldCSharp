using Domain.Common;

namespace Domain.Entities.Catalog
{
    public class MarketPriceSnapshot : BaseEntity
    {
        public long Price18PerGram { get; private set; }

        private MarketPriceSnapshot() { }

        public MarketPriceSnapshot(long price18PerGram)
        {
            Id = Guid.NewGuid();
            Price18PerGram = price18PerGram;
            CreatedAt = DateTime.UtcNow;
        }

    }
}
