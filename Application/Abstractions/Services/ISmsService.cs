using Domain.ValueObjects;

namespace Application.Abstractions.Services
{
    public interface ISMSService
    {
        Task SendOtpAsync(PhoneNumber phoneNumber, string code, CancellationToken ct);
    }
}