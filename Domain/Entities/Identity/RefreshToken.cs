using Domain.Common;

namespace Domain.Entities.Identity
{
    public class RefreshToken : BaseEntity  
    {
        public Guid UserId { get; private set; }
        public string TokenHash { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public Guid? ReplacedByTokenId { get; private set; }

        // Navigation
        public User User { get; private set; }

        private RefreshToken() { }

        public RefreshToken(
            Guid userId, 
            string tokenHash, 
            DateTime expiresAt)
        {
            UserId = userId;
            TokenHash = tokenHash;
            ExpiresAt = expiresAt;
        }

        public bool IsExpired(DateTime now) => now >= ExpiresAt;
        public bool IsRevoked => RevokedAt != null;
        public bool IsActive(DateTime now) => !IsExpired(now) && !IsRevoked;

        public void Revoke(DateTime now, string? reason = null)
        {
            if (RevokedAt != null)
                return; 

            RevokedAt = now;
        }
        public void SetReplacedBy(Guid newTokenId)
        {
            ReplacedByTokenId = newTokenId;
        }

    }
}