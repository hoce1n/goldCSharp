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
            Id = Guid.NewGuid();
            UserId = userId;
            TokenHash = tokenHash;
            ExpiresAt = expiresAt;
            CreatedAt = DateTime.UtcNow;
        }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsRevoked => RevokedAt != null;
        public bool IsActive => !IsExpired && !IsRevoked;

        public void Revoke()
        {
            RevokedAt = DateTime.UtcNow;
        }
        public void SetReplacedBy(Guid newTokenId)
        {
            ReplacedByTokenId = newTokenId;
        }

    }
}