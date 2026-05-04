using Domain.Common.Errors;

namespace Domain.Exceptions.User
{
    public class InvalidNationalCodeException : DomainException
    {
        public InvalidNationalCodeException()
            : base(ErrorCodes.User.InvalidNationalCode, "کد ملی نامعتبر می‌باشد.")
        {
        }
    }
}