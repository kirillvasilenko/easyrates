using AutoFixture;
using AutoMapper;
using EasyRates.Model;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace EasyRates.Model.Tests
{
    public class CurrencyRateExtensionTests
    {
        private readonly Fixture fixture = new Fixture();

        private readonly Mock<IMapper> mapper = new Mock<IMapper>(MockBehavior.Strict);
        
        [Fact]
        public void UpdateRateFromAnotherOne()
        {
            var destination = fixture.Create<CurrencyRate>();
            var source = fixture.Create<CurrencyRate>();
            
            mapper.Setup(x => x.Map(source, destination)).Returns(destination);
            
            destination.Update(source, mapper.Object);
            
            mapper.Verify(x => x.Map(source, destination));
        }

        [Fact]
        public void MakesRateHistoryItemFromRate()
        {
            var rate = fixture.Create<CurrencyRate>();
            var historyItem = fixture.Create<CurrencyRateHistoryItem>();

            mapper.Setup(x => x.Map<CurrencyRateHistoryItem>(rate)).Returns(historyItem);
            
            rate.ToHistoryItem(mapper.Object);
            
            mapper.Verify(x => x.Map<CurrencyRateHistoryItem>(rate));
        }
    }
}