using Microsoft.EntityFrameworkCore;

namespace EasyRates.Model.Ef.Pg
{
    public class RatesContext : DbContext
    {
        public DbSet<CurrencyRate> ActualRates { get; set; }
        
        public DbSet<CurrencyRateHistoryItem> RatesHistory { get; set; }
        
        public RatesContext(DbContextOptions<RatesContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.RemovePluralizingTableNameConvention();

            builder.Entity<CurrencyRate>()
                .Ignore(r => r.Key)
                .HasKey(o => new {o.From, o.To});
        }

    }
}