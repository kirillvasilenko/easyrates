using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using EasyRates.Model;
using EasyRates.Reader.Model;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Time;
using Xunit;

namespace EasyRates.Tests.Domain.Reader
{
    public class RatesReaderWithCacheTests
    {
        private Fixture fixture = new Fixture();

        private RatesReaderWithCache reader;
        
        private Mock<IRatesReaderRepo> repo = new Mock<IRatesReaderRepo>(MockBehavior.Strict);
        
        private Mock<IMemoryCache> cache = new Mock<IMemoryCache>(MockBehavior.Strict);

        private TimeProviderTest time;
        
        public RatesReaderWithCacheTests()
        {
            time = TimeProviderTest.SetAndGetTimeProviderTestInstanceIfNeed();
            reader = new RatesReaderWithCache(repo.Object, cache.Object);
        }

        [Fact]
        public async Task GetRate_TakesFromCache_IfCacheHasIt()
        {
            var from = fixture.Create<string>();
            var to = fixture.Create<string>();
            object rate = fixture.Create<CurrencyRate>();
            
            cache.Setup(c => c.TryGetValue(from + to, out rate)).Returns(true);

            var result = await reader.GetRate(from, to);

            result.Should().Be(rate);
        }

        [Fact]
        public async Task GetRate_TakesFromRepoAndPutToCache_IfCacheDoesntHave()
        {
            var from = fixture.Create<string>();
            var to = fixture.Create<string>();
            var rate = fixture.Create<CurrencyRate>();
            var cacheEntry = new Mock<ICacheEntry>();
            var expirationTimeExpectedInCache =
                rate.ExpirationTime.ToAbsoluteExpirationRelativeNow(TimeSpan.FromSeconds(5));

            object cacheResult = null;
            cache.Setup(c => c.TryGetValue(from + to, out cacheResult)).Returns(false);
            repo.Setup(r => r.GetRate(from, to)).ReturnsAsync(rate);
            cache.Setup(c => c.CreateEntry(from + to)).Returns(cacheEntry.Object);
            
            var result = await reader.GetRate(from, to);

            result.Should().Be(rate);
            
            cache.Verify(c => c.CreateEntry(from + to), Times.Once);
            cacheEntry.VerifySet(c => c.AbsoluteExpirationRelativeToNow = expirationTimeExpectedInCache);
            
        }
        
        
        [Fact]
        public async Task GetRates_TakesFromCache_IfCacheHasIt()
        {
            var from = fixture.Create<string>();
            object rates = fixture.Create<CurrencyRate[]>();
            
            cache.Setup(c => c.TryGetValue(from, out rates)).Returns(true);

            var result = await reader.GetRates(from);

            Assert.Same(rates, result);
        }

        [Fact]
        public async Task GetRates_TakesFromRepoAndPutToCache_IfCacheDoesntHave()
        {
            var from = fixture.Create<string>();
            var rates = fixture.Create<CurrencyRate[]>();
            var cacheEntry = new Mock<ICacheEntry>();
            var expirationTimeExpectedInCache =
                rates.First().ExpirationTime.ToAbsoluteExpirationRelativeNow(TimeSpan.FromSeconds(5));

            object cacheResult = null;
            cache.Setup(c => c.TryGetValue(from, out cacheResult)).Returns(false);
            repo.Setup(r => r.GetRates(from)).ReturnsAsync(rates);
            cache.Setup(c => c.CreateEntry(from)).Returns(cacheEntry.Object);
            
            var result = await reader.GetRates(from);

            Assert.Same(rates, result);
            
            cache.Verify(c => c.CreateEntry(from), Times.Once);
            cacheEntry.VerifySet(c => c.AbsoluteExpirationRelativeToNow = expirationTimeExpectedInCache);
        }
    }
}