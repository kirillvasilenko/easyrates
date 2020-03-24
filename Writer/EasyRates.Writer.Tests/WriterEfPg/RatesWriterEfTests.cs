using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using EasyRates.Model;
using EasyRates.Model.Ef.Pg;
using EasyRates.Writer.Ef.Pg;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EasyRates.Writer.Tests.WriterEfPg
{
    public class RatesWriterEfTests
    {
        private readonly Fixture fixture = new Fixture();

        private readonly RatesWriterEf writer;

        private readonly RatesContext context;

        private readonly IMapper mapper;
        
        public RatesWriterEfTests()
        {
            var opts = new DbContextOptionsBuilder<RatesContext>()
                .UseInMemoryDatabase(fixture.Create<string>())
                .Options;
            
            var provider = new ServiceCollection()
                .AddEasyRatesModel()
                .BuildServiceProvider();
            
            mapper = provider.GetService<IMapper>();

            context = new RatesContext(opts);
            writer = new RatesWriterEf(context, mapper);
        }

        [Fact]
        public async Task AddRatesToHistory()
        {
            var rates = fixture.CreateMany<CurrencyRate>().ToList();
            var expectedResult = rates.Select(r => r.ToHistoryItem(mapper));
            
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