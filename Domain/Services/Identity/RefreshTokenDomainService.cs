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

        public (string rawToken, RefreshToken tokenEntity) Generate(User user, TimeSpan expiry)
        {
            var raw = _tokenGenerator.GenerateRandomToken();

            var hash = _tokenHasher.Hash(raw);

            var entity = new RefreshToken(user.Id, hash, DateTime.UtcNow.Add(expiry));

            return (raw, entity);
        }

        public (string raw, RefreshToken newToken) Rotate(User user, RefreshToken old)
        {
            old.Revoke();

            var (rawNew, newToken) = Generate(user, TimeSpan.FromDays(14));

            old.SetReplacedBy(newToken.Id);

            return (rawNew, newToken);
        }

        public void EnforceReplayDefense(User user, string oldTokenValue)
        {
            var activeToken = user.GetActiveRefreshToken();
            if (activeToken == null)
                return;

            if (!_tokenHasher.Verify(oldTokenValue, activeToken.TokenHash))
            {
                foreach (var token in user.RefreshTokens)
                    token.Revoke();
            }
        }

        public void EnforceTokenLimit(User user, int maxActive = 5)
        {
            var activeTokens = user.RefreshTokens
                .Where(t => t.IsActive)
                .OrderByDescending(t => t.CreatedAt)
                .ToList();

            if (activeTokens.Count <= maxActive)
                return;

            foreach (var token in activeTokens.Skip(maxActive))
                token.Revoke();
        }
    }
}
