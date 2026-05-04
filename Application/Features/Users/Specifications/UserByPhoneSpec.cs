using Ardalis.Specification;
using Domain.Entities.Identity;
using Domain.ValueObjects;

namespace Application.Features.Users.Specifications
{
    public class UserByPhoneSpec : Specification<User>, ISingleResultSpecification<User>
    {
        public UserByPhoneSpec(PhoneNumber phone)
        {
            Query
                .Where(u => u.PhoneNumber == phone)
                .Include(u => u.RefreshTokens);
        }
    }
}
