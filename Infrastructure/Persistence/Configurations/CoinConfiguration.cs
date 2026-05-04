using Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class CoinConfiguration : IEntityTypeConfiguration<Coin>
    {
        public void Configure(EntityTypeBuilder<Coin> builder)
        {
            builder.ToTable("Coins");

            builder.HasKey(x => x.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.WeightInSoot)
                .IsRequired();

            builder.Property(c => c.Karat)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(c => c.MintingFee)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(c => c.Stock)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(c => c.ImageUrl)
                .HasMaxLength(500);

            builder.Property(c => c.Description)
                .HasMaxLength(2000);

            builder.Property(c => c.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasIndex(c => c.Name).IsUnique();
            builder.HasIndex(c => new { c.IsActive, c.CreatedAt });

            builder.Property(c => c.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.UpdatedAt)
                .IsRequired(false)
                .ValueGeneratedOnUpdate(); ;


            // Ignore computed/logic-only properties
            builder.Ignore(c => c.WeightInGrams);
            builder.Ignore(c => c.Purity);
            builder.Ignore(c => c.PureGoldWeight);

            builder.Ignore(c => c.Events);

        }
    }
}
