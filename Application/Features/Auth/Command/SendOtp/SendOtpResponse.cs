namespace Application.Features.Auth.Command.SendOtp
{
    public sealed record SendOtpResponse(
        string PhoneNumber,
        int ExpireInSeconds
    );
}