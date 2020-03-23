using Autofac;

namespace AppVersion
{
    public class AppVersionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IAppVersion).Assembly).AsImplementedInterfaces();
        }
    }
}