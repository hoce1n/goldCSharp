namespace Domain.Events
{
    public abstract class DomainEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
        
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}