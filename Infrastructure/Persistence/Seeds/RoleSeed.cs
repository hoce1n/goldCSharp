using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seeds
{
    public static class RoleSeed
    {
        public static void SeedRoles(ModelBuilder modelBuilder)
        {
            var now = DateTime.UtcNow;

            modelBuilder.Entity<Role>().HasData(
                CreateRole("Customer", Guid.Parse("11111111-1111-1111-1111-111111111111"), now),
                CreateRole("Admin", Guid.Parse("22222222-2222-2222-2222-222222222222"), now),
                CreateRole("Manager", Guid.Parse("33333333-3333-3333-3333-333333333333"), now)
            );
        }
        private static object CreateRole(string name, Guid id, DateTime createdAt)
        {
            return new 
            { 
                Id = id, 
                Name = name,
                CreatedAt = createdAt,
                UpdatedAt = (DateTime?)null
            };
        }
    }
}
