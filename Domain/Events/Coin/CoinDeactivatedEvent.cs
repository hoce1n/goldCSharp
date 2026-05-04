namespace Domain.Events.Coin
{
    public class CoinDeactivatedEvent : DomainEvent
    {
        public Guid CoinId { get; }

        public CoinDeactivatedEvent(Guid coinId)
        {
            CoinId = coinId;
        }
    }
}
