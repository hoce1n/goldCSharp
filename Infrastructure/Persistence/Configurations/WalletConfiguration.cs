using Domain.Entities.Wallet;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable("Wallets");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.Balance)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.HasIndex(x => x.UserId)
                .IsUnique();

            builder.HasMany(x => x.Ledgers)
               .WithOne()
               .HasForeignKey(x => x.WalletId);

            builder.HasMany(x => x.Ledgers)
               .WithOne(x => x.Wallet)
               .HasForeignKey(x => x.WalletId)
               .OnDelete(DeleteBehavior.Restrict);

            //builder.Property(x => x.RowVersion)
            //    .IsRowVersion()
            //    .IsConcurrencyToken();
                

            builder.Ignore(x => x.Events);
        }
    }
}
