using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using SafeOperations;

namespace AppRunner
{
    public abstract class BaseAppConfigExpert : IAppConfigExpert
    {
        public abstract IAppConfig MakeAppConfig(IConfiguration plainConfig);
        
        public SafeOperationResult ValidateSafe(IAppConfig appConfig)
        {
            var missedFields = new List<string>();

            FindMissedFields(appConfig, ref missedFields);

            // There is at least one missed property
            if (missedFields.Any())
            {
                return SafeOperation.Error(new ConfigurationException(missedFields));
            }
            
            return SafeOperation.Success();
        }

        protected virtual void FindMissedFields(object obj, ref List<string> missedFields, string prefix = "")
        {
            foreach (var propertyInfo in obj.GetType().GetProperties())
            {
                // Only for required property
                if (propertyInfo.GetCustomAttribute(typeof(RequiredSettingAttribute)) != null)
                {
                    var propValue = propertyInfo.GetValue(obj);
                    if (propValue == null)
                    {
                        missedFields.Add($"{prefix}{propertyInfo.Name}");    
                    }
                    else
                    {
                        // Find in
                        FindMissedFields(propValue, ref missedFields, $"{prefix}{propertyInfo.Name}.");
                    }
                }
            }
        }
    }
}