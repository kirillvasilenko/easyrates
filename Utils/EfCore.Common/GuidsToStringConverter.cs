using System;
using System.Collections.Generic;
using System.Linq;

namespace EfCore.Common
{
    public static class GuidsToStringConverter
    {
        public const string Separator = ",";
        
        public static string ToString(ICollection<Guid> guids)
        {
            return string.Join(Separator, guids.Select(g => g.ToString()));
        }
        
        public static ICollection<Guid> ToGuids(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new List<Guid>();
            }
            return value.Split(Separator)
                .Select(Guid.Parse).ToList();
        }
    }
}