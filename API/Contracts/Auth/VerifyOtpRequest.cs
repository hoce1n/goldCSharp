namespace API.Contracts.Auth
{
    public class VerifyOtpRequest
    {
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
    }
}
