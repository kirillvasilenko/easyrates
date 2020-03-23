using System;
using System.Collections.Generic;
using System.Linq;
using EasyRates.Writer;
using FluentAssertions;
using Moq;
using Xunit;

namespace EasyRates.Tests.Domain.Writer
{
    public class RatesProvidersTests
    {
        private Random r = new Random();
        
        private OrderedRatesProviders orderedProviders;

        private ICollection<IRatesProvider> providers;

        public RatesProvidersTests()
        {
            var mocks = new[]
            {
                new Mock<IRatesProvider>(),
                new Mock<IRatesProvider>(),
                new Mock<IRatesProvider>()
            };
            foreach (var provider in mocks)
            {
                provider.Setup(p => p.Priority).Returns(r.Next(1, 10));
            }

            providers = mocks.Select(p => p.Object).ToArray();
            
            orderedProviders = new OrderedRatesProviders(providers);
        }

        [Fact]
        public void GetProviders_ReturnsProvidersInRightOrder()
        {
            var result = orderedProviders.GetProviders();

            result.Should().BeEquivalentTo(providers);
            result.Should().BeInDescendingOrder(p => p.Priority);
        }
    }
}