using Domain.Entities.Identity;

namespace Application.Abstractions.Authentication
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
    }
}