using Application.Abstractions.Authentication;
using Application.Abstractions.Configuration;
using Domain.Entities.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Security
{
    public class TokenService : ITokenService
    {
        private readonly IAuthSetting _authSetting;

        public TokenService(IAuthSetting authSetting)
        {
            _authSetting = authSetting;
        }

        public string GenerateAccessToken(User user)
        {
            var secret = _authSetting.JwtSecret;
            var issuer = _authSetting.Issuer;
            var audience = _authSetting.Audience;
            var expiresMinutes = _authSetting.JwtExpiryMinutes;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim("full_name", user.FullName),
                new Claim("phone_number", user.PhoneNumber.Value),
                new Claim("status", user.Status.ToString()),
                new Claim("verification", ((int)user.VerificationLevel).ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}