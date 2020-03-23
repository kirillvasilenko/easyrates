using System.Collections.Generic;
using AppRunner;
using Microsoft.Extensions.Configuration;

namespace EasyRates.WriterApp.Console.Di
{
    public class AppConfigExpert : BaseAppConfigExpert
    {
        public override IAppConfig MakeAppConfig(IConfiguration plainConfig)
        {
            var typedConfig = new WriterAppConfig();
                
            plainConfig.Bind(typedConfig);

            typedConfig.SetDefaultIfNeed();

            return typedConfig;
        }

        protected override void FindMissedFields(object obj, ref List<string> missedFields, string prefix = "")
        {
            base.FindMissedFields(obj, ref missedFields, prefix);

            if (obj is WriterAppConfig cnfg)
            {
                var oeCnfg = cnfg.ProviderOpenExchangeConfig;
                if (oeCnfg.Enabled == false)
                {
                    return;
                }

                if (string.IsNullOrEmpty(oeCnfg.AppId))
                {
                    missedFields.Add($"{nameof(cnfg.ProviderOpenExchangeConfig)}.{nameof(oeCnfg.AppId)}");
                }
            }
        }
    }
}