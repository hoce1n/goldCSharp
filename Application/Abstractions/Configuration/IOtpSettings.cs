namespace Application.Abstractions.Configuration
{
    public interface IOtpSettings
    {
        int ExpiryMinutes { get; }
        int RateLimitWindowMinutes { get; }
        int MaxRequestsPerWindow { get; }
        int CodeLength { get; }

    }
}
