using Domain.Entities.Catalog;
using Domain.Entities.Quote;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class QuoteConfiguration : IEntityTypeConfiguration<Quote>
    {
        public void Configure(EntityTypeBuilder<Quote> builder)
        {
            builder.ToTable("Quote");

            builder.HasKey(q => q.Id);

            builder.Property(q => q.UserId)
                .IsRequired();

            builder.Property(q => q.ProductId)
                .IsRequired();

            builder.Property(q => q.Side)
                .IsRequired();

            builder.Property(q => q.RequestAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(q => q.PriceSnapshotId)
                .IsRequired();

            builder.Property(q => q.Status)
                .IsRequired();

            builder.Property(q => q.CreatedAt)
                .IsRequired();

            builder.Property(q => q.ExpiredAtUtc)
                .IsRequired();

            builder.HasIndex(q => q.UserId);
            builder.HasIndex(q => q.Status);
            builder.HasIndex(q => q.ExpiredAtUtc);

            builder.HasOne<MarketPriceSnapshot>()
                .WithMany()
                .HasForeignKey(q => q.PriceSnapshotId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Ignore(q => q.Events);
        }
    }
}
