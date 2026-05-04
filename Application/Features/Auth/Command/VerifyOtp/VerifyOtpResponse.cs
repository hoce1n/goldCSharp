namespace Application.Features.Auth.Command.VerifyOtp
{
    public record VerifyOtpResponse(
        string AccessToken,
        string RawRefreshToken,
        DateTime AccessTokenExpiresAt,
        bool IsProfileCompleted
    );
}
