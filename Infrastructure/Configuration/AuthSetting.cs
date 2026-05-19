using Application.Abstractions.Configuration;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Configuration
{
    public class AuthSetting : IAuthSetting
    {
        public int MaxActiveRefreshTokens { get; }
        public int RefreshTokenExpiryDays { get; }
        public string JwtSecret { get; }
        public int JwtExpiryMinutes { get; }
        public string Issuer { get; }
        public string Audience { get; }

        public AuthSetting(IConfiguration configuration)
        {
            var section = configuration.GetSection("Auth");
            MaxActiveRefreshTokens = section.GetValue<int>("MaxActiveRefreshTokens", 5);
            RefreshTokenExpiryDays = section.GetValue<int>("RefreshTokenExpiryDays", 14);
            JwtSecret = section.GetValue<string>("JwtSecret")
                ?? throw new InvalidOperationException("JwtSecret is required");
            JwtExpiryMinutes = section.GetValue<int>("JwtExpiryMinutes", 60);
            Issuer = section.GetValue<string>("Issuer") ?? "YourApp";
            Audience = section.GetValue<string>("Audience") ?? "YourAppUsers";
        }
    }
}
