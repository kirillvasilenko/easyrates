using AppRunner;
using Microsoft.Extensions.Configuration;

namespace EasyRates.Migrator.Di
{
    public class AppConfigExpert : BaseAppConfigExpert
    {
        public override IAppConfig MakeAppConfig(IConfiguration plainConfig)
        {
            var typedConfig = new MigratorConfig();
                
            plainConfig.Bind(typedConfig);

            typedConfig.SetDefaultIfNeed();

            return typedConfig;
        }
    }
}