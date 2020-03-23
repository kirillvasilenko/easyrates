using System;
using System.Collections.Generic;

namespace AppRunner
{
    public class ConfigurationException:Exception
    {
        
        
        public ConfigurationException(IEnumerable<string> missedFields)
            :base($"Configuration reading error. Missed fields in configuration:\n{string.Join(", ", missedFields)}")
        {   
        }
    }
}