using System.Collections.Generic;
using System.IO;
using System.Linq;
using SafeOperations;

namespace AppRunner
{
    public class ConfigPathsExpert
    {
        private const string NLogConfigFileName = "NLog.config";

        private const string ConfigFileName = "appsettings.json";

        private const string ConfigFileNameEnv = "appsettings.{0}.json";


        private readonly string environment;

        private readonly string configFolder;
        
        
        public ConfigPathsExpert(string environment, string configFolder)
        {
            this.configFolder = string.IsNullOrEmpty(configFolder)
                ? Directory.GetCurrentDirectory()
                : configFolder;
            this.environment = environment;
        }

        public ConfigPaths MakeConfigPaths()
        {
            return new ConfigPaths
            {
                ConfigFolder = configFolder,
                ConfigFiles = new List<string>()
                {
                    Path.Combine(configFolder, ConfigFileName),
                    Path.Combine(configFolder, string.Format(ConfigFileNameEnv, environment))
                },
                NLogConfig = Path.Combine(configFolder, NLogConfigFileName)
            };
        }

        public SafeOperationResult ValidateSafe(ConfigPaths configPaths)
        {
            var result = SafeOperation.Success();
			
            if (configPaths.ConfigFiles.All(config => !File.Exists(config)))
            {
                result.AddError(
                    new FileNotFoundException(
                        "Application config files not found:\n" +
                        string.Join("\n", configPaths.ConfigFiles)));
            }
            if (!File.Exists(configPaths.NLogConfig))
            {
                result.AddWarning(new FileNotFoundException(
                    "NLog config file not found:\n" +
                    $"{configPaths.NLogConfig}"));
            }

            return result;
        }
        
        /*private AppConfigMaker ConfigMaker { get; } = new AppConfigMaker();
        
        private AppConfigValidator ConfigValidator { get; } = new AppConfigValidator();*/
        
        /*public AppConfig MakeAppConfig(IConfiguration configuration)
        {
            configuration[nameof(AppConfig.ConfigFolder)] = ConfigFolder;
            
            var location = System.Reflection.Assembly.GetEntryAssembly().Location;
            var binaryFolder = Path.GetDirectoryName(location);
            configuration[nameof(AppConfig.BinFolder)] = binaryFolder;
            
            return ConfigMaker.MakeAppConfig(configuration);
        }

        public SafeOperationResult ValidateSafe(AppConfig appConfig)
        {
            return ConfigValidator.ValidateSafe(appConfig);
        }*/
    }
}