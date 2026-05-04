namespace Domain.Events.Coin
{
    public class MintingFeeUpdatedEvent : DomainEvent
    {
        public Guid CoinId { get; }
        public decimal OldFee { get; }
        public decimal NewFee { get; }

        public MintingFeeUpdatedEvent(Guid coinId, decimal oldFee, decimal newFee)
        {
            CoinId = coinId;
            OldFee = oldFee;
            NewFee = newFee;
        }
    }
}
