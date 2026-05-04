namespace Domain.Events.Coin
{
    public class CoinStockChangedEvent : DomainEvent
    {
        public Guid CoinId { get; }
        public int OldStock { get; }
        public int NewStock { get; }
        public int Change { get; }

        public CoinStockChangedEvent(Guid coinId, int oldStock, int newStock)
        {
            CoinId = coinId;
            OldStock = oldStock;
            NewStock = newStock;
            Change = newStock - oldStock;
        }
    }
}
