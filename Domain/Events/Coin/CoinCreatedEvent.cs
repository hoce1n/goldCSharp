namespace Domain.Events.Coin
{
    public class CoinCreatedEvent : DomainEvent
    {
        public Guid CoinId { get; }
        public string CoinName { get; }

        public CoinCreatedEvent(Guid coinId, string coinName)
        {
            CoinId = coinId;
            CoinName = coinName;
        }
    }
}
