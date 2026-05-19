using Application.Abstractions.Configuration;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Configuration
{
    public class OtpSettings : IOtpSettings
    {
        public int ExpiryMinutes { get; }
        public int RateLimitWindowMinutes { get; }
        public int MaxRequestsPerWindow { get; }
        public int CodeLength { get; }

        public OtpSettings(IConfiguration configuration)
        {
            var section = configuration.GetSection("Otp");
            ExpiryMinutes = section.GetValue<int>("ExpiryMinutes", 2);
            RateLimitWindowMinutes = section.GetValue<int>("RateLimitWindowMinutes", 10);
            MaxRequestsPerWindow = section.GetValue<int>("MaxRequestsPerWindow", 3);
            CodeLength = section.GetValue<int>("CodeLength", 6);
        }
    }
}