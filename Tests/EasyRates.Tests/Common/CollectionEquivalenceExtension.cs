using System.Collections.Generic;
using FluentAssertions;

namespace EasyRates.Tests.Common
{
    public static class CollectionEquivalenceExtension
    {
        public static bool IsEquivalent<T>(this ICollection<T> collection, ICollection<T> another,
            bool withStrictOrdering = true)
        {
            if (withStrictOrdering)
            {
                collection.Should().BeEquivalentTo(another, c => c.WithStrictOrdering());
            }
            else
            {
                collection.Should().BeEquivalentTo(another);
            }
            
            return true;
        }
    }
}