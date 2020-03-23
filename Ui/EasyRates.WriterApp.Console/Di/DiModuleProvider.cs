using Autofac.Core;

namespace EasyRates.WriterApp.Console.Di
{
    public class DiModuleProvider
    {
        public IModule GetModule(WriterAppConfig migratorConfig)
        {
            return new FullModule(migratorConfig);
        }
    }
}