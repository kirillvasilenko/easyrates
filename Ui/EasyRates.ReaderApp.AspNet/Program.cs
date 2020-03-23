using System;
using System.IO;
using AppRunner;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace EasyRates.ReaderApp.AspNet
{
    /// <summary>
    /// Start program class
    /// </summary>
    public class Program
    {
        private const string AspnetcoreEnvironmentKey = "ASPNETCORE_ENVIRONMENT";

        private const string DefaultEnvironment = "Production";
	    
        /// <summary>
        /// Programs starts here
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var applicationRunner = new ApplicationRunner(
                GetEnvironment(),
                new Di.AppConfigExpert());
			
            applicationRunner.Run(args,
                runningArgs =>
                {
                    var logger = LogManager.GetCurrentClassLogger();
                    logger.Debug("Building web host...");
                    var host = BuildWebHost(runningArgs);
                    logger.Debug("Starting web host...");
                    host.Run();
                    logger.Debug("Application stopped.");    
                });
        }

        private static IWebHost BuildWebHost(RunningApplicationArgs args)
        {
            var appConfig = (Di.AppConfig) args.AppConfig;
            return new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    // Just in case.
                    // May be someone will need the standard configuration object.
                    config.SetStandardConfiguration(args.ConfigPaths);
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    // Disable filtration by microsoft.
                    // NLog will manage all about logs.
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseIISIntegration()
                .UseDefaultServiceProvider((context, options) =>
                {
                    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
                })
                .ConfigureServices(serviceCollection =>
                {
                    serviceCollection.AddSingleton(appConfig);
                    serviceCollection.AddSingleton(new Di.DiModuleProvider());
                })
                .UseStartup<Startup>()
                .UseNLog()
                .UseUrls(appConfig.Urls)
                .Build();
        }

        private static string GetEnvironment()
        {
            var env = Environment.GetEnvironmentVariable(AspnetcoreEnvironmentKey);
            return string.IsNullOrEmpty(env)
                ? DefaultEnvironment
                : env;
        }
    }
}