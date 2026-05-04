using Ardalis.Specification;
using Domain.Entities.Identity;

namespace Application.Features.Users.Specifications
{
    public class UserByIdSpec
        : Specification<User>, ISingleResultSpecification<User>
    {
        public UserByIdSpec(Guid id) 
        {
            Query
                .Where(u => u.Id == id);
        }
    }
}
