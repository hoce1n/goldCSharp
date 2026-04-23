using Domain.Common.Errors;

namespace Domain.Exceptions.Auth
{
    public class OTPAlreadyUsedException : DomainException
    {
        public OTPAlreadyUsedException()
            : base(ErrorCodes.OTP.OTPAlreadyUsed, "کد تایید قبلا استفاده شده است.")
        {
        }
    }
}