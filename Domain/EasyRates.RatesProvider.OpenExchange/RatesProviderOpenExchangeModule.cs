using Autofac;
using EasyRates.Writer;

namespace EasyRates.RatesProvider.OpenExchange
{
    public class RatesProviderOpenExchangeModule : Module
    {
        private readonly int priority;
        private readonly string name;
        private readonly string appId;
        private readonly string[] currencies;
        private readonly bool forPoorMode;
        private readonly bool enabled;

        public RatesProviderOpenExchangeModule(
            int priority, 
            string name,
            string appId,
            string[] currencies,
            bool forPoorMode,
            bool enabled)
        {
            this.priority = priority;
            this.name = name;
            this.appId = appId;
            this.currencies = currencies;
            this.forPoorMode = forPoorMode;
            this.enabled = enabled;
        }
        
        protected override void Load(ContainerBuilder builder)
        {
            if (!enabled)
            {
                return;
            }

            builder.Register(c => new OpenExchangeRateProviderSettings
                {
                    Name = name,
                    Priority = priority,
                    Currencies = currencies
                })
                .As<IOpenExchangeRateProviderSettings>();

            builder.RegisterType<OpenExchangeRatesProviderForPoor>()
                .As<IRatesProvider>();

            builder.Register(c => new OpenExchangeProxy(appId))
                .As<IOpenExchangeProxy>();
            
        }
    }
}