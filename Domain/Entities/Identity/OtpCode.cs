using Domain.Common;
using Domain.Exceptions.Auth;
using Domain.ValueObjects;

namespace Domain.Entities.Identity
{
    public class OtpCode : BaseEntity
    {
        public PhoneNumber PhoneNumber { get; private set; }
        public string Code { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public DateTime? UsedAt { get; private set; }
        public int AttemptCount { get; private set; }

        private OtpCode() { }

        public OtpCode(
            PhoneNumber phoneNumber, 
            string code, 
            DateTime expiresAt)
        {
            PhoneNumber = phoneNumber;
            Code = code;
            ExpiresAt = expiresAt;
            AttemptCount = 0;
        }

        public bool IsExpired()
        {
            return DateTime.UtcNow > ExpiresAt;
        }

        public void MarkAsUsed()
        {
            if (UsedAt != null)
                throw new OTPAlreadyUsedException();

            UsedAt = DateTime.UtcNow;
        }

        public void IncreaseAttempt()
        {
            AttemptCount++;
        }

        public bool IsLockedOut(int maxAttempts) => AttemptCount >= maxAttempts;
        public void RecordFailedAttempt()
        {
            AttemptCount++;
        }
    }
}