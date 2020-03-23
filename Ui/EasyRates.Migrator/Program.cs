using System;
using AppRunner;
using Autofac;
using EasyRates.Migrator.Di;

namespace EasyRates.Migrator
{
    class Program
    {
        private const string AspnetcoreEnvironmentKey = "ASPNETCORE_ENVIRONMENT";
        
        static int Main(string[] args)
        {
            var applicationRunner = new ApplicationRunner(
                Environment.GetEnvironmentVariable(AspnetcoreEnvironmentKey)
                , new AppConfigExpert());
			
            return applicationRunner.Run(args, RunMigrate);
        }

        static void RunMigrate(RunningApplicationArgs args)
        {
            var config = (MigratorConfig) args.AppConfig;
            
            var builder = new ContainerBuilder();

            builder.RegisterModule(new FullModule(config));
            
            var appContainer = builder.Build();
                    
            var migrator = new Migrator(config, appContainer);
                    
            migrator.Migrate(); 
        }
    }
}