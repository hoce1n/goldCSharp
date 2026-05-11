using Domain.Entities.Identity;
using Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Quote;

namespace Infrastructure.Persistence.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public AppDbContext() { }

        public DbSet<User> Users => Set<User>();
        public DbSet<OtpCode> OtpCodes => Set<OtpCode>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<Coin> Coins => Set<Coin>();
        public DbSet<MarketPriceSnapshot> MarketPriceSnapshots => Set<MarketPriceSnapshot>();
        public DbSet<Quote> Quotes => Set<Quote>();
        public DbSet<MeltedGold> MeltedGolds => Set<MeltedGold>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);

        }
    }
}