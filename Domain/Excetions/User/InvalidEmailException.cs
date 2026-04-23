using Domain.Common.Errors;

namespace Domain.Exceptions.User
{
    public class InvalidEmailException : DomainException
    {
        public InvalidEmailException()
            : base(ErrorCodes.User.InvalidEmail, "The provided Email is not valid.")
        {   
        }
    }
}