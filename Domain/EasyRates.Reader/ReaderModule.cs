using System;
using Autofac;

namespace EasyRates.Reader.Model
{
    public class ReaderModule : Module
    {
        
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RatesReaderWithCache>()
                .As<IRatesReader>();
        }
    }
}