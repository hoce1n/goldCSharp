using Domain.Common.Errors;

namespace Domain.Exceptions.User
{
    public class InvalidPhoneNumberException : DomainException
    {
        public InvalidPhoneNumberException()
            : base(ErrorCodes.User.InvalidPhoneNumber, "The provided phone number is not valid.")
        {   
        }
    }
}