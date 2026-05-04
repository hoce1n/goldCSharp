namespace Application.Features.Auth.Command.RefreshToken
{
    public class RefreshTokenResponse
    {
        public string AccessToken { get; init; }
        public Guid UserId { get; init; }
        public string PhoneNumber { get; init; }
        public bool IsProfileCompleted { get; init; }
        public int VerificationLevel { get; init; }
        public string Status { get; init; }
    }
}
