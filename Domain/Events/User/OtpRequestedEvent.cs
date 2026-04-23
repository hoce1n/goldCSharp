namespace Domain.Events.User
{
    public class OtpRequestedEvent : DomainEvent
    {
        public Guid UserId { get; }
        public string PhoneNumber { get; }

        public OtpRequestedEvent(Guid userId, string phoneNumber)
        {
            UserId = userId;
            PhoneNumber = phoneNumber;
        }
    }
}