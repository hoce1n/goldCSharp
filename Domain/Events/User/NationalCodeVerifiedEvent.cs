using Domain.ValueObjects;

namespace Domain.Events.User
{
    public class NationalCodeVerifiedEvent : DomainEvent
    {
        public Guid UserId { get; }
        public NationalCode NationalCode { get; }

        public NationalCodeVerifiedEvent(Guid userId, NationalCode nationalCode)
        {
            UserId = userId;
            NationalCode = nationalCode;
        }
    }
}