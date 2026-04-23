namespace Domain.Entities.Identity
{
    public class UserRole
    {
        public Role Role { get; private set; }
        public User User { get; private set; }
        public Guid UserId { get; private set; }
        public Guid RoleId { get; private set; }

        private UserRole() {}

        public UserRole(Guid userId, Guid roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}