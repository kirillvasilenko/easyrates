using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EasyRates.Model.Ef.Pg
{
    public class RatesContextFactory : IDesignTimeDbContextFactory<RatesContext>
    {
        public RatesContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");
            
            return CreateDbContext(connectionString);
        }
        
        public RatesContext CreateDbContext(string connectionString, ILoggerFactory loggerFactory = null)
        {
            var migrationsAssembly = GetType().Assembly.GetName().Name;
            
            var builder = new DbContextOptionsBuilder<RatesContext>();
            builder
                .UseNpgsql(connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly))
                .UseSnakeCaseNamingConvention();
            
            if (loggerFactory != null)
            {
                builder.UseLoggerFactory(loggerFactory);
            }
            
            return new RatesContext(builder.Options);
        }
    }
}