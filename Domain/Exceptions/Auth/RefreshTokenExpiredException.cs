using Domain.Common.Errors;
using Domain.Exceptions;

namespace Domain.Excetions.Auth
{
    public class RefreshTokenExpiredException : DomainException
    {
        public RefreshTokenExpiredException()
            : base(ErrorCodes.RefreshToken.Expired, "RefreshToken Expired.")
        { }
    }
}

