using Domain.Entities.Identity;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            // Primary Key
            builder.HasKey(x => x.Id);


            // Value Objects via Converter
            builder.Property(x => x.PhoneNumber)
                .HasConversion<PhoneNumberConverter>()
                .HasMaxLength(11)
                .IsRequired();

            builder.Property(x => x.NationalCode)
                .HasConversion<NationalCodeConverter>()
                .HasMaxLength(10);

            builder.Property(x => x.Email)
                .HasConversion<EmailConverter>()
                .HasMaxLength(200);

            // Enums
            builder.Property(x => x.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.VerificationLevel)
                .HasConversion<int>()
                .IsRequired();

            // Indexes
            builder.HasIndex(x => x.PhoneNumber)
                .IsUnique();

            // Relations
            builder.HasMany(x => x.Roles)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.RefreshTokens)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Audit Fields
            builder.Property(x => x.CreatedAt)
                .IsRequired(true);
                
            builder.Property(x => x.UpdatedAt)
                .IsRequired(false);

            builder.Property(x => x.FirstName)
                .HasMaxLength(100);

            builder.Property(x => x.LastName)
                .HasMaxLength(100);


            builder.Ignore(x => x.FullName);
            builder.Ignore(x => x.IsProfileCompleted);
            builder.Ignore(x => x.Events);

        }
    }
}