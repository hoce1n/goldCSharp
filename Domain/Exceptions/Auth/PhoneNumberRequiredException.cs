using Domain.Common.Errors;

namespace Domain.Exceptions.Auth
{
    public class PhoneNumberRequiredException : DomainException
    {
        public PhoneNumberRequiredException()
            : base(ErrorCodes.User.PhoneNumberRequired, "شماره تلفن الزامی‌ست.")
        {
        }
    }
}