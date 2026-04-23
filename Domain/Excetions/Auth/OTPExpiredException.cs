using Domain.Common.Errors;
using Domain.Exceptions;

namespace Domain.Excetions.Auth
{
    public class OTPExpiredException : DomainException
    {
        public OTPExpiredException()
            : base(ErrorCodes.OTP.Expired, "OTP expired.")
        {
        }
    }
}
