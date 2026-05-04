namespace Domain.Events.Coin
{
    public class CoinUpdatedEvent : DomainEvent
    {
        public Guid CoinId { get; }

        public CoinUpdatedEvent(Guid coinId)
        {
            CoinId = coinId;
        }
    }
}
