using AppRunner;
using Microsoft.Extensions.Configuration;
#pragma warning disable 1591

namespace EasyRates.ReaderApp.AspNet.Di
{
    public class AppConfigExpert : BaseAppConfigExpert
    {
        public override IAppConfig MakeAppConfig(IConfiguration plainConfig)
        {
            var typedConfig = new AppConfig();
                
            plainConfig.Bind(typedConfig);

            typedConfig.SetDefaultIfNeed();

            return typedConfig;
        }
    }
}