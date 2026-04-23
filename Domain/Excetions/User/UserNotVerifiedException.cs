using Domain.Common.Errors;

namespace Domain.Exceptions.User
{
    public class UserNotVerifiedException : DomainException
    {
        public UserNotVerifiedException()
            : base(ErrorCodes.User.NotVerified, "هنوز احراز هویت نشده اید.")
        {
        }
    }
}