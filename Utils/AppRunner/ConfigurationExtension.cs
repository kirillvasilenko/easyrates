using System.IO;
using Microsoft.Extensions.Configuration;

namespace AppRunner
{
    public static class ConfigurationExtension
    {
        public static IConfiguration SetBinFolder(this IConfiguration configuration)
        {
            var location = System.Reflection.Assembly.GetEntryAssembly().Location;
            var binaryFolder = Path.GetDirectoryName(location);
            configuration[nameof(IAppConfig.BinFolder)] = binaryFolder;

            return configuration;
        }
    }
}