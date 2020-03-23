using AppVersion;
using Autofac;
using EasyRates.ErrorMessages.InMemory;
using EasyRates.Model;
using EasyRates.Model.Ef;
using EasyRates.Reader.Ef;
using EasyRates.Reader.Model;
using EasyRates.ReaderApp;
using Microsoft.Extensions.Caching.Memory;
#pragma warning disable 1591
namespace EasyRates.ReaderApp.AspNet.Di
{
    public class FullModule : Module
    {
        private readonly AppConfig cnfg;

        public FullModule(AppConfig cnfg)
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
            builder.RegisterModule<ErrorMessagesInMemoryModule>();
        }

        private void RegisterUtils(ContainerBuilder builder)
        {
            builder.RegisterModule<AppVersionModule>();
            
            builder.RegisterType<MemoryCache>()
                .As<IMemoryCache>()
                .SingleInstance();
        }

        private void RegisterDomain(ContainerBuilder builder)
        {
            // ReSharper disable once PossibleInvalidOperationException
            builder.RegisterModule(new ReaderModule());
            
            builder.RegisterModule(new RatesModelEfModule(
                cnfg.ConnectionString, 
                // ReSharper disable once PossibleInvalidOperationException
                cnfg.DbType.Value));

            builder.RegisterModule<ReaderEfModule>();
            
            builder.RegisterModule<ModelModule>();
            
        }

        private void RegisterApp(ContainerBuilder builder)
        {
            builder.RegisterModule<ReaderRatesAppModule>();
        }
    }
}