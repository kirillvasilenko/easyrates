using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SafeOperations;

namespace AppRunner
{
    public class ApplicationRunner
    {
        private readonly string environment;
        private readonly IAppConfigExpert appConfigExpert;

        private readonly CommandLineExpert commandLineExpert;
        
        public ApplicationRunner(string environment, IAppConfigExpert appConfigExpert)
        {
            this.environment = environment;
            this.appConfigExpert = appConfigExpert;
            commandLineExpert = new CommandLineExpert();
        }

		
        public int Run(string[] commandLineArgs, Action<RunningApplicationArgs> runApplication)
        {
            EnableUnicodeOnConsole();
            
            // 1. Command line args
            var clArgs = commandLineExpert.ReadArgs(commandLineArgs);
            var clArgsValidation = commandLineExpert.ValidateSafe(clArgs);
            
            // 2. Configuration
            var configPathsExpert = MakeConfigPathsExpert(clArgs.ConfigFolder);

            // 2.1 Paths to configs
            var cnfgPaths = configPathsExpert.MakeConfigPaths();
            var cnfgPathsValidation = configPathsExpert.ValidateSafe(cnfgPaths);
            
            // 2.2 Make typed configuration object
            var plainCnfg = new ConfigurationBuilder()
                .SetStandardConfiguration(cnfgPaths)
                .Build()
                .SetBinFolder();

            var cnfg = appConfigExpert.MakeAppConfig(plainCnfg);
            var cnfgValidation = appConfigExpert.ValidateSafe(cnfg);

            // 3. Configure NLog
            NLogConfigurator.Configure(cnfgPaths.NLogConfig, cnfg.LogsFolder);
            var nlogValidation = NLogConfigurator.ValidateSafe(cnfgPaths.NLogConfig);
            
            // 4. Total validation result
            var totalValidation = SafeOperation.FromMany(clArgsValidation, cnfgPathsValidation, cnfgValidation, nlogValidation);
            
            var logger = MakeLogger();
            
            logger.LogDebug($"{cnfg.ApplicationName} starting...\n" +
                            $"Command line args: {string.Join(", ", commandLineArgs)}\n" +
                            $"Config path: {cnfgPaths.ConfigFolder}\n" +
                            $"Log path:    {cnfg.LogsFolder}\n" +
                            $"Data path:   {cnfg.DataFolder}");

            // 5. Log all the errors, that were on reading configuration
            // and command line arguments
            logger.LogSafeOperationFaults(totalValidation);
            
            // 6. If there were errors, will stop application
            if (totalValidation.HasErrors)
            {
                logger.LogInformation("Application is unable to start.");
                return 1;
            }
            
            // 7. Run application
            try
            {
                runApplication(new RunningApplicationArgs
                {
                    ConfigPaths = cnfgPaths,
                    AppConfig = cnfg
                });
                return 0;
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Application stopped because of exception.");
                return 1;
            }
        }
        
        private ILogger MakeLogger()
        {
            return new NLogLoggerProvider().CreateLogger(GetType().FullName);
        }
        
        private ConfigPathsExpert MakeConfigPathsExpert(string configFolder)
        {
            return new ConfigPathsExpert(environment, configFolder);
        }
		
        private static void EnableUnicodeOnConsole()
        {
            // Configure console for support Russian language
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
        }
    }
}