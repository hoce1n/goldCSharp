namespace Domain.Events.User
{
    public class UserActivatedEvent : DomainEvent
    {
        public Guid UserId { get; }
        public UserActivatedEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}