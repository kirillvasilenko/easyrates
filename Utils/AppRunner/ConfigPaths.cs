using System.Collections.Generic;

namespace AppRunner
{
    public class ConfigPaths
    {
        public string ConfigFolder { get; set; }

        public IList<string> ConfigFiles { get; set; }

        public string NLogConfig { get; set; }
    }
}