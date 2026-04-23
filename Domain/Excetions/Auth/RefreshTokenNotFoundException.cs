using Domain.Common.Errors;
using Domain.Exceptions;

namespace Domain.Excetions.Auth
{
    public class RefreshTokenNotFoundException : DomainException
    {
        public RefreshTokenNotFoundException()
            : base(ErrorCodes.RefreshToken.NotFound, "RefreshToken not found.")
        { }
    }
}
