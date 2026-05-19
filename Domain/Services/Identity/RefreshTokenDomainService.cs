using Domain.Abstractions.Security;
using Domain.Entities.Identity;

namespace Domain.Services.Identity
{
    public class RefreshTokenDomainService
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ITokenHasher _tokenHasher;

        public RefreshTokenDomainService(
            ITokenGenerator tokenGenerator,
            ITokenHasher tokenHasher)
        {
            _tokenGenerator = tokenGenerator;
            _tokenHasher = tokenHasher;
        }

        public (string rawToken, RefreshToken tokenEntity) Generate(
            User user, 
            DateTime now,
            TimeSpan expiry)
        {
            var raw = _tokenGenerator.GenerateRandomToken();

            var hash = _tokenHasher.Hash(raw);

            var entity = new RefreshToken(user.Id, hash, now.Add(expiry));

            return (raw, entity);
        }

        public (string raw, RefreshToken newToken) Rotate
            (User user, 
            RefreshToken old, 
            DateTime now,
            TimeSpan expiry)
        {
            old.Revoke(now);

            var (rawNew, newToken) = Generate(user, now, expiry);

            old.SetReplacedBy(newToken.Id);

            return (rawNew, newToken);
        }

        public void EnforceReplayDefense(
            User user, 
            string oldTokenValue, 
            DateTime now)
        {
            var activeToken = user.GetActiveRefreshToken(now);
            if (activeToken == null)
                return;

            if (!_tokenHasher.Verify(oldTokenValue, activeToken.TokenHash))
            {
                foreach (var token in user.RefreshTokens)
                    token.Revoke(now);
            }
        }

        public void EnforceTokenLimit(User user, 
            int maxActive, DateTime now)
        {
            var activeTokens = user.RefreshTokens
                .Where(t => t.IsActive(now))
                .OrderByDescending(t => t.CreatedAt)
                .ToList();

            if (activeTokens.Count <= maxActive)
                return;

            foreach (var token in activeTokens.Skip(maxActive))
                token.Revoke(now);
        }
    }
}
