using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using EasyRates.Model;
using EasyRates.Model.Ef;
using EasyRates.Reader.Model;
using EfCore.Common;
using FluentAssertions;
using Xunit;

namespace EasyRates.Tests.Domain.ReaderEf
{
    public class RatesReaderRepoEfTests
    {
        private Fixture fixture = new Fixture();

        private RatesReaderRepoEf repo;

        private RatesContext context;
        
        public RatesReaderRepoEfTests()
        {
            context = new RatesContext(new RatesDbParams
            {
                ConnectionString = "test",
                DbType = DbType.InMemory
            });
            repo = new RatesReaderRepoEf(context);
        }

        [Fact]
        public async Task GetRate_GetsFromContext()
        {
            var rate = fixture.Create<CurrencyRate>();

            context.ActualRates.Add(rate);
            context.SaveChanges();
            
            var result = await repo.GetRate(rate.From, rate.To);

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
                .With(r => r.From, from)
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