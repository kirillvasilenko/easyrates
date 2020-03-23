using Microsoft.Extensions.Configuration;
using SafeOperations;

namespace AppRunner
{
    public class CommandLineExpert
    {
        public const string ConfigFolder = "ConfigFolder";
        
        public CommandLineArgs ReadArgs(string[] commandLineArgs)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(commandLineArgs)
                .Build();

            return new CommandLineArgs
            {
                ConfigFolder = config[ConfigFolder]
            };
        }

        public SafeOperationResult ValidateSafe(CommandLineArgs args)
        {
            return SafeOperation.Success();
        }
    }
}