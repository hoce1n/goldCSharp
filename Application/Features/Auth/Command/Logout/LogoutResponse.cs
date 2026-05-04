namespace Application.Features.Auth.Command.Logout
{
    public sealed class LogoutResponse
    {
        public DateTime RevokedAtUtc { get; init; }
        public string? Message { get; init; }
    }
}
