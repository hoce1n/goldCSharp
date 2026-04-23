using Domain.Common.Errors;

namespace Application.Common.Exceptions
{
    public class ForbiddenException : Exception
    {
        public string Code => ErrorCodes.Auth.Forbidden;
        public ForbiddenException(string message = "Access to this resource is forbidden.")
            : base(message) 
        { }
    }
}
