using Domain.Entities.Identity;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class OtpCodeConfiguration 
        : IEntityTypeConfiguration<OtpCode>
    {
        public void Configure(EntityTypeBuilder<OtpCode> builder)
        {
            builder.ToTable("OtpCodes");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(6);

            builder.Property(x => x.PhoneNumber)
            .IsRequired()
            .HasMaxLength(11);

            builder.Property(x => x.ExpiresAt)
                .IsRequired();

            builder.Property(x => x.UsedAt)
                .IsRequired(false);

            builder.Property(x => x.AttemptCount)
                .IsRequired();

            builder.Property(x => x.PhoneNumber)
                .HasConversion<PhoneNumberConverter>()
                .HasMaxLength(11)
                .IsRequired();

            // Indexes
            builder.HasIndex("PhoneNumber");
            builder.HasIndex(x => x.Code);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .IsRequired(false);

            builder.Ignore(x => x.Events);
        }
    }
}
