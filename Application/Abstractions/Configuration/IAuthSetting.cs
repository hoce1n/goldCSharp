namespace Application.Abstractions.Configuration
{
    public interface IAuthSetting
    {
        int MaxActiveRefreshTokens { get; }
        int RefreshTokenExpiryDays { get; }
        string JwtSecret { get; }
        int JwtExpiryMinutes { get; }
        string Issuer { get; }
        string Audience { get; }
    }
}
