namespace Domain.Events.Coin
{
    public class CoinActivatedEvent : DomainEvent
    {
        public Guid CoinId { get; }

        public CoinActivatedEvent(Guid coinId)
        {
            CoinId = coinId;
        }
    }
}
