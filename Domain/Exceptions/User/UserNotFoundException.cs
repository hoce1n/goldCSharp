using Domain.Common.Errors;

namespace Domain.Exceptions.User
{
    public class UserNotFoundException : DomainException
    {
        public UserNotFoundException()
            : base(ErrorCodes.User.NotFound, "User not found.")
        {
        }
    }
}