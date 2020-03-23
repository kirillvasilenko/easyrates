using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

namespace EfCore.Common
{
    public abstract class BaseDbContext : DbContext
    {
        private readonly string connectionString;
        private readonly DbType dbType;
        private readonly ILoggerFactory loggerFactory;

        protected BaseDbContext(string connectionString, DbType dbType, ILoggerFactory loggerFactory)
        {
            this.connectionString = connectionString;
            this.dbType = dbType;
            this.loggerFactory = loggerFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            switch (dbType)
            {
                case DbType.SQLite:
                    optionsBuilder.UseSqlite(connectionString);
                    break;
                case DbType.SQLServer:
                    optionsBuilder.UseSqlServer(connectionString);
                    break;
                case DbType.Postgres:
                    optionsBuilder.UseNpgsql(connectionString);
                    break;
                case DbType.InMemory:
                    optionsBuilder.UseInMemoryDatabase(connectionString);
                    optionsBuilder.ConfigureWarnings(b =>
                    {
                        b.Ignore(InMemoryEventId.TransactionIgnoredWarning);
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            optionsBuilder.UseLoggerFactory(loggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.RemovePluralizingTableNameConvention();
            
            //base.OnModelCreating(builder);
        }

        public virtual void InitData()
        {
            
        }
    }
    
    public static class ModelBuilderExtensions 
    {
        public static void RemovePluralizingTableNameConvention(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.DisplayName();
            }
        }
    }
}