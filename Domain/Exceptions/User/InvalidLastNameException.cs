using Domain.Common.Errors;

namespace Domain.Exceptions.User
{
    public class InvalidLastNameException : DomainException
    {
        public InvalidLastNameException() : base(ErrorCodes.User.InvalidLastName, "Last name is invalid.") { }
    }
}
