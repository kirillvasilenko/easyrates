using System;
using System.IO;
using EfCore.Common;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EasyRates.Model.Ef
{
    public class RatesContextFactory : IDesignTimeDbContextFactory<RatesContext>
    {
        public RatesContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config[nameof(RatesDbParams.ConnectionString)];
            var dbType = Enum.Parse<DbType>(config[nameof(RatesDbParams.DbType)]);
            
            return new RatesContext(new RatesDbParams {ConnectionString = connectionString, DbType = dbType}, null);
        }
    }
}