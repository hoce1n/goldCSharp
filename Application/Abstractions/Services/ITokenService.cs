using Domain.Entities.Identity;

namespace Application.Abstractions.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
    }
}