using Ardalis.Specification;
using Domain.Entities.Identity;

namespace Application.Features.Users.Specifications
{
    public class UserByPhoneSpec : Specification<User>, ISingleResultSpecification
    {
        public UserByPhoneSpec(string phone)
        {
            Query
                .Where(u => u.PhoneNumber.Value == phone)
                .Include(u => u.Roles)
                .Include(u => u.RefreshTokens);
        }
    }
}
