using Microsoft.Extensions.Logging;

namespace SafeOperations
{
    public static class LoggerExtensions
    {
        public static void LogSafeOperationFaults(this ILogger logger, SafeOperationResult result)
        {
            foreach (var warning in result.Warnings)
            {
                logger.LogWarning(warning, warning.Message);
            }

            foreach (var error in result.Errors)
            {
                logger.LogError(error, error.Message);
            }
        }
    }
}