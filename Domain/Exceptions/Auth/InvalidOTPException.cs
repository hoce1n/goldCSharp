using Domain.Common.Errors;
using Domain.Exceptions;

namespace Domain.Excetions.Auth
{
    public class InvalidOTPException : DomainException
    {
        public InvalidOTPException() 
            : base(ErrorCodes.OTP.Invalid, "Invalid OTP.")
        { }
    }
}
