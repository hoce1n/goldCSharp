using Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class MarketPriceSnapshotConfiguration : IEntityTypeConfiguration<MarketPriceSnapshot>
    {
        public void Configure(EntityTypeBuilder<MarketPriceSnapshot> builder)
        {
            builder.ToTable("MarketPriceSnapshots");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Price18PerGram)
                .IsRequired();

            builder.Property(s => s.CreatedAt)
                .IsRequired();

            builder.HasIndex(s => s.CreatedAt)
                .IsDescending();

            builder.Ignore(x => x.Events);
        }
    }
}
