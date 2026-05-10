//using Domain.Entities.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Infrastructure.Persistence.Configurations
//{
//    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
//    {
//        public void Configure(EntityTypeBuilder<UserRole> builder)
//        {
//            builder.ToTable("UserRole");
            
//            builder.HasKey(x => new { x.UserId, x.RoleId });

//            builder.HasOne(x => x.User)
//                .WithMany(x => x.UserRoles)
//                .HasForeignKey(x => x.UserId)
//                .OnDelete(DeleteBehavior.Restrict);

//            builder.HasOne(x => x.Role)
//                .WithMany(x => x.UserRoles)
//                .HasForeignKey(x => x.RoleId)
//                .OnDelete(DeleteBehavior.Restrict);
//        }
//    }
//}
