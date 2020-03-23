using Autofac;

namespace EasyRates.ErrorMessages.InMemory
{
    public class ErrorMessagesInMemoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(ErrorMessagesInMemory).Assembly).AsImplementedInterfaces();
        }
    }
}