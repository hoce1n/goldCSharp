namespace API.Contracts.Auth
{
    public class VerifyOtpResponse
    {
        public Guid UserId { get; set; }
        public bool IsNewUser { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

    }
}
