using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using EasyRates.Model;
using EasyRates.RatesProvider.OpenExchange;
using EasyRates.Tests.Common;
using EasyRates.Writer;
using EasyRates.WriterApp;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EasyRates.Tests.App.WriterApp
{
    public class RatesProviderTest : IRatesProvider
    {
        
        

        public RatesProviderTest(
            string name,
            int priority,
            InvokeCase invokeCase)
        {
            ActualInvokeCase  = invokeCase;
            Name = name;
            Priority = priority;
        }
        
        public InvokeCase ActualInvokeCase { get; }
        
        public int Priority { get; }
        
        public string Name { get; }
        
        public bool Invoked { get; private set; }
        
        public async Task<CurrencyRate[]> GetAllRates()
        {
            Invoked = true;
            await Task.Delay(ActualInvokeCase.EmulationTime);
            if (ActualInvokeCase.ThrowException)
            {
                throw new Exception();
            }
            return ActualInvokeCase.Response;
        }
        
        public class InvokeCase
        {
            public TimeSpan EmulationTime { get; set; }
        
            public bool ThrowException { get; set; }
        
            public CurrencyRate[] Response { get; set; }
        }

        
    }
    
    public class RatesUpdaterTests
    {
        private Fixture fixture = new Fixture();
        
        private Random r = new Random();

        private RatesUpdater updater;
        
        private Mock<IRatesWriter> writer = new Mock<IRatesWriter>();
        
        private Mock<ICurrencyRateProcessor> processor = new Mock<ICurrencyRateProcessor>(); 

        private Mock<ILogger<RatesUpdater>> logger = new Mock<ILogger<RatesUpdater>>();
        
        private CancellationTokenSource cts = new CancellationTokenSource();

        private Mock<IOrderedRatesProviders> orderedProviders = new Mock<IOrderedRatesProviders>();

        private RatesProviderTest[] providers;
        
        public RatesUpdaterTests()
        {
            providers = new[]
            {
                new RatesProviderTest(fixture.Create<string>(), r.Next(0,10),
                    new RatesProviderTest.InvokeCase
                    {
                        EmulationTime = TimeSpan.FromSeconds(1),
                        ThrowException = fixture.Create<bool>(),
                        Response = fixture.CreateMany<CurrencyRate>().ToArray()
                    }),
                new RatesProviderTest(fixture.Create<string>(), r.Next(0,10),
                    new RatesProviderTest.InvokeCase
                    {
                        EmulationTime = TimeSpan.FromSeconds(1),
                        ThrowException = fixture.Create<bool>(),
                        Response = fixture.CreateMany<CurrencyRate>().ToArray()
                    }),
                new RatesProviderTest(fixture.Create<string>(), r.Next(0,10),
                    new RatesProviderTest.InvokeCase
                    {
                        EmulationTime = TimeSpan.FromSeconds(1),
                        ThrowException = fixture.Create<bool>(),
                        Response = fixture.CreateMany<CurrencyRate>().ToArray()
                    }),
            };
            orderedProviders.Setup(p => p.GetProviders()).Returns(providers);
            
            updater = new RatesUpdater(writer.Object, orderedProviders.Object, processor.Object, logger.Object);
        }

        [Fact]
        public async Task UpdateRates_InvokesProvidersAsynchronously()
        {
            var watch = new Stopwatch();
            
            watch.Start();
            await updater.UpdateRates(cts.Token);
            watch.Stop();

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