namespace Domain.Events.User
{
    public class UserRegisteredEvent : DomainEvent
    {
        public Guid UserId { get; }

        public UserRegisteredEvent(Guid userId)
        {
            UserId = userId;
        }
    }
} 