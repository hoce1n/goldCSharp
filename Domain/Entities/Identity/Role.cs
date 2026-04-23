using Domain.Common;

namespace Domain.Entities.Identity
{
    public class Role : BaseEntity
    {
        private readonly List<UserRole> _userRoles = new();
        public IReadOnlyCollection<UserRole> UserRoles => _userRoles;
        public string Name { get; private set; }
        private Role() {}
        public Role(string name)
        {
            Name = name;
        }
    }
}