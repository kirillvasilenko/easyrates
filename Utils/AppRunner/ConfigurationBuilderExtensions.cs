using Microsoft.Extensions.Configuration;

namespace AppRunner
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder SetStandardConfiguration(
            this IConfigurationBuilder builder, 
            ConfigPaths configPaths)
        {
            builder.SetBasePath(configPaths.ConfigFolder);

            foreach (var configFile in configPaths.ConfigFiles)
            {
                builder.AddJsonFile(configFile, true, true);
            }
			
            return builder
                .AddEnvironmentVariables();
        }
        
    }
}