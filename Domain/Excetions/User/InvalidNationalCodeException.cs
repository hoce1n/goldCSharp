using Domain.Common.Errors;

namespace Domain.Exceptions.User
{
    public class InvalidNationalCodeException : DomainException
    {
        public InvalidNationalCodeException()
            : base(ErrorCodes.User.InvalidNationalCode, "The provided National Code is invalid or expired.")
        {
        }
    }
}