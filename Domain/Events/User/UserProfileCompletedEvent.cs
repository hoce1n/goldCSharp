namespace Domain.Events.User
{
    public class UserProfileCompletedEvent : DomainEvent
    {
        public Guid UserId { get; }
        public UserProfileCompletedEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}