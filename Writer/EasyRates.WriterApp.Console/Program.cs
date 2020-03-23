using System;
using AppRunner;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EasyRates.WriterApp.Console.Di;
using EasyRates.WriterApp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;


namespace EasyRates.WriterApp.Console
{
    public class Program
    {
        private const string AspnetcoreEnvironmentKey = "ASPNETCORE_ENVIRONMENT";
        
        public static int Main(string[] args)
        {
            var applicationRunner = new ApplicationRunner(
                Environment.GetEnvironmentVariable(AspnetcoreEnvironmentKey)
                , new AppConfigExpert());
			
            return applicationRunner.Run(args, RunWriter);
            
        }
        
        static void RunWriter(RunningApplicationArgs args)
        {
            var appContainer = MakeAppContainer((WriterAppConfig) args.AppConfig);
            var logger = LogManager.GetCurrentClassLogger();

            using (var writerApp = appContainer.Resolve<IWriterApp>())
            {
                writerApp.Start();
                logger.Info("Application started. Press Enter to stop.");
                System.Console.ReadLine();
                logger.Info("Application is stopping...");
            }

            logger.Info("Application stopped.");
        }

        static IContainer MakeAppContainer(WriterAppConfig config)
        {
            var tmp = new ServiceCollection()
                .AddLogging(loggingBuilder =>
                {
                    // configure Logging with NLog
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    loggingBuilder.AddNLog();
                });
            
            var builder = new ContainerBuilder();
            builder.Populate(tmp);

            builder.RegisterModule(new FullModule(config));
            
            return builder.Build();
        }
    }
}