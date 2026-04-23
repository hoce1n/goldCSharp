using Domain.Common.Errors;

namespace Domain.Exceptions.Auth
{
    public class OTPNotFoundException : DomainException
    {
        public OTPNotFoundException()
            : base(ErrorCodes.OTP.NotFound, "OTP not found!")
        {
        }
    }
}