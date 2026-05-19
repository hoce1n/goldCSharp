using Domain.Common;
using Domain.Exceptions.Auth;
using Domain.Excetions.Auth;
using Domain.ValueObjects;

namespace Domain.Entities.Identity
{
    public class OtpCode : BaseEntity
    {
        public PhoneNumber PhoneNumber { get; private set; }
        public string Code { get; private set; }
        public DateTime? LockedUntil { get; private set; }

        public DateTime ExpiresAt { get; private set; }
        public DateTime? UsedAt { get; private set; }
        public int AttemptCount { get; private set; }

        private OtpCode() { }

        private OtpCode(
            PhoneNumber phoneNumber,
            string code,
            DateTime expiryTime)
        {
            PhoneNumber = phoneNumber;
            Code = code;
            ExpiresAt = expiryTime;
        }

        public static OtpCode Create(
            PhoneNumber phone,
            string code,
            DateTime expiryTime)
        {
            if (string.IsNullOrWhiteSpace(code) ||
                code.Length != 6 ||
                !code.All(char.IsDigit))
                throw new InvalidOTPException();
            if (expiryTime <= DateTime.UtcNow)
                throw new OTPExpiredException();

            return new OtpCode(phone, code, expiryTime);
        }
        public bool IsExpired(DateTime now)
        {
            return now > ExpiresAt;
        }

        public void MarkAsUsed(DateTime now)
        {
            if (UsedAt != null)
                throw new OTPAlreadyUsedException();

            UsedAt = now;
        }

        public void IncreaseAttempt()
        {
            AttemptCount++;
        }


        public bool IsLockedOut(
            int maxAttempts, 
            DateTime now, 
            TimeSpan lockoutDuration)
        {
            if (LockedUntil.HasValue)
            {
                if (now < LockedUntil.Value)
                    return true;

                ResetLockout();
                return false;
            }

            if (AttemptCount >= maxAttempts)
            {
                LockedUntil = now.Add(lockoutDuration);
                return true;
            }

            return false;
        }

        private void ResetLockout()
        {
            LockedUntil = null;
            AttemptCount = 0;
        }


        public void RecordFailedAttempt()
        {
            AttemptCount++;
        }

        public static bool CanRequestNewOtp(
            IEnumerable<OtpCode> recentOtps, 
            DateTime now,
            TimeSpan rateLimitWindow,
            int maxRequestsPerWindow)
        {
            var hasActiveOtp = recentOtps.Any(o => !o.IsExpired(now));
            if (hasActiveOtp) return false;

            var recentRequests = recentOtps
                .Where(o => o.CreatedAt >= now - rateLimitWindow)
                .Count();

            return recentRequests < maxRequestsPerWindow;
        }

    }
}