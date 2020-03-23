using System;

namespace AppRunner
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class RequiredSettingAttribute : Attribute
    {
        
    }
}