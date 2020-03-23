using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using EasyRates.Model;
using EasyRates.Model.Ef;
using EasyRates.Writer.Ef;
using EfCore.Common;
using FluentAssertions;
using Xunit;

namespace EasyRates.Tests.Domain.WriterEf
{
    public class RatesWriterEfTests
    {
        private Fixture fixture = new Fixture();

        private RatesWriterEf writer;

        private RatesContext context;
        
        public RatesWriterEfTests()
        {
            context = new RatesContext(new RatesDbParams
            {
                ConnectionString = fixture.Create<string>(),
                DbType = DbType.InMemory
            });
            writer = new RatesWriterEf(context);
        }

        [Fact]
        public async Task AddRatesToHistory()
        {
            var rates = fixture.CreateMany<CurrencyRate>().ToList();
            var expectedResult = rates.Select(r => r.ToHistoryItem());
            
            await writer.AddRatesToHistory(rates);

            context.RatesHistory.Should().BeEmpty();
            
            await writer.SaveChanges();

            context.RatesHistory.Should().BeEquivalentTo(expectedResult, c =>
            {
                return c.Excluding(d => d.Id);
            });
        }

        [Fact]
        public async Task SetActualRates_UpdateOldRates_And_AddNewRates()
        {
            var oldRate = fixture.Create<CurrencyRate>();
            
            context.ActualRates.Add(oldRate);
            context.SaveChanges();

            var rates = fixture.CreateMany<CurrencyRate>().ToList();
            var updatedOldRate = rates.First();
            updatedOldRate.From = oldRate.From;
            updatedOldRate.To = oldRate.To;
            
            await writer.SetActualRates(rates);

            context.ActualRates.Should().HaveCount(1);

            await writer.SaveChanges();

            context.ActualRates.Should().BeEquivalentTo(rates);
            oldRate.Should().BeEquivalentTo(updatedOldRate);
        }
    }
}