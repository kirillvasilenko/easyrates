using Autofac;
using EasyRates.Migrator.Di;
using EasyRates.Model.Ef;
using EfCore.Common;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace EasyRates.Migrator
{
    public class Migrator
    {
        private readonly MigratorConfig config;
        private readonly IContainer container;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public Migrator(MigratorConfig config, IContainer container)
        {
            this.config = config;
            this.container = container;
        }

        public void Migrate()
        {
            // Auth
            
            logger.Info("EasyRates database migration...");

            if (config.DbType != DbType.InMemory)
            {
                var authContext = container.Resolve<RatesContext>();
                authContext.Database.Migrate();
            }
            
            logger.Info("EasyRates database migration completed.");
        }
    }
}