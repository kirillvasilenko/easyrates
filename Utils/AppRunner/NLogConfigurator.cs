using System.IO;
using System.Linq;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using SafeOperations;

namespace AppRunner
{
    public static class NLogConfigurator
    {
        private const string LogFileName = "${date:format=yyyy-MM-dd}.log";

        private const string InternalLogFileName = "nlog-internal.log";

        private const string DefaultLayout = "${level:upperCase=true}|${logger}|${exception}\n${message}\n--------------------------------";

        private const string LogsFolderKey = "logsFolder";
		
        public static void Configure(string configFile, string logsFolder)
        {
            var config = File.Exists(configFile)
                ? new XmlLoggingConfiguration(configFile, true)
                : new LoggingConfiguration();

            // Need that at least one target is.
            // Otherwise all messages will be lost.
            // It may be if there is critical error
            // in NLog configuration file.
            if (!config.AllTargets.Any())
            {
                config.SetDefault(logsFolder);
            }
            config.Variables.Add(LogsFolderKey, logsFolder);
			
            NLog.Common.InternalLogger.LogFile = Path.Combine(logsFolder, InternalLogFileName);

            LogManager.Configuration = config;
        }

        public static SafeOperationResult ValidateSafe(string configFile)
        {
            return SafeOperation.Run(() =>
            {
                var config = new XmlLoggingConfiguration(configFile, false);
                if (!config.AllTargets.Any())
                {
                    throw new NLogConfigurationException($"There are no any targets in NLog config file:\n" +
                                                         $"{configFile}\n" +
                                                         $"It may be in case if there are some errors in the file.");
                }
            }, SafeOperationMode.WarningIfException);
        }

        private static void SetDefault(this LoggingConfiguration config, string logsFolder)
        {
            var fileTarget = new FileTarget
            {
                Name = "defaultFile",
                FileName = Path.Combine(logsFolder, LogFileName),
                Layout = new SimpleLayout(DefaultLayout)
            };
            var consoleTarget = new ColoredConsoleTarget
            {
                Name = "defaultConsole",
                Layout = new SimpleLayout(DefaultLayout)
            };
            config.AddTarget(fileTarget);
            config.AddTarget(consoleTarget);
			
            config.AddRuleForAllLevels(fileTarget);
            config.AddRuleForAllLevels(consoleTarget);
        }
    }
}