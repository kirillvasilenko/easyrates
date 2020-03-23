using EfCore.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EasyRates.Model.Ef
{
    public class RatesContext : BaseDbContext
    {
        public RatesContext(RatesDbParams dbParams, ILoggerFactory loggerFactory = null) 
            : base(dbParams.ConnectionString, dbParams.DbType, loggerFactory)
        {
        }
        
        public DbSet<CurrencyRate> ActualRates { get; set; }
        
        public DbSet<CurrencyRateHistoryItem> RatesHistory { get; set; }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CurrencyRate>()
                .Ignore(r => r.Key)
                .HasKey(o => new {o.From, o.To});
        }

    }
}