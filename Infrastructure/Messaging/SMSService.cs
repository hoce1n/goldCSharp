
using Application.Abstractions.Services;
using Domain.ValueObjects;

namespace Infrastructure.Services
{
    public class SMSService : ISMSService
    {
        public async Task SendOtpAsync(PhoneNumber phoneNumber, string message, CancellationToken cancellationToken)
        {
            Console.WriteLine($"SMS to {phoneNumber}: {message}");
        }
    }
}
