using Domain.Abstractions.Security;
using System.Security.Cryptography;

namespace Infrastructure.Security
{
    public class TokenGenerator : ITokenGenerator
    {
        public string GenerateRandomToken()
        {
            var bytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);

            return Convert.ToBase64String(bytes);
        }
    }
}
