using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using EasyRates.RatesProvider.OpenExchange;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EasyRates.Tests.Domain.RatesProviderOpenExchange
{
    public class OpenExchangeRatesProviderForPoorTests
    {
        private Fixture fixture = new Fixture();

        private OpenExchangeRatesProviderForPoor provider;
        
        private Mock<IOpenExchangeProxy> proxy = new Mock<IOpenExchangeProxy>(MockBehavior.Strict);

        private OpenExchangeRateProviderSettings settings;
        
        private Mock<ILogger<OpenExchangeRatesProviderForPoor>> logger = new Mock<ILogger<OpenExchangeRatesProviderForPoor>>();
        
        public OpenExchangeRatesProviderForPoorTests()
        {
            settings = new OpenExchangeRateProviderSettings()
            {
                Name = fixture.Create<string>(),
                Priority = fixture.Create<int>(),
                Currencies = fixture.CreateMany<string>().ToArray()
            };
            
        }

        private OpenExchangeProxyTest.InvokeCase MakeCase(TimeSpan emulationTime, bool throwException = false)
        {
            return new OpenExchangeProxyTest.InvokeCase()
            {
                From = fixture.Create<string>(),
                Response = fixture.Create<LatestRateResponse>(),
                ThrowException = throwException,
                EmulationTime = emulationTime
            };
        }
        
        [Fact]
        public async Task GetAllRates_InvokeProxyForEveryCurrency_Simultaneously()
        {
            var case1 = MakeCase(TimeSpan.FromSeconds(1));
            var case2 = MakeCase(TimeSpan.FromSeconds(1));
            var case3 = MakeCase(TimeSpan.FromSeconds(1));

            var currencies = new[] {case1.From, case2.From, case3.From};
            settings.Currencies = currencies;
            
            var expectedResult = new[] {case1.Response, case2.Response, case3.Response}
                .Select(r => r.ToDomain(settings.Name))
                .SelectMany(r => r)
                .ToArray();
            
            var proxy = new OpenExchangeProxyTest(new []{case1, case2, case3});
            
            provider = new OpenExchangeRatesProviderForPoor(proxy, settings, logger.Object);

            var watch = new Stopwatch();
            watch.Start();
            var result = await provider.GetAllRates();
            watch.Stop();
            
            // it means that proxy was invoked simultaneously for every currency
            Assert.True(watch.Elapsed < TimeSpan.FromSeconds(3));
            
            result.Should().BeEquivalentTo(expectedResult);
        }
        
        [Fact]
        public async Task GetAllRates_IfSomeRequestsThrowException_TheyWillBeSuppressed()
        {
            var case1 = MakeCase(TimeSpan.FromSeconds(1));
            var case2 = MakeCase(TimeSpan.FromSeconds(1), true);
            var case3 = MakeCase(TimeSpan.FromSeconds(1));

            var currencies = new[] {case1.From, case2.From, case3.From};
            settings.Currencies = currencies;
            
            var expectedResult = new[] {case1.Response, case3.Response}
                .Select(r => r.ToDomain(settings.Name))
                .SelectMany(r => r)
                .ToArray();
            
            var proxy = new OpenExchangeProxyTest(new []{case1, case2, case3});
            
            provider = new OpenExchangeRatesProviderForPoor(proxy, settings, logger.Object);

            var result = await provider.GetAllRates();
            
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}