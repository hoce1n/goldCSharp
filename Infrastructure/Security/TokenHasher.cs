using Domain.Abstractions.Security;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Security
{
    internal class TokenHasher : ITokenHasher
    {
        public string Hash(string rawToken)
        {
            if (string.IsNullOrWhiteSpace(rawToken))
                throw new ArgumentException("Token cannot be empty");

            using var sha = SHA256.Create();

            var bytes = Encoding.UTF8.GetBytes(rawToken);
            var hash = sha.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }

        public bool Verify(string rawToken, string hash)
        {
            var computed = Hash(rawToken);

            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(computed),
                Encoding.UTF8.GetBytes(hash)
            );
        }
    }
}
