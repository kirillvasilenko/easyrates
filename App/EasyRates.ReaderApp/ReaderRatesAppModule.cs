using Autofac;
#pragma warning disable 1591
namespace EasyRates.ReaderApp
{
    public class ReaderRatesAppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IReaderRateService).Assembly).AsImplementedInterfaces();
        }
    }
}