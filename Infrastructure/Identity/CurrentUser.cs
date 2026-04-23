using Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Identity
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public Guid? UserId
        {
            get
            {
                var raw =
                    User?.FindFirstValue(ClaimTypes.NameIdentifier) ??
                    User?.FindFirstValue("sub");

                if (Guid.TryParse(raw, out var guid))
                    return guid;

                return null;
            }
        }


        public string? Role =>
            User?.FindFirstValue(ClaimTypes.Role);

        public bool IsAuthenticated =>
            User?.Identity?.IsAuthenticated ?? false;

    }
}
