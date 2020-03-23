using System;
using System.Linq;
using System.Reflection;

namespace AppVersion
{
    public class AppVersion:IAppVersion
    {
        public AppVersion()
        {
            var assembly = Assembly.GetEntryAssembly(); 
            
            FileVersion = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;

            Version = FileVersion;
            
            InformationalVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

            OnlyMajorMinor = string.Join(".", Version.Split('.').Take(2));
        }

        public string FileVersion { get; }

        public string Version { get; }

        public string InformationalVersion { get; }

        public string OnlyMajorMinor { get; }
    }
}