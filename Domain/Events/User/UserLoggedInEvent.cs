namespace Domain.Events.User
{
    public class UserLoggedInEvent : DomainEvent
    {
        public Guid UserId { get; }

        public UserLoggedInEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}