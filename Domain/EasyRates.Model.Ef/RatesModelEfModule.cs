using Autofac;
using EfCore.Common;

namespace EasyRates.Model.Ef
{
    public class RatesModelEfModule : Module
    {
        private readonly string connectionString;
        private readonly DbType dbType;

        public RatesModelEfModule(string connectionString, DbType dbType)
        {
            this.connectionString = connectionString;
            this.dbType = dbType;
        }
        
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new RatesDbParams
            {
                ConnectionString = connectionString,
                DbType = dbType
            });
            builder
                .RegisterType<RatesContext>()
                .InstancePerLifetimeScope();
        }
    }
}