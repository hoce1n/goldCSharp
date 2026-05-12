using Domain.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ProductType).HasConversion<int>();
            builder.Property(x => x.QuoteSide).HasConversion<int>();
            builder.Property(x => x.Status).HasConversion<int>();

            builder.Property(x => x.RequestAmount).HasPrecision(18, 6);
            builder.Property(x => x.UnitPrice).HasPrecision(18, 2);
            builder.Property(x => x.TotalPrice).HasPrecision(18, 2);

            builder.Property(x => x.QuoteId).IsRequired();
            builder.Property(x => x.UserId).IsRequired();

            builder.Ignore(x => x.Events);
        }
    }
}
