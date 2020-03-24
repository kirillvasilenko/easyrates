using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using EasyRates.RatesProvider.OpenExchange;
using FluentAssertions;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace EasyRates.Writer.Tests.WriterOpenExchange
{
    public class OpenExchangeRatesProviderTests
    {
        private readonly Fixture fixture = new Fixture();

        private readonly Mock<ISystemClock> clock = new Mock<ISystemClock>();
        
        private readonly OpenExchangeProviderOptions opts;

        private readonly ILogger<OpenExchangeRatesProvider> logger = new NullLogger<OpenExchangeRatesProvider>();
        
        private readonly DateTime currentTime = DateTime.UtcNow;
        
        public OpenExchangeRatesProviderTests()
        {
            opts = new OpenExchangeProviderOptions
            {
                Name = fixture.Create<string>(),
                Priority = fixture.Create<int>(),
                Currencies = fixture.CreateMany<string>().ToArray()
            };
            
            clock.Setup(x => x.UtcNow).Returns(currentTime);
        }

        private OpenExchangeProxyTest.InvokeCase MakeCase(TimeSpan emulationTime, bool throwException = false)
        {
            return new OpenExchangeProxyTest.InvokeCase()
            {
                From = fixture.Create<string>(),
                Response = fixture.Create<ActualRateResponse>(),
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
            opts.Currencies = currencies;
            
            var expectedResult = new[] {case1.Response, case2.Response, case3.Response}
                .Select(r => r.ToDomain(opts.Name, currentTime))
                .SelectMany(r => r)
                .ToArray();
            
            var proxy = new OpenExchangeProxyTest(new []{case1, case2, case3});
            
            var provider = new OpenExchangeRatesProvider(proxy, clock.Object, Options.Create(opts), logger);

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
            opts.Currencies = currencies;
            
            var expectedResult = new[] {case1.Response, case3.Response}
                .Select(r => r.ToDomain(opts.Name, currentTime))
                .SelectMany(r => r)
                .ToArray();
            
            var proxy = new OpenExchangeProxyTest(new []{case1, case2, case3});
            
            var provider = new OpenExchangeRatesProvider(proxy, clock.Object, Options.Create(opts), logger);

            var result = await provider.GetAllRates();
            
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}