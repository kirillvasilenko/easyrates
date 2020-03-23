using Autofac.Core;
#pragma warning disable 1591
namespace EasyRates.ReaderApp.AspNet.Di
{
    public class DiModuleProvider
    {
        public IModule GetModule(AppConfig config)
        {
            return new FullModule(config);
        }
    }
}