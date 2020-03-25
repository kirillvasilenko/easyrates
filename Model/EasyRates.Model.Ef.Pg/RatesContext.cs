using System;
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
            
            builder
                .Entity<CurrencyRate>(x =>
                {
                    x.Ignore(r => r.Key)
                        .HasKey(o => new {From = o.CurrencyFrom, To = o.CurrencyTo});
                    
                    x.Property(e => e.ExpirationTime)
                        .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
                    x.Property(e => e.OriginalPublishedTime)
                        .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
                    x.Property(e => e.TimeOfReceipt)
                        .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
                });
        }

    }
}