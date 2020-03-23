using Microsoft.Extensions.Configuration;
using SafeOperations;

namespace AppRunner
{
    public interface IAppConfigExpert
    {
        IAppConfig MakeAppConfig(IConfiguration plainConfig);

        SafeOperationResult ValidateSafe(IAppConfig appConfig);
    }
}