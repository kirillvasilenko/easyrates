using System.Collections.Generic;
using System.Linq;

namespace EfCore.Common
{
    public static class StringsToStringConverter
    {
        public const string Separator = ",";
        
        public static string ToString(ICollection<string> collection)
        {
            return string.Join(Separator, collection.Select(g => g.ToString()));
        }
        
        public static ICollection<string> ToCollection(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new List<string>();
            }
            return value.Split(Separator).ToList();
        }
    }
}