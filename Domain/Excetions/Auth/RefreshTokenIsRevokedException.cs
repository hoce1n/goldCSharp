using Domain.Common.Errors;
using Domain.Exceptions;

namespace Domain.Excetions.Auth
{
    public class RefreshTokenIsRevokedException : DomainException
    {
        public RefreshTokenIsRevokedException()
            : base(ErrorCodes.RefreshToken.Revoked, "RefreshToken Is Revoked.")
        { }
    }
}

