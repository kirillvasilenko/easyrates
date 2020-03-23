using System;
using Autofac;

namespace EasyRates.WriterApp
{
    public class WriterRatesAppModule : Module
    {
        private readonly TimeSpan anchorTime;
        private readonly TimeSpan updatePeriod;
        
        public WriterRatesAppModule(TimeSpan anchorTime, TimeSpan updatePeriod)
        {
            this.anchorTime = anchorTime;
            this.updatePeriod = updatePeriod;
        }
        
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new Timetable(anchorTime, updatePeriod))
                .As<ITimetable>();

            builder.RegisterType<RatesUpdater>()
                .As<IRatesUpdater>();

            builder.RegisterType<WriterApp>()
                .As<IWriterApp>();
        }
    }
}