using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class RefreshTokenConfiguration
        : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshToken");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.TokenHash)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.ExpiresAt)
                .IsRequired();

            builder.Property(x => x.RevokedAt)
                .IsRequired();

            builder.Property(x => x.ReplacedByTokenId)
                .IsRequired(false);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .IsRequired(false);

            // Indexes

            builder.HasIndex(x => x.Id);

            builder.HasIndex(x => x.TokenHash)
                .IsUnique();

            // Relations:
            builder.HasOne(x => x.User)
                .WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Ignore(x => x.Events);
        }
    }
}
