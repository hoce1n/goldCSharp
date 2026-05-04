using Domain.Common.Errors;

namespace Domain.Exceptions.User
{
    public class InvalidFirstNameException : DomainException
    {
        public InvalidFirstNameException() : base(ErrorCodes.User.InvalidFirstName, "Invalid First Name") { }
    }
}
