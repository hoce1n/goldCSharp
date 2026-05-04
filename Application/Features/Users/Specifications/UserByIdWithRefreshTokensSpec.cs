using Ardalis.Specification;
using Domain.Entities.Identity;

namespace Application.Features.Users.Specifications
{
    public class UserByIdWithRefreshTokensSpec
        : Specification<User>, ISingleResultSpecification<User>
    {
        public UserByIdWithRefreshTokensSpec(Guid id)
        {
            Query
                .Where(u => u.Id == id)
                .Include(u => u.RefreshTokens);
        }
    }
}
