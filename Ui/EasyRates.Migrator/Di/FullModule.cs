using Autofac;
using EasyRates.Model;
using EasyRates.Model.Ef;

namespace EasyRates.Migrator.Di
{
    public class FullModule : Module
    {
        private readonly MigratorConfig cnfg;

        public FullModule(MigratorConfig cnfg)
        {
            this.cnfg = cnfg;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new RatesModelEfModule(
                cnfg.ConnectionString, 
                // ReSharper disable once PossibleInvalidOperationException
                cnfg.DbType.Value));
        }

    }
}