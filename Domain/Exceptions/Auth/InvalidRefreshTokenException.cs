using Domain.Common.Errors;
using Domain.Exceptions;

namespace Domain.Excetions.Auth
{
    public class InvalidRefreshTokenException : DomainException
    {
        public InvalidRefreshTokenException()
            : base(ErrorCodes.RefreshToken.Invalid, "Invalid Refresh Token.")
        { }
    }
}
