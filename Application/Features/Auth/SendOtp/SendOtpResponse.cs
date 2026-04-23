namespace Application.Features.Auth.SendOtp
{
    public sealed record SendOtpResponse(
        string PhoneNumber,
        int ExpireInSeconds
    );
}
