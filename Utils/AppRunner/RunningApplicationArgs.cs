using Microsoft.Extensions.Configuration;

namespace AppRunner
{
    public class RunningApplicationArgs
    {
        public ConfigPaths ConfigPaths { get; set; }
        
        public IAppConfig AppConfig { get; set; }
    }
}