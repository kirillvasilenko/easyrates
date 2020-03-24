using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using EasyRates.Model;
using EasyRates.WriterApp;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace EasyRates.Writer.Tests.WriterApp
{
    public class RatesUpdaterTests
    {
        private readonly Fixture fixture = new Fixture();
        
        private readonly Random r = new Random();

        private readonly RatesUpdater updater;
        
        private readonly Mock<IRatesWriter> writer = new Mock<IRatesWriter>();
        
        private readonly Mock<ICurrencyRateProcessor> processor = new Mock<ICurrencyRateProcessor>(); 

        private readonly ILogger<RatesUpdater> logger = new NullLogger<RatesUpdater>();
        
        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        private readonly Mock<IOrderedRatesProviders> orderedProviders = new Mock<IOrderedRatesProviders>();

        private readonly RatesProviderMock[] providers;
        
        public RatesUpdaterTests()
        {
            providers = new[]
            {
                new RatesProviderMock(fixture.Create<string>(), r.Next(0,10),
                    new RatesProviderMock.InvokeCase
                    {
                        EmulationTime = TimeSpan.FromSeconds(1),
                        ThrowException = fixture.Create<bool>(),
                        Response = fixture.CreateMany<CurrencyRate>().ToArray()
                    }),
                new RatesProviderMock(fixture.Create<string>(), r.Next(0,10),
                    new RatesProviderMock.InvokeCase
                    {
                        EmulationTime = TimeSpan.FromSeconds(1),
                        ThrowException = fixture.Create<bool>(),
                        Response = fixture.CreateMany<CurrencyRate>().ToArray()
                    }),
                new RatesProviderMock(fixture.Create<string>(), r.Next(0,10),
                    new RatesProviderMock.InvokeCase
                    {
                        EmulationTime = TimeSpan.FromSeconds(1),
                        ThrowException = fixture.Create<bool>(),
                        Response = fixture.CreateMany<CurrencyRate>().ToArray()
                    }),
            };
            orderedProviders.Setup(p => p.GetProviders()).Returns(providers);
            
            updater = new RatesUpdater(writer.Object, orderedProviders.Object, processor.Object, logger);
        }

        [Fact]
        public async Task UpdateRates_InvokesProvidersAsynchronously()
        {
            var watch = new Stopwatch();
            
            watch.Start();
            await updater.UpdateRates(cts.Token);
            watch.Stop();

            // it means that providers were invoked simultaneously
            watch.Elapsed.Should().BeLessThan(TimeSpan.FromSeconds(2));
        }

        [Fact]
        public async Task UpdateRates_CancelsIfNeed()
        {
            cts.Cancel();
            await Assert.ThrowsAsync<OperationCanceledException>(
                async () => await updater.UpdateRates(cts.Token));

            foreach (var provider in providers)
            {
                provider.Invoked.Should().Be(false);
            }
        }
        
        [Fact]
        public async Task UpdateRates_WritesResultProperly()
        {
            var allRatesInRightOrder = providers
                .Where(p => !p.ActualInvokeCase.ThrowException)
                .SelectMany(p => p.ActualInvokeCase.Response).ToArray();

            var actualRates = new Dictionary<string, CurrencyRate>();
            foreach (var rate in allRatesInRightOrder)
            {
                actualRates[rate.Key] = rate;
            }

            await updater.UpdateRates(cts.Token);
            
            writer.Verify(w => w.SetActualRates(It.Is<ICollection<CurrencyRate>>(c => c.IsEquivalent(actualRates.Values, false))), Times.Once);
            writer.Verify(w => w.AddRatesToHistory(It.Is<ICollection<CurrencyRate>>(c => c.IsEquivalent(actualRates.Values, false))), Times.Once);
            writer.Verify(w => w.SaveChanges(), Times.Once);
        }
    }
}