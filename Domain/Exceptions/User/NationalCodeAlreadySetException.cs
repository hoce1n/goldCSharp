using Domain.Common.Errors;

namespace Domain.Exceptions.User
{
    public class NationalCodeAlreadySetException : DomainException
    {
        public NationalCodeAlreadySetException()
            : base(ErrorCodes.User.NationalCodeAlreadySet, "کاربری با این کد ملی قبلا ثبت شده است.")
        {
        }
    }
}