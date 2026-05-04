using Domain.Common.Errors;

namespace Domain.Exceptions.User
{
    public class UserBlockedException : DomainException
    {
        public UserBlockedException()
            : base(ErrorCodes.User.Blocked, "User is blocked and cannot perform this action.")
        {
        }
    }
}