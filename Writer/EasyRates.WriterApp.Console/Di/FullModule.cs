using AppVersion;
using Autofac;
using EasyRates.Model;
using EasyRates.Model.Ef;
using EasyRates.RatesProvider.InMemory;
using EasyRates.RatesProvider.OpenExchange;
using EasyRates.Writer.Ef;

namespace EasyRates.WriterApp.Console.Di
{
    public class FullModule : Module
    {
        private readonly WriterAppConfig cnfg;

        public FullModule(WriterAppConfig cnfg)
        {
            this.cnfg = cnfg;
        }

        protected override void Load(ContainerBuilder builder)
        {
            RegisterUtils(builder);
            
            RegisterDomain(builder);
            
            RegisterApp(builder);
            
            RegisterUi(builder);
        }

        private void RegisterUi(ContainerBuilder builder)
        {
            

        }

        private void RegisterUtils(ContainerBuilder builder)
        {
            builder.RegisterModule<AppVersionModule>();
            
        }

        private void RegisterDomain(ContainerBuilder builder)
        {
            builder.RegisterModule(new WriterEfModule());
            
            builder.RegisterModule(new RatesModelEfModule(
                cnfg.ConnectionString, 
                // ReSharper disable once PossibleInvalidOperationException
                cnfg.DbType.Value));

            builder.RegisterModule(new RatesProviderInMemoryModule(
                cnfg.ProviderInMemoryConfig.Priority,
                cnfg.ProviderInMemoryConfig.Name,
                cnfg.ProviderInMemoryConfig.Enabled));

            builder.RegisterModule(new RatesProviderOpenExchangeModule(
                cnfg.ProviderOpenExchangeConfig.Priority,
                cnfg.ProviderOpenExchangeConfig.Name,
                cnfg.ProviderOpenExchangeConfig.AppId,
                cnfg.ProviderOpenExchangeConfig.Currencies,
                cnfg.ProviderOpenExchangeConfig.ForPoorMode,
                cnfg.ProviderOpenExchangeConfig.Enabled));
            
            builder.RegisterModule<ModelModule>();
            
        }

        private void RegisterApp(ContainerBuilder builder)
        {
            builder.RegisterModule(new WriterRatesAppModule(
                cnfg.TimetableSettings.AnchorTime,
                cnfg.TimetableSettings.UpdatePeriod));
        }
    }
}