using Autofac;
using EasyRates.Writer;

namespace EasyRates.RatesProvider.InMemory
{
    public class RatesProviderInMemoryModule : Module
    {
        private readonly int priority;
        private readonly string name;
        private readonly bool enabled;

        public RatesProviderInMemoryModule(int priority, string name, bool enabled)
        {
            this.priority = priority;
            this.name = name;
            this.enabled = enabled;
        }
        
        protected override void Load(ContainerBuilder builder)
        {
            if (!enabled)
            {
                return;
            }

            builder.Register(c => new RatesProviderInMemory(priority, name))
                .As<IRatesProvider>();
        }
    }
}