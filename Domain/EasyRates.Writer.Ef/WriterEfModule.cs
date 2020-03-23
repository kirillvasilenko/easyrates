using Autofac;

namespace EasyRates.Writer.Ef
{
    public class WriterEfModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RatesWriterEf>()
                .As<IRatesWriter>();
        }
    }
}