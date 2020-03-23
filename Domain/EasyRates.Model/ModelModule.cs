using Autofac;

namespace EasyRates.Model
{
    public class ModelModule : Module
    {
        
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(ICurrencyNameFormatter).Assembly).AsImplementedInterfaces();
        }
    }
}