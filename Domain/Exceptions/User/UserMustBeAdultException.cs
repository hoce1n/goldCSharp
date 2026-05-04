using Domain.Common.Errors;

namespace Domain.Exceptions.User
{
    public class UserMustBeAdultException : DomainException
    {
        public UserMustBeAdultException()
            : base(ErrorCodes.User.MustBeAdult, "سن باید بالای هیجده سال باشد.")
        {
        }
    }
}