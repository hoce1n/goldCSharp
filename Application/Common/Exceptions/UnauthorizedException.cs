using Domain.Common.Errors;

namespace Application.Common.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public string Code => ErrorCodes.Auth.Unauthorized;
        public UnauthorizedException(string message = "you are not authorized")
            : base(message)
        { }
    }
}
