using Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class MeltedGoldConfigurations : IEntityTypeConfiguration<MeltedGold>
    {
        public void Configure(EntityTypeBuilder<MeltedGold> builder)
        {
            builder.ToTable("MeltedGolds");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.BuyFeePerGram)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(x => x.SellFeePerGram)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(x => x.MinTradeAmount)
                .IsRequired()
                .HasPrecision(18, 3);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt);

            builder.Ignore(x => x.Events);
        }
    }
}
