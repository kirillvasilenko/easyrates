using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using EasyRates.Model;
using EasyRates.Model.Ef.Pg;
using EasyRates.Reader.Ef.Pg;
using EasyRates.Reader.Model;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EasyRates.Reader.Tests.ReaderEfPg
{
    public class RatesReaderRepoEfTests
    {
        private readonly Fixture fixture = new Fixture();

        private readonly RatesReaderRepoEf repo;

        private readonly RatesContext context;
        
        public RatesReaderRepoEfTests()
        {
            var opts = new DbContextOptionsBuilder<RatesContext>()
                .UseInMemoryDatabase(fixture.Create<string>())
                .Options;
            
            context = new RatesContext(opts);;
            repo = new RatesReaderRepoEf(context);
        }

        [Fact]
        public async Task GetRate_GetsFromContext()
        {
            var rate = fixture.Create<CurrencyRate>();

            context.ActualRates.Add(rate);
            context.SaveChanges();
            
            var result = await repo.GetRate(rate.CurrencyFrom, rate.CurrencyTo);

            result.Should().Be(rate);
        }
        
        [Fact]
        public async Task GetRate_ThrowException_IfContextNotHave()
        {
            await Assert.ThrowsAsync<RateNotFoundException>(
                async () => await repo.GetRate(fixture.Create<string>(), fixture.Create<string>()));
            
        }
        
        [Fact]
        public async Task GetRates_GetsFromContext()
        {
            var from = fixture.Create<string>();
            var rates = fixture.Build<CurrencyRate>()
                .With(r => r.CurrencyFrom, from)
                .CreateMany()
                .ToList();

            context.ActualRates.AddRange(rates);
            context.SaveChanges();
            
            var result = await repo.GetRates(from);

            result.Should().BeEquivalentTo(rates);
        }
        
        [Fact]
        public async Task GetRates_ThrowException_IfContextNotHave()
        {
            await Assert.ThrowsAsync<NoOneRateFoundException>(
                async () => await repo.GetRates(fixture.Create<string>()));
            
        }
    }
}