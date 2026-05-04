using Domain.Common.Errors;

namespace Domain.Exceptions.Auth
{
    public class TooManyAttemptException : DomainException
    {
        public TooManyAttemptException()
            : base(ErrorCodes.OTP.TooManyRequests, "Too many OTP Requests. Try later.")
        {
        }
    }
}