using Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Security
{
    public class UserContext : CurrentUser, IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public string? PhoneNumber =>
            User?.FindFirstValue("phone_number");

        public string? NationalCode =>
            User?.FindFirstValue("national_code");

        public int? VerificationLevel
        {
            get
            {
                var value = User?.FindFirstValue("verification_level");
                if (int.TryParse(value, out var level))
                    return level;

                return null;

            }
        }

        IReadOnlyCollection<string> IUserContext.Roles => throw new NotImplementedException();
    }
}