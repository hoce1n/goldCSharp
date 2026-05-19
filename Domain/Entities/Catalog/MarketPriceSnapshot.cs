using Domain.Common;

namespace Domain.Entities.Catalog
{
    public class MarketPriceSnapshot : BaseEntity
    {
        public long Price18PerGram { get; private set; }

        private MarketPriceSnapshot() { }

        public MarketPriceSnapshot(long price18PerGram)
        {
            Price18PerGram = price18PerGram;
        }

    }
}
