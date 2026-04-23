namespace Domain.Events.User
{
    public class UserBlockedEvent : DomainEvent
    {
        public Guid UserId { get; }

        public UserBlockedEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}