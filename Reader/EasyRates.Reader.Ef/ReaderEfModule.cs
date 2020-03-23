using Autofac;
using EasyRates.Reader.Model;

namespace EasyRates.Reader.Ef
{
    public class ReaderEfModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RatesReaderRepoEf>()
                .As<IRatesReaderRepo>();
            
        }
    }
}