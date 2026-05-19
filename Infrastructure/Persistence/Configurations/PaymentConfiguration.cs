using Domain.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Amount)
                .HasPrecision(18, 2);

            builder.Property(x => x.Gateway)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Authority)
                .HasMaxLength(100);

            builder.HasIndex(x => x.Authority)
                .IsUnique();

            builder.Property(x => x.RefId)
                .HasMaxLength(100);

            builder.Property(x => x.Status)
                .HasConversion<int>();

            builder.Ignore(x => x.Events);
        }
    }
}
